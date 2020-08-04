﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace ValidateSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateSubMeterDataController : ControllerBase
    {
        private readonly ILogger<ValidateSubMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
        private readonly Int64 validateSubMeterDataAPIId;

        public ValidateSubMeterDataController(ILogger<ValidateSubMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateSubMeterDataAPI, _systemAPIPasswordEnums.ValidateSubMeterDataAPI);
            validateSubMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateSubMeterDataAPI);
        }

        [HttpPost]
        [Route("ValidateSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateSubMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateSubMeterData/Validate")]
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
                    validateSubMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateSubMeterDataAPI, validateSubMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[SubMeter] table
                var customerDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateSubMeterDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"SubMeterIdentifier", "SubMeter Name"}
                    };
                
                var records = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns);

                //Get submeters not stored in database
                var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);
                var newSubMeterDataRecords = customerDataRows.Where(r => 
                    _customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, r.Field<string>("SubMeterIdentifier")) == 0);

                //MPXN, SerialNumber, SubArea and Asset must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {"MPXN", "MPAN/MPRN"},
                        {"SerialNumber", "SubMeter Serial Number"},
                        {"SubArea", "SubArea"},
                        {"Asset", "Asset"}
                    };
                var newSubMeterErrors = _tempCustomerMethods.GetMissingRecords(newSubMeterDataRecords, requiredColumns);
                _tempCustomerMethods.AddErrorsToRecords(records, newSubMeterErrors);

                //Update Process Queue
                var errorMessage = _tempCustomerMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.SubMeter);
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateSubMeterDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

