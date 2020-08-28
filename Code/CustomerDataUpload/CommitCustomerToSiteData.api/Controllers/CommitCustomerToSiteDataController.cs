using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace CommitCustomerToSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitCustomerToSiteDataController : ControllerBase
    {
        private readonly ILogger<CommitCustomerToSiteDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Attribute _customerAttributeEnums = new Enums.Customer.Attribute();
        private readonly Enums.Customer.Site.Attribute _customerSiteAttributeEnums = new Enums.Customer.Site.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitCustomerToSiteDataAPIId;

        public CommitCustomerToSiteDataController(ILogger<CommitCustomerToSiteDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitCustomerToSiteDataAPI, _systemAPIPasswordEnums.CommitCustomerToSiteDataAPI);
            commitCustomerToSiteDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitCustomerToSiteDataAPI);
        }

        [HttpPost]
        [Route("CommitCustomerToSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitCustomerToSiteDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitCustomerToSiteData/Commit")]
        public void Commit([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
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

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitCustomerToSiteDataAPI, commitCustomerToSiteDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitCustomerToSiteDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Site] where CanCommit = 1
                var siteDataRows = _tempCustomerDataUploadMethods.Site_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(siteDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCustomerToSiteDataAPIId, false, null);
                    return;
                }

                var customerNameCustomerAttributeId = _customerMethods.CustomerAttribute_GetCustomerAttributeIdByCustomerAttributeDescription(_customerAttributeEnums.CustomerName);
                var siteNameSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SiteName);
                var sitePostCodeSiteAttributeId = _customerMethods.SiteAttribute_GetSiteAttributeIdBySiteAttributeDescription(_customerSiteAttributeEnums.SitePostCode);

                var customers = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.CustomerName))
                    .Distinct()
                    .ToDictionary(c => c, c => _customerMethods.CustomerDetail_GetCustomerDetailIdByCustomerAttributeIdAndCustomerDetailDescription(customerNameCustomerAttributeId, c));

                var siteNames = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SiteName))
                    .Distinct()
                    .ToDictionary(sn => sn, sn => _customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(siteNameSiteAttributeId, sn));

                var sitePostCodes = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SitePostCode))
                    .Distinct()
                    .ToDictionary(spc => spc, spc => _customerMethods.SiteDetail_GetSiteIdListBySiteAttributeIdAndSiteDetailDescription(sitePostCodeSiteAttributeId, spc));

                foreach(var dataRow in commitableDataRows)
                {
                    //Get CustomerId by CustomerName
                    var customerId = customers[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.CustomerName)];

                    //Get SiteId by SiteName and SitePostCode
                    var siteNameSiteIdList = siteNames[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.SiteName)];
                    var sitePostCodeSiteIdList = sitePostCodes[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.SitePostCode)];

                    var matchingSiteIdList = siteNameSiteIdList.Intersect(sitePostCodeSiteIdList);
                    var siteId = matchingSiteIdList.FirstOrDefault();

                    //Insert into [Mapping].[CustomerToSite]
                    _mappingMethods.CustomerToSite_Insert(createdByUserId, sourceId, customerId, siteId);
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

