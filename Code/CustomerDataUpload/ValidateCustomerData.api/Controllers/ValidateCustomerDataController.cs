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

namespace ValidateCustomerData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateCustomerDataController : ControllerBase
    {
        private readonly ILogger<ValidateCustomerDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private readonly Int64 validateCustomerDataAPIId;

        public ValidateCustomerDataController(ILogger<ValidateCustomerDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateCustomerDataAPI, _systemAPIPasswordEnums.ValidateCustomerDataAPI);
            validateCustomerDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateCustomerDataAPI);
        }

        [HttpPost]
        [Route("ValidateCustomerData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateCustomerDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateCustomerData/Validate")]
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
                    validateCustomerDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateCustomerDataAPI, validateCustomerDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[Customer] table
                var customerDataRows = _tempCustomerMethods.Customer_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateCustomerDataAPIId, false, null);
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
                
                var records = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns);

                //Validate telephone number
                var invalidTelephoneNumberDataRows = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContactTelephoneNumber")) 
                    && !_methods.IsValidPhoneNumber(r.Field<string>("ContactTelephoneNumber")));

                foreach(var invalidTelephoneNumberDataRow in invalidTelephoneNumberDataRows)
                {
                    var rowId = Convert.ToInt32(invalidTelephoneNumberDataRow["RowId"]);
                    records[rowId]["ContactTelephoneNumber"].Add($"Invalid Contact Telephone Number '{invalidTelephoneNumberDataRow["ContactTelephoneNumber"]}'");
                }

                //Validate email address
                var invalidEmailAddressDataRows = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContactEmailAddress")) 
                    && !_methods.IsValidEmailAddress(r.Field<string>("ContactEmailAddress")));

                foreach(var invalidEmailAddressDataRow in invalidEmailAddressDataRows)
                {
                    var rowId = Convert.ToInt32(invalidEmailAddressDataRow["RowId"]);
                    records[rowId]["ContactEmailAddress"].Add($"Invalid Contact Email Address '{invalidEmailAddressDataRow["ContactEmailAddress"]}'");
                }

                //Update Process Queue
                var errorMessage = _tempCustomerMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.Customer);
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateCustomerDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateCustomerDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}