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

namespace ValidateSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateSubMeterDataController> _logger;
        private readonly Int64 validateSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateSubMeterDataController(ILogger<ValidateSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateSubMeterDataAPI, password);
            validateSubMeterDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateSubMeterDataAPI);
        }

        [HttpPost]
        [Route("ValidateSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(validateSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateSubMeterData/Validate")]
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
                    validateSubMeterDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateSubMeterDataAPI, validateSubMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateSubMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] table
                var subMeterEntities = new Methods.Temp.CustomerDataUpload.SubMeter().SubMeter_GetByProcessQueueGUID(processQueueGUID);

                if(!subMeterEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterDataAPIId, false, null);
                    return;
                }

                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
                var customerMethods = new Methods.Customer();

                var columns = new List<string>
                    {
                        customerDataUploadValidationEntityEnums.MPXN,
                        customerDataUploadValidationEntityEnums.SubMeterIdentifier,
                        customerDataUploadValidationEntityEnums.SerialNumber,
                        customerDataUploadValidationEntityEnums.SubArea,
                        customerDataUploadValidationEntityEnums.Asset
                    };

                var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(subMeterEntities.Select(sme => sme.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.SubMeterIdentifier, "SubMeter Name"}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, subMeterEntities, requiredColumns);

                //Get submeters not stored in database
                var subMeterIdentifierSubMeterAttributeId = customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(new Enums.CustomerSchema.SubMeter.Attribute().SubMeterIdentifier);
                var newSubMeterDataRecords = subMeterEntities.Where(sme => 
                    customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, sme.SubMeterIdentifier) == 0)
                    .ToList();

                //MPXN, SerialNumber, SubArea and Asset must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {customerDataUploadValidationEntityEnums.SerialNumber, "SubMeter Serial Number"},
                        {customerDataUploadValidationEntityEnums.SubArea, customerDataUploadValidationEntityEnums.SubArea},
                        {customerDataUploadValidationEntityEnums.Asset, customerDataUploadValidationEntityEnums.Asset}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, newSubMeterDataRecords, requiredColumns);

                //Update Process Queue
                var errorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, new Enums.CustomerSchema.DataUploadValidation.SheetName().SubMeter);
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}