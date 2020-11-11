using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CommitCustomerToSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitCustomerToSiteDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitCustomerToSiteDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Customer.Attribute _customerAttributeEnums = new Enums.CustomerSchema.Customer.Attribute();
        private readonly Enums.CustomerSchema.Site.Attribute _customerSiteAttributeEnums = new Enums.CustomerSchema.Site.Attribute();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitCustomerToSiteDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitCustomerToSiteDataController(ILogger<CommitCustomerToSiteDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitCustomerToSiteDataAPI, password);
            commitCustomerToSiteDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitCustomerToSiteDataAPI);
        }

        [HttpPost]
        [Route("CommitCustomerToSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitCustomerToSiteDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitCustomerToSiteData/Commit")]
        public void Commit([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            var customerDataUploadProcessQueueGUID = _systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitCustomerToSiteDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitCustomerToSiteDataAPI, commitCustomerToSiteDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitCustomerToSiteDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Site] where CanCommit = 1
                var siteEntities = new Methods.Temp.CustomerDataUpload.Site().Site_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSiteEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(siteEntities);

                if(!commitableSiteEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerToSiteDataAPIId, false, null);
                    return;
                }

                var customerNameCustomerAttributeId = _customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.CustomerName);
                var siteNameSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteName);
                var sitePostCodeSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SitePostCode);

                var customers = commitableSiteEntities.Select(cse => cse.CustomerName).Distinct()
                    .ToDictionary(c => c, c => _customerMethods.CustomerDetail_GetCustomerDetailIdByCustomerAttributeIdAndCustomerDetailDescription(customerNameCustomerAttributeId, c));

                var siteNames = commitableSiteEntities.Select(cse => cse.SiteName).Distinct()
                    .ToDictionary(sn => sn, sn => _customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(siteNameSiteAttributeId, sn));

                var sitePostCodes = commitableSiteEntities.Select(cse => cse.SitePostCode).Distinct()
                    .ToDictionary(spc => spc, spc => _customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(sitePostCodeSiteAttributeId, spc));

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
                        var customerToSiteId = _mappingMethods.CustomerToSite_GetCustomerToSiteIdByCustomerIdAndSiteId(customerId, siteId);

                        if(customerToSiteId == 0)
                        {
                            //Insert into [Mapping].[CustomerToSite]
                            _mappingMethods.CustomerToSite_Insert(createdByUserId, sourceId, customerId, siteId);
                        }
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerToSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerToSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}