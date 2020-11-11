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
using Microsoft.Extensions.Configuration;

namespace ValidateMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateMeterUsageDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private static readonly Enums.CustomerSchema.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.CustomerSchema.DataUploadValidation.SheetName();
        private static readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 validateMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateMeterUsageDataController(ILogger<ValidateMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateMeterUsageDataAPI, password);
            validateMeterUsageDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("ValidateMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateMeterUsageDataAPI, validateMeterUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateMeterUsageDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[MeterUsage] table
                var meterUsageEntities = new Methods.Temp.CustomerDataUpload.MeterUsage().MeterUsage_GetMeterUsageEntityListByProcessQueueGUID(processQueueGUID);

                if(!meterUsageEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateMeterUsageDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();

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
                    var usageEntities = meterUsageEntities.Where(mu => mu.SheetName == sourceSheet).ToList();

                    if(!usageEntities.Any())
                    {
                        continue;
                    }

                    var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(usageEntities.Select(u => u.RowId.Value).Distinct().ToList(), columns);

                    //If any are empty records, store error
                    _tempCustomerDataUploadMethods.GetMissingRecords<Temp.CustomerDataUpload.MeterUsage>(records, usageEntities, requiredColumns);

                    //Check dates are valid
                    var invalidDates = usageEntities.Where(u => !methods.IsValidDate(u.Date)).Select(u => new {u.RowId, u.Date}).Distinct().ToList();

                    foreach(var invalidDate in invalidDates)
                    {
                        if(!records[invalidDate.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Contains($"Invalid date '{invalidDate.Date}' found"))
                        {
                            records[invalidDate.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Add($"Invalid date '{invalidDate.Date}' found");
                        }
                    }

                    //Check all dates are in the past
                    var validDates = usageEntities.Where(u => methods.IsValidDate(u.Date)).ToList();
                    var futureDates = validDates.Where(u => Convert.ToDateTime(u.Date) >= DateTime.Today).ToList();

                    foreach(var futureDate in futureDates)
                    {
                        if(!records[futureDate.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Contains($"Future date '{futureDate.Date}' found"))
                        {
                            records[futureDate.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Add($"Future date '{futureDate.Date}' found");
                        }
                    }

                    //Check usage is valid (if day is not October clock change, don't allow HH49 or HH50 to be populated)
                    var invalidUsages = usageEntities.Where(u => !methods.IsValidUsage(u.Value)).ToList();

                    foreach(var invalidUsage in invalidUsages)
                    {
                        if(!records[invalidUsage.RowId.Value][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid usage '{invalidUsage.Value}' for {invalidUsage.Date} {invalidUsage.TimePeriod}"))
                        {
                            records[invalidUsage.RowId.Value][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid usage '{invalidUsage.Value}' for {invalidUsage.Date} {invalidUsage.TimePeriod}");
                        }
                    }

                    var additionalHalfHours = usageEntities.Where(u => methods.IsAdditionalTimePeriod(u.TimePeriod) && !string.IsNullOrWhiteSpace(u.Value)).ToList();
                    var invalidAdditionalHalfHours = additionalHalfHours.Where(u => !methods.IsOctoberClockChange(u.Date)).ToList();

                    foreach(var invalidUsage in invalidAdditionalHalfHours)
                    {
                        if(!records[invalidUsage.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Contains($"Usage found in additional half hour '{invalidUsage.TimePeriod}' but {invalidUsage.Date} is not an October clock change date"))
                        {
                            records[invalidUsage.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Add($"Usage found in additional half hour '{invalidUsage.TimePeriod}' but {invalidUsage.Date} is not an October clock change date");
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