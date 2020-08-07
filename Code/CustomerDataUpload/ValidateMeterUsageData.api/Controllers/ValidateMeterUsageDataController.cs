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
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
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

                //Get data from [Temp.CustomerDataUpload].[MeterUsage] table
                var meterUsageDataRows = _tempCustomerMethods.MeterUsage_GetByProcessQueueGUID(processQueueGUID);

                if(!meterUsageDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateMeterUsageDataAPIId, false, null);
                    return;
                }

                var columns = new Dictionary<string, string>
                    {
                        {"MPXN", "MPAN/MPRN"},
                        {"Date", "Read Date"},
                        {"TimePeriod", "Time Period"},
                        {"Value", "Volume"},
                    };

                var records = _tempCustomerMethods.InitialiseRecordsDictionary(meterUsageDataRows, columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"MPXN", "MPAN/MPRN"},
                        {"Date", "Read Date"}
                    };
                _tempCustomerMethods.GetMissingRecords(records, meterUsageDataRows, requiredColumns);

                //Check all dates are in the past
                var futureDateDataRows = meterUsageDataRows.Where(r => _methods.IsValidDate(r.Field<string>("Date")) 
                    && r.Field<DateTime>("Date") >= DateTime.Today);

                foreach(var futureDateDataRow in futureDateDataRows)
                {
                    var rowId = Convert.ToInt32(futureDateDataRow["RowId"]);
                    records[rowId]["Date"].Add($"Future date {futureDateDataRow["Date"]} found");
                }

                //Check usage is valid (if day is not October clock change, don't allow HH49 or HH50 to be populated)
                var invalidUsageDataRows = meterUsageDataRows.Where(r => !_methods.IsValidUsage(r.Field<string>("Value")));

                foreach(var invalidUsageDataRow in invalidUsageDataRows)
                {
                    var rowId = Convert.ToInt32(invalidUsageDataRow["RowId"]);
                    records[rowId]["Value"].Add($"Invalid usage {invalidUsageDataRow["Value"]} for {invalidUsageDataRow["Date"]} {invalidUsageDataRow["TimePeriod"]}");
                }

                var additionalHalfHourDataRows = meterUsageDataRows.Where(r => _methods.IsAdditionalTimePeriod(r.Field<string>("TimePeriod")));
                var invalidAdditionalHalfHourDataRows = additionalHalfHourDataRows.Where(r => !_methods.IsOctoberClockChange(r.Field<string>("Date")));

                foreach(var invalidUsageDataRow in invalidAdditionalHalfHourDataRows)
                {
                    var rowId = Convert.ToInt32(invalidUsageDataRow["RowId"]);
                    records[rowId]["Date"].Add($"Usage found in additional half hour {invalidUsageDataRow["TimePeriod"]} but {invalidUsageDataRow["Date"]} is not an October clock change date");
                }

                //Update Process Queue
                var errorMessage = _tempCustomerMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.MeterUsage);
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateMeterUsageDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

