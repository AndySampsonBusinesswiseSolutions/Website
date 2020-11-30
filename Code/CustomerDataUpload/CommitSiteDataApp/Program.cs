using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CommitSiteDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "vZ3RFaCBjSLCbFwc";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitSiteDataAPI, password);
                var commitSiteDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitSiteDataAPI);

                var systemMethods = new Methods.SystemSchema();

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitSiteDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitSiteDataAPI, commitSiteDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitSiteDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Site] where CanCommit = 1
                var siteEntities = new Methods.TempSchema.CustomerDataUpload.Site().Site_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSiteEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(siteEntities);

                if(!commitableSiteEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSiteDataAPIId, false, null);
                    return;
                }

                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var customerSiteAttributeEnums = new Enums.CustomerSchema.Site.Attribute();
                var customerMethods = new Methods.CustomerSchema();

                var siteNameSiteAttributeId = customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.SiteName);
                var sitePostCodeSiteAttributeId = customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.SitePostCode);

                //For each column, get CustomerAttributeId
                var attributes = new Dictionary<long, string>
                {
                    {siteNameSiteAttributeId, customerDataUploadValidationEntityEnums.SiteName},
                    {customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.SiteAddress), customerDataUploadValidationEntityEnums.SiteAddress},
                    {customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.SiteTown), customerDataUploadValidationEntityEnums.SiteTown},
                    {customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.SiteCounty), customerDataUploadValidationEntityEnums.SiteCounty},
                    {sitePostCodeSiteAttributeId, customerDataUploadValidationEntityEnums.SitePostCode},
                    {customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.SiteDescription), customerDataUploadValidationEntityEnums.SiteDescription},
                    {customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.ContactName), customerDataUploadValidationEntityEnums.ContactName},
                    {customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.ContactRole), customerDataUploadValidationEntityEnums.ContactRole},
                    {customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.ContactTelephoneNumber), customerDataUploadValidationEntityEnums.ContactTelephoneNumber},
                    {customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.ContactEmailAddress), customerDataUploadValidationEntityEnums.ContactEmailAddress},
                };

                foreach(var siteEntity in commitableSiteEntities)
                {
                    var detailDictionary = attributes.ToDictionary(
                        a => a.Key,
                        a => siteEntity.GetType().GetProperty(a.Value).GetValue(siteEntity).ToString()
                    );

                    //Get SiteId by SiteName and SitePostCode
                    var siteNameSiteIdList = customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(siteNameSiteAttributeId, detailDictionary[siteNameSiteAttributeId]);
                    var sitePostCodeSiteIdList = customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(sitePostCodeSiteAttributeId, detailDictionary[sitePostCodeSiteAttributeId]);

                    var matchingSiteIdList = siteNameSiteIdList.Intersect(sitePostCodeSiteIdList);
                    var siteId = matchingSiteIdList.FirstOrDefault();

                    if(siteId == 0)
                    {
                        siteId = customerMethods.InsertNewSite(createdByUserId, sourceId);

                        //Insert into [Site].[SiteDetail]
                        foreach(var detail in detailDictionary)
                        {
                            customerMethods.SiteDetail_Insert(createdByUserId, sourceId, siteId, detail.Key, detail.Value);
                        }
                    }
                    else
                    {
                        //Update [Site].[SiteDetail]
                        foreach(var detail in detailDictionary)
                        {
                            var currentDetailEntity = customerMethods.SiteDetail_GetBySiteIdAndSiteAttributeId(siteId, detail.Key);

                            if(detail.Value != currentDetailEntity.SiteDetailDescription)
                            {
                                customerMethods.SiteDetail_DeleteBySiteDetailId(currentDetailEntity.SiteDetailId);
                                customerMethods.SiteDetail_Insert(createdByUserId, sourceId, siteId, detail.Key, detail.Value);
                            }
                        }
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
