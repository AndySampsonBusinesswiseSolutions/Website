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
using Microsoft.Extensions.Configuration;

namespace ValidateCustomerData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateCustomerDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateCustomerDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private static readonly Enums.CustomerSchema.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.CustomerSchema.DataUploadValidation.SheetName();
        private static readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 validateCustomerDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateCustomerDataController(ILogger<ValidateCustomerDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateCustomerDataAPI, password);
            validateCustomerDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateCustomerDataAPI);
        }

        [HttpPost]
        [Route("ValidateCustomerData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateCustomerDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateCustomerData/Validate")]
        public void Validate([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
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

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateCustomerDataAPI, validateCustomerDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateCustomerDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Customer] table
                var tempCustomerDataUploadCustomerMethods = new Methods.Temp.CustomerDataUpload.Customer();
                var customerEntities = tempCustomerDataUploadCustomerMethods.Customer_GetByProcessQueueGUID(processQueueGUID);

                if(!customerEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateCustomerDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();

                var columns = new List<string>
                    {
                        _customerDataUploadValidationEntityEnums.CustomerName,
                        _customerDataUploadValidationEntityEnums.ContactName,
                        _customerDataUploadValidationEntityEnums.ContactTelephoneNumber,
                        _customerDataUploadValidationEntityEnums.ContactEmailAddress,
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(customerEntities.Select(ce => ce.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.CustomerName, "Customer Name"},
                        {_customerDataUploadValidationEntityEnums.ContactName, "Contact Name"},
                        {_customerDataUploadValidationEntityEnums.ContactTelephoneNumber, "Contact Telephone Number"},
                        {_customerDataUploadValidationEntityEnums.ContactEmailAddress, "Contact Email Address"}
                    };
                
                _tempCustomerDataUploadMethods.GetMissingRecords(records, customerEntities, requiredColumns);

                //Validate telephone number
                var invalidTelephoneNumberDataRows = customerEntities.Where(ce => !string.IsNullOrWhiteSpace(ce.ContactTelephoneNumber) 
                    && !methods.IsValidPhoneNumber(ce.ContactTelephoneNumber));

                foreach(var invalidTelephoneNumberDataRow in invalidTelephoneNumberDataRows)
                {
                    if(!records[invalidTelephoneNumberDataRow.RowId.Value][_customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Contains($"Invalid Contact Telephone Number '{invalidTelephoneNumberDataRow.ContactTelephoneNumber}'"))
                    {
                        records[invalidTelephoneNumberDataRow.RowId.Value][_customerDataUploadValidationEntityEnums.ContactTelephoneNumber].Add($"Invalid Contact Telephone Number '{invalidTelephoneNumberDataRow.ContactTelephoneNumber}'");
                    }
                }

                //Validate email address
                var invalidEmailAddressDataRows = customerEntities.Where(ce => !string.IsNullOrWhiteSpace(ce.ContactEmailAddress) 
                    && !methods.IsValidEmailAddress(ce.ContactEmailAddress));

                foreach(var invalidEmailAddressDataRow in invalidEmailAddressDataRows)
                {
                    if(!records[invalidEmailAddressDataRow.RowId.Value][_customerDataUploadValidationEntityEnums.ContactEmailAddress].Contains($"Invalid Contact Email Address '{invalidEmailAddressDataRow.ContactEmailAddress}'"))
                    {
                        records[invalidEmailAddressDataRow.RowId.Value][_customerDataUploadValidationEntityEnums.ContactEmailAddress].Add($"Invalid Contact Email Address '{invalidEmailAddressDataRow.ContactEmailAddress}'");
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.Customer);
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateCustomerDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateCustomerDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}