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

namespace ValidateUsageUploadTempMeterExempionData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateUsageUploadTempMeterExempionDataController : ControllerBase
    {
        private readonly ILogger<ValidateUsageUploadTempMeterExempionDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 validateUsageUploadTempMeterExempionDataAPIId;

        public ValidateUsageUploadTempMeterExempionDataController(ILogger<ValidateUsageUploadTempMeterExempionDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateUsageUploadTempMeterExempionDataAPI, _systemAPIPasswordEnums.ValidateUsageUploadTempMeterExempionDataAPI);
            validateUsageUploadTempMeterExempionDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateUsageUploadTempMeterExempionDataAPI);
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempMeterExempionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateUsageUploadTempMeterExempionDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempMeterExempionData/Validate")]
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
                    validateUsageUploadTempMeterExempionDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateUsageUploadTempMeterExempionDataAPI, validateUsageUploadTempMeterExempionDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.Customer].[MeterExemption] table
                var customerDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempMeterExempionDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"MPXN", "MPAN/MPRN"},
                        {"DateFrom", "Date From"},
                        {"DateTo", "Date To"}
                    };
                
                var errors = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns).ToList();

                //Validate Exemption Product
                var invalidExemptionProductDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ExemptionProduct"))
                    && !_methods.IsValidExemptionProduct(r.Field<string>("ExemptionProduct")));

                foreach(var invalidExemptionProductDataRecord in invalidExemptionProductDataRecords)
                {
                    errors.Add($"Invalid Exemption Product {invalidExemptionProductDataRecord["ExemptionProduct"]} in row {invalidExemptionProductDataRecord["RowId"]}");
                }

                //Validate Exemption Proportion
                var invalidExemptionProportionDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ExemptionProportion"))
                    && !_methods.IsValidExemptionProportion(r.Field<string>("ExemptionProduct"), 
                                                            r.Field<string>("ExemptionProportion"))
                    );

                foreach(var invalidExemptionProportionDataRecord in invalidExemptionProportionDataRecords)
                {
                    errors.Add($"Invalid Exemption Proportion {invalidExemptionProportionDataRecord["ExemptionProportion"]} in row {invalidExemptionProportionDataRecord["RowId"]}");
                }

                //Update Process Queue
                var errorMessage = errors.Any() ? string.Join(';', errors) : null;
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempMeterExempionDataAPIId, errors.Any(), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempMeterExempionDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

