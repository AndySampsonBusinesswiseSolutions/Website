using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

namespace ValidateSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateSubMeterUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateSubMeterUsageDataController> _logger;
        private readonly Int64 validateSubMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateSubMeterUsageDataController(ILogger<ValidateSubMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateSubMeterUsageDataAPI, password);
            validateSubMeterUsageDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("ValidateSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(validateSubMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateSubMeterUsageData/Validate")]
        public void Validate([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    validateSubMeterUsageDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateSubMeterUsageDataAPI, validateSubMeterUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateSubMeterUsageDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeterUsage] table
                var subMeterUsageEntities = new Methods.Temp.CustomerDataUpload.SubMeterUsage().SubMeterUsage_GetByProcessQueueGUID(processQueueGUID);

                if(!subMeterUsageEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterUsageDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();

                var columns = new List<string>
                    {
                        customerDataUploadValidationEntityEnums.SubMeterIdentifier,
                        customerDataUploadValidationEntityEnums.Date,
                        customerDataUploadValidationEntityEnums.TimePeriod,
                        customerDataUploadValidationEntityEnums.Value,
                    };

                var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(subMeterUsageEntities.Select(smue => smue.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.SubMeterIdentifier, "SubMeter Identifier"},
                        {customerDataUploadValidationEntityEnums.Date, "Read Date"}
                    };
                
                //If any are empty records, store error
                tempCustomerDataUploadMethods.GetMissingRecords(records, subMeterUsageEntities, requiredColumns);

                //Check dates are valid
                var invalidDateEntities = subMeterUsageEntities.Where(smue => !methods.IsValidDate(smue.Date));

                foreach(var invalidDateEntity in invalidDateEntities)
                {
                    if(!records[invalidDateEntity.RowId.Value][customerDataUploadValidationEntityEnums.Date].Contains($"Invalid date '{invalidDateEntity.Date}' found"))
                    {
                        records[invalidDateEntity.RowId.Value][customerDataUploadValidationEntityEnums.Date].Add($"Invalid date '{invalidDateEntity.Date}' found");
                    }
                }

                //Check all dates are in the past
                var validDateEntities = subMeterUsageEntities.Where(smue => methods.IsValidDate(smue.Date));
                var futureDateEntities = validDateEntities.Where(smue => Convert.ToDateTime(smue.Date) >= DateTime.Today);

                foreach(var futureDateEntity in futureDateEntities)
                {
                    if(!records[futureDateEntity.RowId.Value][customerDataUploadValidationEntityEnums.Date].Contains($"Future date '{futureDateEntity.Date}' found"))
                    {
                        records[futureDateEntity.RowId.Value][customerDataUploadValidationEntityEnums.Date].Add($"Future date '{futureDateEntity.Date}' found");
                    }
                }

                //Check usage is valid
                var invalidUsageEntities = subMeterUsageEntities.Where(smue => !methods.IsValidUsage(smue.Value));

                foreach(var invalidUsageEntity in invalidUsageEntities)
                {
                    if(!records[invalidUsageEntity.RowId.Value][customerDataUploadValidationEntityEnums.Value].Contains($"Invalid usage {invalidUsageEntity.Value} for {invalidUsageEntity.Date} {invalidUsageEntity.TimePeriod}"))
                    {
                        records[invalidUsageEntity.RowId.Value][customerDataUploadValidationEntityEnums.Value].Add($"Invalid usage {invalidUsageEntity.Value} for {invalidUsageEntity.Date} {invalidUsageEntity.TimePeriod}");
                    }
                }

                //If day is not October clock change, don't allow HH49 or HH50 to be populated
                var additionalHalfHourEntities = subMeterUsageEntities.Where(smue => methods.IsAdditionalTimePeriod(smue.TimePeriod)
                    && !string.IsNullOrWhiteSpace(smue.Value));
                var invalidAdditionalHalfHourEntities = additionalHalfHourEntities.Where(smue => !methods.IsOctoberClockChange(smue.Date));

                foreach(var invalidAdditionalHalfHourEntity in invalidAdditionalHalfHourEntities)
                {
                    if(!records[invalidAdditionalHalfHourEntity.RowId.Value][customerDataUploadValidationEntityEnums.Date].Contains($"Usage found in additional half hour {invalidAdditionalHalfHourEntity.TimePeriod} but {invalidAdditionalHalfHourEntity.Date} is not an October clock change date"))
                    {
                        records[invalidAdditionalHalfHourEntity.RowId.Value][customerDataUploadValidationEntityEnums.Date].Add($"Usage found in additional half hour {invalidAdditionalHalfHourEntity.TimePeriod} but {invalidAdditionalHalfHourEntity.Date} is not an October clock change date");
                    }
                }

                //Update Process Queue
                var errorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, new Enums.CustomerSchema.DataUploadValidation.SheetName().SubMeterUsage);
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterUsageDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}