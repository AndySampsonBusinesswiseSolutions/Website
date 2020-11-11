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
using Microsoft.Extensions.Configuration;

namespace ValidateSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateSubMeterUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateSubMeterUsageDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private static readonly Enums.CustomerSchema.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.CustomerSchema.DataUploadValidation.SheetName();
        private static readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 validateSubMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateSubMeterUsageDataController(ILogger<ValidateSubMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateSubMeterUsageDataAPI, password);
            validateSubMeterUsageDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("ValidateSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateSubMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateSubMeterUsageDataAPI, validateSubMeterUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateSubMeterUsageDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeterUsage] table
                var subMeterUsageEntities = new Methods.Temp.CustomerDataUpload.SubMeterUsage().SubMeterUsage_GetByProcessQueueGUID(processQueueGUID);

                if(!subMeterUsageEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterUsageDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();

                var columns = new List<string>
                    {
                        _customerDataUploadValidationEntityEnums.SubMeterIdentifier,
                        _customerDataUploadValidationEntityEnums.Date,
                        _customerDataUploadValidationEntityEnums.TimePeriod,
                        _customerDataUploadValidationEntityEnums.Value,
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(subMeterUsageEntities.Select(smue => smue.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.SubMeterIdentifier, "SubMeter Identifier"},
                        {_customerDataUploadValidationEntityEnums.Date, "Read Date"}
                    };
                
                //If any are empty records, store error
                _tempCustomerDataUploadMethods.GetMissingRecords(records, subMeterUsageEntities, requiredColumns);

                //Check dates are valid
                var invalidDateEntities = subMeterUsageEntities.Where(smue => !methods.IsValidDate(smue.Date));

                foreach(var invalidDateEntity in invalidDateEntities)
                {
                    if(!records[invalidDateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Contains($"Invalid date '{invalidDateEntity.Date}' found"))
                    {
                        records[invalidDateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Add($"Invalid date '{invalidDateEntity.Date}' found");
                    }
                }

                //Check all dates are in the past
                var validDateEntities = subMeterUsageEntities.Where(smue => methods.IsValidDate(smue.Date));
                var futureDateEntities = validDateEntities.Where(smue => Convert.ToDateTime(smue.Date) >= DateTime.Today);

                foreach(var futureDateEntity in futureDateEntities)
                {
                    if(!records[futureDateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Contains($"Future date '{futureDateEntity.Date}' found"))
                    {
                        records[futureDateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Add($"Future date '{futureDateEntity.Date}' found");
                    }
                }

                //Check usage is valid
                var invalidUsageEntities = subMeterUsageEntities.Where(smue => !methods.IsValidUsage(smue.Value));

                foreach(var invalidUsageEntity in invalidUsageEntities)
                {
                    if(!records[invalidUsageEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid usage {invalidUsageEntity.Value} for {invalidUsageEntity.Date} {invalidUsageEntity.TimePeriod}"))
                    {
                        records[invalidUsageEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid usage {invalidUsageEntity.Value} for {invalidUsageEntity.Date} {invalidUsageEntity.TimePeriod}");
                    }
                }

                //If day is not October clock change, don't allow HH49 or HH50 to be populated
                var additionalHalfHourEntities = subMeterUsageEntities.Where(smue => methods.IsAdditionalTimePeriod(smue.TimePeriod)
                    && !string.IsNullOrWhiteSpace(smue.Value));
                var invalidAdditionalHalfHourEntities = additionalHalfHourEntities.Where(smue => !methods.IsOctoberClockChange(smue.Date));

                foreach(var invalidAdditionalHalfHourEntity in invalidAdditionalHalfHourEntities)
                {
                    if(!records[invalidAdditionalHalfHourEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Contains($"Usage found in additional half hour {invalidAdditionalHalfHourEntity.TimePeriod} but {invalidAdditionalHalfHourEntity.Date} is not an October clock change date"))
                    {
                        records[invalidAdditionalHalfHourEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Date].Add($"Usage found in additional half hour {invalidAdditionalHalfHourEntity.TimePeriod} but {invalidAdditionalHalfHourEntity.Date} is not an October clock change date");
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