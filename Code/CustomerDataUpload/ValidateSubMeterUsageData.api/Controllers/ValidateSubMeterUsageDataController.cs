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
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.DataUploadValidation.SheetName _dataUploadValidationSheetNameEnums = new Enums.DataUploadValidation.SheetName();
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
                    validateSubMeterUsageDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateSubMeterUsageDataAPI, validateSubMeterUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[SubMeterUsage] table
                var customerDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateSubMeterUsageDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"SubMeterName", "SubMeter Identifier"},
                        {"Date", "Read Date"}
                    };
                
                //Creates dictionary string, int, list<string>
                //and populates any required columns that are missing
                var records = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns);

                //Check all dates are in the past
                var futureDateDataRows = customerDataRows.Where(r => _methods.IsValidDate(r.Field<string>("Date")) 
                    && r.Field<DateTime>("Date") >= DateTime.Today);

                foreach(var futureDateDataRow in futureDateDataRows)
                {
                    var rowId = Convert.ToInt32(futureDateDataRow["RowId"]);
                    records[rowId]["Date"].Add($"Future date {futureDateDataRow["Date"]} found");
                }

                //Check usage is valid
                var invalidUsageDataRows = customerDataRows.Where(r => !_methods.IsValidUsage(r.Field<string>("Value")));

                foreach(var invalidUsageDataRow in invalidUsageDataRows)
                {
                    var rowId = Convert.ToInt32(invalidUsageDataRow["RowId"]);
                    records[rowId]["Value"].Add($"Invalid usage {invalidUsageDataRow["Value"]} for {invalidUsageDataRow["Date"]} {invalidUsageDataRow["TimePeriod"]}");
                }

                //If day is not October clock change, don't allow HH49 or HH50 to be populated
                var additionalHalfHourDataRows = customerDataRows.Where(r => _methods.IsAdditionalTimePeriod(r.Field<string>("TimePeriod")));
                var invalidAdditionalHalfHourDataRows = additionalHalfHourDataRows.Where(r => !_methods.IsOctoberClockChange(r.Field<string>("Date")));

                foreach(var invalidAdditionalHalfHourDataRow in invalidAdditionalHalfHourDataRows)
                {
                    var rowId = Convert.ToInt32(invalidAdditionalHalfHourDataRow["RowId"]);
                    records[rowId]["Date"].Add($"Usage found in additional half hour {invalidAdditionalHalfHourDataRow["TimePeriod"]} but {invalidAdditionalHalfHourDataRow["Date"]} is not an October clock change date");
                }

                //Update Process Queue
                var errorMessage = _tempCustomerMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _dataUploadValidationSheetNameEnums.SubMeterUsage);
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateSubMeterUsageDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}