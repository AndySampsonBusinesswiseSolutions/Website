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

namespace ValidateSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateSubMeterDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private static readonly Enums.Customer.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
        private readonly Int64 validateSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateSubMeterDataController(ILogger<ValidateSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().ValidateSubMeterDataAPI, password);
            validateSubMeterDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateSubMeterDataAPI);
        }

        [HttpPost]
        [Route("ValidateSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateSubMeterData/Validate")]
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
                    validateSubMeterDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateSubMeterDataAPI, validateSubMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateSubMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] table
                var subMeterEntities = new Methods.Temp.CustomerDataUpload.SubMeter().SubMeter_GetByProcessQueueGUID(processQueueGUID);

                if(!subMeterEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterDataAPIId, false, null);
                    return;
                }

                var columns = new List<string>
                    {
                        _customerDataUploadValidationEntityEnums.MPXN,
                        _customerDataUploadValidationEntityEnums.SubMeterIdentifier,
                        _customerDataUploadValidationEntityEnums.SerialNumber,
                        _customerDataUploadValidationEntityEnums.SubArea,
                        _customerDataUploadValidationEntityEnums.Asset
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(subMeterEntities.Select(sme => sme.RowId).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.SubMeterIdentifier, "SubMeter Name"}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, subMeterEntities, requiredColumns);

                //Get submeters not stored in database
                var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);
                var newSubMeterDataRecords = subMeterEntities.Where(sme => 
                    _customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, sme.SubMeterIdentifier) == 0)
                    .ToList();

                //MPXN, SerialNumber, SubArea and Asset must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.SerialNumber, "SubMeter Serial Number"},
                        {_customerDataUploadValidationEntityEnums.SubArea, _customerDataUploadValidationEntityEnums.SubArea},
                        {_customerDataUploadValidationEntityEnums.Asset, _customerDataUploadValidationEntityEnums.Asset}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, newSubMeterDataRecords, requiredColumns);

                //Update Process Queue
                var errorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.SubMeter);
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}