using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace ValidateUsageUploadTempCustomerData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateUsageUploadTempCustomerDataController : ControllerBase
    {
        private readonly ILogger<ValidateUsageUploadTempCustomerDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Int64 validateUsageUploadTempCustomerDataAPIId;

        public ValidateUsageUploadTempCustomerDataController(ILogger<ValidateUsageUploadTempCustomerDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateUsageUploadTempCustomerDataAPI, _systemAPIPasswordEnums.ValidateUsageUploadTempCustomerDataAPI);
            validateUsageUploadTempCustomerDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateUsageUploadTempCustomerDataAPI);
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempCustomerData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());            
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(validateUsageUploadTempCustomerDataAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempCustomerData/Validate")]
        public void Validate([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    validateUsageUploadTempCustomerDataAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.ValidateUsageUploadTempCustomerDataAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempCustomerDataAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get data from [Temp.Customer].[Customer] table
                var customerDataRows = _tempCustomerMethods.Customer_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempCustomerDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"CustomerName", "Customer Name"},
                        {"ContactName", "Contact Name"},
                        {"ContactTelephoneNumber", "Contact Telephone Number"},
                        {"ContactEmailAddress", "Contact Email Address"}
                    };
                
                var errors = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns).ToList();

                //If Customer already exists, check that User is linked to Customer
                //If not, then reject - this is to stop people updating details of other customers

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
                var errorMessage = errors.Any() ? string.Join(';', errors) : null;
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempCustomerDataAPIId, errors.Any(), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempCustomerDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

