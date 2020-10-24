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

namespace ValidateSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateSubMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<ValidateSubMeterUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 validateSubMeterUsageDataAPIId;

        public ValidateSubMeterUsageDataController(ILogger<ValidateSubMeterUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateSubMeterUsageDataAPI, _systemAPIPasswordEnums.ValidateSubMeterUsageDataAPI);
            validateSubMeterUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("ValidateSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateSubMeterUsageDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateSubMeterUsageData/Validate")]
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
                    validateSubMeterUsageDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateSubMeterUsageDataAPI, validateSubMeterUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateSubMeterUsageDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeterUsage] table
                var subMeterUsageDataRows = _tempCustomerDataUploadMethods.SubMeterUsage_GetByProcessQueueGUID(processQueueGUID);

                if(!subMeterUsageDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterUsageDataAPIId, false, null);
                    return;
                }

                var columns = new List<string>
                    {
                        _customerDataUploadValidationEntityEnums.SubMeterIdentifier,
                        _customerDataUploadValidationEntityEnums.Date,
                        _customerDataUploadValidationEntityEnums.TimePeriod,
                        _customerDataUploadValidationEntityEnums.Value,
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(subMeterUsageDataRows.Select(d => Convert.ToInt32(d["RowId"].ToString())).ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.SubMeterIdentifier, "SubMeter Identifier"},
                        {_customerDataUploadValidationEntityEnums.Date, "Read Date"}
                    };
                
                //If any are empty records, store error
                _tempCustomerDataUploadMethods.GetMissingRecords(records, subMeterUsageDataRows, requiredColumns);

                //Check dates are valid
                var invalidDateDataRows = subMeterUsageDataRows.Where(r => !_methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.Date)));

                foreach(var invalidDateDataRow in invalidDateDataRows)
                {
                    var rowId = Convert.ToInt32(invalidDateDataRow["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.Date].Contains($"Invalid date {invalidDateDataRow[_customerDataUploadValidationEntityEnums.Date]} found"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.Date].Add($"Invalid date {invalidDateDataRow[_customerDataUploadValidationEntityEnums.Date]} found");
                    }
                }

                //Check all dates are in the past
                var validDateDataRows = subMeterUsageDataRows.Where(r => _methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.Date)));
                var futureDateDataRows = validDateDataRows.Where(r => Convert.ToDateTime(r.Field<string>(_customerDataUploadValidationEntityEnums.Date)) >= DateTime.Today);

                foreach(var futureDateDataRow in futureDateDataRows)
                {
                    var rowId = Convert.ToInt32(futureDateDataRow["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.Date].Contains($"Future date {futureDateDataRow[_customerDataUploadValidationEntityEnums.Date]} found"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.Date].Add($"Future date {futureDateDataRow[_customerDataUploadValidationEntityEnums.Date]} found");
                    }
                }

                //Check usage is valid
                var invalidUsageDataRows = subMeterUsageDataRows.Where(r => !_methods.IsValidUsage(r.Field<string>(_customerDataUploadValidationEntityEnums.Value)));

                foreach(var invalidUsageDataRow in invalidUsageDataRows)
                {
                    var rowId = Convert.ToInt32(invalidUsageDataRow["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid usage {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.Value]} for {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.Date]} {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.TimePeriod]}"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid usage {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.Value]} for {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.Date]} {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.TimePeriod]}");
                    }
                }

                //If day is not October clock change, don't allow HH49 or HH50 to be populated
                var additionalHalfHourDataRows = subMeterUsageDataRows.Where(r => _methods.IsAdditionalTimePeriod(r.Field<string>(_customerDataUploadValidationEntityEnums.TimePeriod))
                    && !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.Value)));
                var invalidAdditionalHalfHourDataRows = additionalHalfHourDataRows.Where(r => !_methods.IsOctoberClockChange(r.Field<string>(_customerDataUploadValidationEntityEnums.Date)));

                foreach(var invalidAdditionalHalfHourDataRow in invalidAdditionalHalfHourDataRows)
                {
                    var rowId = Convert.ToInt32(invalidAdditionalHalfHourDataRow["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.Date].Contains($"Usage found in additional half hour {invalidAdditionalHalfHourDataRow[_customerDataUploadValidationEntityEnums.TimePeriod]} but {invalidAdditionalHalfHourDataRow[_customerDataUploadValidationEntityEnums.Date]} is not an October clock change date"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.Date].Add($"Usage found in additional half hour {invalidAdditionalHalfHourDataRow[_customerDataUploadValidationEntityEnums.TimePeriod]} but {invalidAdditionalHalfHourDataRow[_customerDataUploadValidationEntityEnums.Date]} is not an October clock change date");
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.SubMeterUsage);
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterUsageDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}