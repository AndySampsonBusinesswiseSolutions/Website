using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace ValidateUsageUploadTempSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateUsageUploadTempSiteDataController : ControllerBase
    {
        private readonly ILogger<ValidateUsageUploadTempSiteDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 validateUsageUploadTempSiteDataAPIId;

        public ValidateUsageUploadTempSiteDataController(ILogger<ValidateUsageUploadTempSiteDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateUsageUploadTempSiteDataAPI, _systemAPIPasswordEnums.ValidateUsageUploadTempSiteDataAPI);
            validateUsageUploadTempSiteDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateUsageUploadTempSiteDataAPI);
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateUsageUploadTempSiteDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempSiteData/Validate")]
        public void Validate([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    validateUsageUploadTempSiteDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateUsageUploadTempSiteDataAPI, validateUsageUploadTempSiteDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.Customer].[Site] table
                var customerDataRows = _tempCustomerMethods.Site_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempSiteDataAPIId, false, null);
                    return;
                }
                
                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"SiteName", "Site Name"}
                    };
                
                var errors = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns).ToList();

                //Validate post code
                var invalidPostCodeDataRows = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("SitePostCode")) 
                    && !_methods.IsValidPostCode(r.Field<string>("SitePostCode")));

                foreach(var invalidPostCodeDataRow in invalidPostCodeDataRows)
                {
                    errors.Add($"Invalid Site Post Code '{invalidPostCodeDataRow["SitePostCode"]}' in row {invalidPostCodeDataRow["RowId"]}");
                }

                //Validate telephone number
                var invalidTelephoneNumberDataRows = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContactTelephoneNumber")) 
                    && !_methods.IsValidPhoneNumber(r.Field<string>("ContactTelephoneNumber")));

                foreach(var invalidTelephoneNumberDataRow in invalidTelephoneNumberDataRows)
                {
                    errors.Add($"Invalid Contact Telephone Number '{invalidTelephoneNumberDataRow["ContactTelephoneNumber"]}' in row {invalidTelephoneNumberDataRow["RowId"]}");
                }

                //Validate email address
                var invalidEmailAddressDataRows = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContactEmailAddress")) 
                    && !_methods.IsValidEmailAddress(r.Field<string>("ContactEmailAddress")));

                foreach(var invalidEmailAddressDataRow in invalidEmailAddressDataRows)
                {
                    errors.Add($"Invalid Contact Email Address '{invalidEmailAddressDataRow["ContactEmailAddress"]}' in row {invalidEmailAddressDataRow["RowId"]}");
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempSiteDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempSiteDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

