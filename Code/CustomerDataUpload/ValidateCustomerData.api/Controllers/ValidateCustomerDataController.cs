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
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
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
                var customerDataRows = _tempCustomerDataUploadMethods.Customer_GetByProcessQueueGUID(processQueueGUID);

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateCustomerDataAPIId, false, null);
                    return;
                }

                var columns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.CustomerName, "Customer Name"},
                        {_customerDataUploadValidationEntityEnums.ContactName, "Contact Name"},
                        {_customerDataUploadValidationEntityEnums.ContactTelephoneNumber, "Contact Telephone Number"},
                        {_customerDataUploadValidationEntityEnums.ContactEmailAddress, "Contact Email Address"}
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(customerDataRows, columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.CustomerName, "Customer Name"},
                        {_customerDataUploadValidationEntityEnums.ContactName, "Contact Name"},
                        {_customerDataUploadValidationEntityEnums.ContactTelephoneNumber, "Contact Telephone Number"},
                        {_customerDataUploadValidationEntityEnums.ContactEmailAddress, "Contact Email Address"}
                    };
                
                _tempCustomerDataUploadMethods.GetMissingRecords(records, customerDataRows, requiredColumns);

                //Validate telephone number
                var invalidTelephoneNumberDataRows = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContactTelephoneNumber)) 
                    && !_methods.IsValidPhoneNumber(r.Field<string>(_customerDataUploadValidationEntityEnums.ContactTelephoneNumber)));

                foreach(var invalidTelephoneNumberDataRow in invalidTelephoneNumberDataRows)
                {
                    var rowId = Convert.ToInt32(invalidTelephoneNumberDataRow["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Contains($"Invalid Contact Telephone Number '{invalidTelephoneNumberDataRow[_customerDataUploadValidationEntityEnums.ContactTelephoneNumber]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Add($"Invalid Contact Telephone Number '{invalidTelephoneNumberDataRow[_customerDataUploadValidationEntityEnums.ContactTelephoneNumber]}'");
                    }
                }

                //Validate email address
                var invalidEmailAddressDataRows = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContactEmailAddress)) 
                    && !_methods.IsValidEmailAddress(r.Field<string>(_customerDataUploadValidationEntityEnums.ContactEmailAddress)));

                foreach(var invalidEmailAddressDataRow in invalidEmailAddressDataRows)
                {
                    var rowId = Convert.ToInt32(invalidEmailAddressDataRow["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.ContactEmailAddress].Contains($"Invalid Contact Email Address '{invalidEmailAddressDataRow[_customerDataUploadValidationEntityEnums.ContactEmailAddress]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.ContactEmailAddress].Add($"Invalid Contact Email Address '{invalidEmailAddressDataRow[_customerDataUploadValidationEntityEnums.ContactEmailAddress]}'");
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.Customer);
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