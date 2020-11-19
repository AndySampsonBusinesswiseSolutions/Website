using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace CommitMeterToSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToSiteDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterToSiteDataController> _logger;
        private readonly Int64 commitMeterToSiteDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterToSiteDataController(ILogger<CommitMeterToSiteDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToSiteDataAPI, password);
            commitMeterToSiteDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterToSiteDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(commitMeterToSiteDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToSiteData/Commit")]
        public void Commit([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitMeterToSiteDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitMeterToSiteDataAPI, commitMeterToSiteDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterToSiteDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterDataRows = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.Temp.CustomerDataUpload().GetCommitableEntities(meterDataRows);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToSiteDataAPIId, false, null);
                    return;
                }

                var customerSiteAttributeEnums = new Enums.CustomerSchema.Site.Attribute();
                var mappingMethods = new Methods.Mapping();
                var customerMethods = new Methods.Customer();

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
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}