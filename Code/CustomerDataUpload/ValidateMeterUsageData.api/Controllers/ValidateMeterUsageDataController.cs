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

namespace ValidateMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<ValidateMeterUsageDataController> _logger;
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
        private readonly Int64 validateMeterUsageDataAPIId;

        public ValidateMeterUsageDataController(ILogger<ValidateMeterUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateMeterUsageDataAPI, _systemAPIPasswordEnums.ValidateMeterUsageDataAPI);
            validateMeterUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("ValidateMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateMeterUsageDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateMeterUsageData/Validate")]
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
                    validateMeterUsageDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateMeterUsageDataAPI, validateMeterUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateMeterUsageDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[MeterUsage] table
                var meterUsageDataRows = _tempCustomerDataUploadMethods.MeterUsage_GetByProcessQueueGUID(processQueueGUID);

                if(!meterUsageDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterUsageDataAPIId, false, null);
                    return;
                }

                string errorMessage = null;
                var sourceSheetList = new List<string>
                {
                    _customerDataUploadValidationSheetNameEnums.MeterUsage,
                    _customerDataUploadValidationSheetNameEnums.DailyMeterUsage
                };

                foreach(var sourceSheet in sourceSheetList)
                {
                    var usageDataRows = meterUsageDataRows.Where(r => r.Field<string>("SheetName") == sourceSheet);

                    var columns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.Date, "Read Date"},
                        {_customerDataUploadValidationEntityEnums.TimePeriod, "Time Period"},
                        {_customerDataUploadValidationEntityEnums.Value, "Volume"},
                    };

                    var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(meterUsageDataRows, columns);

                    //If any are empty records, store error
                    var requiredColumns = new Dictionary<string, string>
                        {
                            {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                            {_customerDataUploadValidationEntityEnums.Date, "Read Date"}
                        };
                    _tempCustomerDataUploadMethods.GetMissingRecords(records, meterUsageDataRows, requiredColumns);

                    //Check dates are valid
                    var invalidDateDataRows = meterUsageDataRows.Where(r => !_methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.Date)));

                    foreach(var invalidDateDataRow in invalidDateDataRows)
                    {
                        var rowId = Convert.ToInt32(invalidDateDataRow["RowId"]);
                        if(!records[rowId][_customerDataUploadValidationEntityEnums.Date].Contains($"Invalid date {invalidDateDataRow[_customerDataUploadValidationEntityEnums.Date]} found"))
                        {
                            records[rowId][_customerDataUploadValidationEntityEnums.Date].Add($"Invalid date {invalidDateDataRow[_customerDataUploadValidationEntityEnums.Date]} found");
                        }
                    }

                    //Check all dates are in the past
                    var validDateDataRows = meterUsageDataRows.Where(r => _methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.Date)));
                    var futureDateDataRows = validDateDataRows.Where(r => Convert.ToDateTime(r.Field<string>(_customerDataUploadValidationEntityEnums.Date)) >= DateTime.Today);

                    foreach(var futureDateDataRow in futureDateDataRows)
                    {
                        var rowId = Convert.ToInt32(futureDateDataRow["RowId"]);
                        if(!records[rowId][_customerDataUploadValidationEntityEnums.Date].Contains($"Future date {futureDateDataRow[_customerDataUploadValidationEntityEnums.Date]} found"))
                        {
                            records[rowId][_customerDataUploadValidationEntityEnums.Date].Add($"Future date {futureDateDataRow[_customerDataUploadValidationEntityEnums.Date]} found");
                        }
                    }

                    //Check usage is valid (if day is not October clock change, don't allow HH49 or HH50 to be populated)
                    var invalidUsageDataRows = meterUsageDataRows.Where(r => !_methods.IsValidUsage(r.Field<string>(_customerDataUploadValidationEntityEnums.Value)));

                    foreach(var invalidUsageDataRow in invalidUsageDataRows)
                    {
                        var rowId = Convert.ToInt32(invalidUsageDataRow["RowId"]);
                        if(!records[rowId][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid usage {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.Value]} for {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.Date]} {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.TimePeriod]}"))
                        {
                            records[rowId][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid usage {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.Value]} for {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.Date]} {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.TimePeriod]}");
                        }
                    }

                    var additionalHalfHourDataRows = meterUsageDataRows.Where(r => _methods.IsAdditionalTimePeriod(r.Field<string>(_customerDataUploadValidationEntityEnums.TimePeriod))
                        && !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.Value)));
                    var invalidAdditionalHalfHourDataRows = additionalHalfHourDataRows.Where(r => !_methods.IsOctoberClockChange(r.Field<string>(_customerDataUploadValidationEntityEnums.Date)));

                    foreach(var invalidUsageDataRow in invalidAdditionalHalfHourDataRows)
                    {
                        var rowId = Convert.ToInt32(invalidUsageDataRow["RowId"]);
                        if(!records[rowId][_customerDataUploadValidationEntityEnums.Date].Contains($"Usage found in additional half hour {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.TimePeriod]} but {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.Date]} is not an October clock change date"))
                        {
                            records[rowId][_customerDataUploadValidationEntityEnums.Date].Add($"Usage found in additional half hour {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.TimePeriod]} but {invalidUsageDataRow[_customerDataUploadValidationEntityEnums.Date]} is not an October clock change date");
                        }
                    }

                    //Update Process Queue
                    var sourceErrorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, sourceSheet);
                    if(!string.IsNullOrWhiteSpace(sourceErrorMessage))
                    {
                        errorMessage = sourceErrorMessage;
                    }
                }
                
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterUsageDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}