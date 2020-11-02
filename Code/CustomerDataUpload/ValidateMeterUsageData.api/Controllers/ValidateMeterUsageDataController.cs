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
using Entity;

namespace ValidateMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<ValidateMeterUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
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
                    validateMeterUsageDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateMeterUsageDataAPI, validateMeterUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateMeterUsageDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[MeterUsage] table
                var meterUsageEntities = _tempCustomerDataUploadMethods.MeterUsage_GetMeterUsageEntityListByProcessQueueGUID(processQueueGUID);

                if(!meterUsageEntities.Any())
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

                var columns = new List<string>
                {
                    _customerDataUploadValidationEntityEnums.MPXN,
                    _customerDataUploadValidationEntityEnums.Date,
                    _customerDataUploadValidationEntityEnums.TimePeriod,
                    _customerDataUploadValidationEntityEnums.Value,
                };
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.Date, "Read Date"}
                    };

                foreach(var sourceSheet in sourceSheetList)
                {
                    var usages = meterUsageEntities.Where(mu => mu.SheetName == sourceSheet).ToList();

                    if(!usages.Any())
                    {
                        continue;
                    }

                    var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(usages.Select(u => u.RowId).Distinct().ToList(), columns);

                    //If any are empty records, store error
                    _tempCustomerDataUploadMethods.GetMissingRecords<Temp.CustomerDataUpload.MeterUsage>(records, usages, requiredColumns);

                    //Check dates are valid
                    var invalidDates = usages.Where(u => !_methods.IsValidDate(u.Date)).Select(u => new {u.RowId, u.Date}).Distinct().ToList();

                    foreach(var invalidDate in invalidDates)
                    {
                        var rowId = Convert.ToInt32(invalidDate.RowId);
                        if(!records[rowId][_customerDataUploadValidationEntityEnums.Date].Contains($"Invalid date {invalidDate.Date} found"))
                        {
                            records[rowId][_customerDataUploadValidationEntityEnums.Date].Add($"Invalid date {invalidDate.Date} found");
                        }
                    }

                    //Check all dates are in the past
                    var validDates = usages.Where(u => _methods.IsValidDate(u.Date)).ToList();
                    var futureDates = validDates.Where(u => Convert.ToDateTime(u.Date) >= DateTime.Today).ToList();

                    foreach(var futureDate in futureDates)
                    {
                        var rowId = Convert.ToInt32(futureDate.RowId);
                        if(!records[rowId][_customerDataUploadValidationEntityEnums.Date].Contains($"Future date {futureDate.Date} found"))
                        {
                            records[rowId][_customerDataUploadValidationEntityEnums.Date].Add($"Future date {futureDate.Date} found");
                        }
                    }

                    //Check usage is valid (if day is not October clock change, don't allow HH49 or HH50 to be populated)
                    var invalidUsages = usages.Where(u => !_methods.IsValidUsage(u.Value)).ToList();

                    foreach(var invalidUsage in invalidUsages)
                    {
                        var rowId = Convert.ToInt32(invalidUsage.RowId);
                        if(!records[rowId][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid usage {invalidUsage.Value} for {invalidUsage.Date} {invalidUsage.TimePeriod}"))
                        {
                            records[rowId][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid usage {invalidUsage.Value} for {invalidUsage.Date} {invalidUsage.TimePeriod}");
                        }
                    }

                    var additionalHalfHours = usages.Where(u => _methods.IsAdditionalTimePeriod(u.TimePeriod) && !string.IsNullOrWhiteSpace(u.Value)).ToList();
                    var invalidAdditionalHalfHours = additionalHalfHours.Where(u => !_methods.IsOctoberClockChange(u.Date)).ToList();

                    foreach(var invalidUsage in invalidAdditionalHalfHours)
                    {
                        var rowId = Convert.ToInt32(invalidUsage.RowId);
                        if(!records[rowId][_customerDataUploadValidationEntityEnums.Date].Contains($"Usage found in additional half hour {invalidUsage.TimePeriod} but {invalidUsage.Date} is not an October clock change date"))
                        {
                            records[rowId][_customerDataUploadValidationEntityEnums.Date].Add($"Usage found in additional half hour {invalidUsage.TimePeriod} but {invalidUsage.Date} is not an October clock change date");
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