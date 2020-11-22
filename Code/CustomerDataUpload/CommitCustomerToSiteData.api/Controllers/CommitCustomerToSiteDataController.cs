using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CommitCustomerToSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitCustomerToSiteDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitCustomerToSiteDataController> _logger;
        private readonly Int64 commitCustomerToSiteDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitCustomerToSiteDataController(ILogger<CommitCustomerToSiteDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitCustomerToSiteDataAPI, password);
            commitCustomerToSiteDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitCustomerToSiteDataAPI);
        }

        [HttpPost]
        [Route("CommitCustomerToSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitCustomerToSiteDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitCustomerToSiteData/Commit")]
        public void Commit([FromBody] object data)
        {
            var systemMethods = new Methods.SystemSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

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
                    commitCustomerToSiteDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitCustomerToSiteDataAPI, commitCustomerToSiteDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitCustomerToSiteDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Site] where CanCommit = 1
                var siteEntities = new Methods.TempSchema.CustomerDataUpload.Site().Site_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSiteEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(siteEntities);

                if(!commitableSiteEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerToSiteDataAPIId, false, null);
                    return;
                }

                var customerSiteAttributeEnums = new Enums.CustomerSchema.Site.Attribute();
                var mappingMethods = new Methods.MappingSchema();
                var customerMethods = new Methods.CustomerSchema();

                var customerNameCustomerAttributeId = customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(new Enums.CustomerSchema.Customer.Attribute().CustomerName);
                var siteNameSiteAttributeId = customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.SiteName);
                var sitePostCodeSiteAttributeId = customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(customerSiteAttributeEnums.SitePostCode);

                var customers = commitableSiteEntities.Select(cse => cse.CustomerName).Distinct()
                    .ToDictionary(c => c, c => customerMethods.CustomerDetail_GetCustomerDetailIdByCustomerAttributeIdAndCustomerDetailDescription(customerNameCustomerAttributeId, c));

                var siteNames = commitableSiteEntities.Select(cse => cse.SiteName).Distinct()
                    .ToDictionary(sn => sn, sn => customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(siteNameSiteAttributeId, sn));

                var sitePostCodes = commitableSiteEntities.Select(cse => cse.SitePostCode).Distinct()
                    .ToDictionary(spc => spc, spc => customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(sitePostCodeSiteAttributeId, spc));

                foreach(var siteEntity in commitableSiteEntities)
                {
                    //Get CustomerId by CustomerName
                    var customerId = customers[siteEntity.CustomerName];

                    //Get SiteId by SiteName and SitePostCode
                    var siteNameSiteIdList = siteNames[siteEntity.SiteName];
                    var sitePostCodeSiteIdList = sitePostCodes[siteEntity.SitePostCode];

                    var matchingSiteIdList = siteNameSiteIdList.Intersect(sitePostCodeSiteIdList);

                    foreach(var siteId in matchingSiteIdList)
                    {
                        var customerToSiteId = mappingMethods.CustomerToSite_GetCustomerToSiteIdByCustomerIdAndSiteId(customerId, siteId);

                        if(customerToSiteId == 0)
                        {
                            //Insert into [Mapping].[CustomerToSite]
                            mappingMethods.CustomerToSite_Insert(createdByUserId, sourceId, customerId, siteId);
                        }
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerToSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerToSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}