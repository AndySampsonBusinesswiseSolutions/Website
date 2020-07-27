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

namespace ValidateUsageUploadTempSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateUsageUploadTempSubMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<ValidateUsageUploadTempSubMeterUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 validateUsageUploadTempSubMeterUsageDataAPIId;

        public ValidateUsageUploadTempSubMeterUsageDataController(ILogger<ValidateUsageUploadTempSubMeterUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateUsageUploadTempSubMeterUsageDataAPI, _systemAPIPasswordEnums.ValidateUsageUploadTempSubMeterUsageDataAPI);
            validateUsageUploadTempSubMeterUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateUsageUploadTempSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateUsageUploadTempSubMeterUsageDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempSubMeterUsageData/Validate")]
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
                    validateUsageUploadTempSubMeterUsageDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateUsageUploadTempSubMeterUsageDataAPI, validateUsageUploadTempSubMeterUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.Customer].[SubMeterUsage] table
                var customerDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempSubMeterUsageDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"SubMeterName", "SubMeter Identifier"},
                        {"Date", "Read Date"}
                    };
                
                var errors = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns).ToList();

                //Check all dates are in the past
                var futureDateDataRows = customerDataRows.Where(r => _methods.IsValidDate(r.Field<string>("Date")) 
                    && r.Field<DateTime>("Date") >= DateTime.Today);

                foreach(var futureDateDataRow in futureDateDataRows)
                {
                    errors.Add($"Future date {futureDateDataRow["Date"]} in row {futureDateDataRow["RowId"]}");
                }

                //Check usage is valid (if day is not October clock change, don't allow HH49 or HH50 to be populated)
                var invalidUsageDataRows = customerDataRows.Where(r => !_methods.IsValidUsage(r.Field<string>("Value")));

                foreach(var invalidUsageDataRow in invalidUsageDataRows)
                {
                    errors.Add($"Invalid usage {invalidUsageDataRow["Value"]} in row {invalidUsageDataRow["RowId"]} for {invalidUsageDataRow["Date"]} {invalidUsageDataRow["TimePeriod"]}");
                }

                var additionalHalfHourDataRows = customerDataRows.Where(r => _methods.IsAdditionalTimePeriod(r.Field<string>("TimePeriod")));
                var invalidAdditionalHalfHourDataRows = additionalHalfHourDataRows.Where(r => !_methods.IsOctoberClockChange(r.Field<string>("Date")));

                foreach(var invalidUsageDataRow in invalidAdditionalHalfHourDataRows)
                {
                    errors.Add($"Usage found in additional half hour {invalidUsageDataRow["TimePeriod"]} but {invalidUsageDataRow["Date"]} is not an October clock change date");
                }

                //Update Process Queue
                var errorMessage = errors.Any() ? string.Join(';', errors) : null;
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempSubMeterUsageDataAPIId, errors.Any(), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

