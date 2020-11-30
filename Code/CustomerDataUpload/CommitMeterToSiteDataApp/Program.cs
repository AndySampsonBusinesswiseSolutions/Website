using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitMeterToSiteDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "XRvxMrsZ6metMPUB";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToSiteDataAPI, password);
                var commitMeterToSiteDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterToSiteDataAPI);

                var systemMethods = new Methods.SystemSchema();

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitMeterToSiteDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitMeterToSiteDataAPI, commitMeterToSiteDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterToSiteDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterDataRows = new Methods.TempSchema.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(meterDataRows);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToSiteDataAPIId, false, null);
                    return;
                }

                var customerSiteAttributeEnums = new Enums.CustomerSchema.Site.Attribute();
                var mappingMethods = new Methods.MappingSchema();
                var customerMethods = new Methods.CustomerSchema();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var siteNameSiteAttributeId = customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.SiteName);
                var sitePostCodeSiteAttributeId = customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.SitePostCode);

                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                var siteNames = commitableMeterEntities.Select(cme => cme.SiteName).Distinct()
                    .ToDictionary(sn => sn, sn => customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(siteNameSiteAttributeId, sn));

                var sitePostCodes = commitableMeterEntities.Select(cme => cme.SitePostCode).Distinct()
                    .ToDictionary(spc => spc, spc => customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(sitePostCodeSiteAttributeId, spc));

                foreach(var meterEntity in commitableMeterEntities)
                {
                    //Get MeterId by MPXN
                    var meterId = meters[meterEntity.MPXN];

                    //Get SiteId by SiteName and SitePostCode
                    var siteNameSiteIdList = siteNames[meterEntity.SiteName];
                    var sitePostCodeSiteIdList = sitePostCodes[meterEntity.SitePostCode];

                    var matchingSiteIdList = siteNameSiteIdList.Intersect(sitePostCodeSiteIdList);
                    var siteId = matchingSiteIdList.FirstOrDefault();

                    //Get existing MeterToSite Id
                    var existingMeterToSiteId = mappingMethods.MeterToSite_GetMetertoSiteIdByMeterIdAndSiteId(meterId, siteId);

                    if(existingMeterToSiteId == 0)
                    {
                        //Insert into [Mapping].[MeterToSite]
                        mappingMethods.MeterToSite_Insert(createdByUserId, sourceId, meterId, siteId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
