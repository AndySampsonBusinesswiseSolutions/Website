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

namespace ValidateFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFlexReferenceVolumeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateFlexReferenceVolumeDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private static readonly Enums.CustomerSchema.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.CustomerSchema.DataUploadValidation.SheetName();
        private static readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 validateFlexReferenceVolumeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateFlexReferenceVolumeDataController(ILogger<ValidateFlexReferenceVolumeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateFlexReferenceVolumeDataAPI, password);
            validateFlexReferenceVolumeDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateFlexReferenceVolumeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFlexReferenceVolumeData/Validate")]
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
                    validateFlexReferenceVolumeDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateFlexReferenceVolumeDataAPI, validateFlexReferenceVolumeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateFlexReferenceVolumeDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexReferenceVolume] table
                var flexReferenceVolumeEntities = new Methods.Temp.CustomerDataUpload.FlexReferenceVolume().FlexReferenceVolume_GetByProcessQueueGUID(processQueueGUID);

                if(!flexReferenceVolumeEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexReferenceVolumeDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();

                var columns = new List<string>
                    {
                        _customerDataUploadValidationEntityEnums.ContractReference,
                        _customerDataUploadValidationEntityEnums.DateFrom,
                        _customerDataUploadValidationEntityEnums.DateTo,
                        _customerDataUploadValidationEntityEnums.Volume,
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(flexReferenceVolumeEntities.Select(frve => frve.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.ContractReference, "Contract Reference"}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, flexReferenceVolumeEntities, requiredColumns);

                //Validate Contract Dates
                var invalidDateFromDataRecords = flexReferenceVolumeEntities.Where(frve => !string.IsNullOrWhiteSpace(frve.DateFrom)
                    && !methods.IsValidDate(frve.DateFrom));

                foreach(var invalidDateFromDataRecord in invalidDateFromDataRecords)
                {
                    if(!records[invalidDateFromDataRecord.RowId.Value][_customerDataUploadValidationEntityEnums.DateFrom].Contains($"Invalid Date From '{invalidDateFromDataRecord.DateFrom}'"))
                    {
                        records[invalidDateFromDataRecord.RowId.Value][_customerDataUploadValidationEntityEnums.DateFrom].Add($"Invalid Date From '{invalidDateFromDataRecord.DateFrom}'");
                    }
                }

                var invalidDateToDataRecords = flexReferenceVolumeEntities.Where(frve => !string.IsNullOrWhiteSpace(frve.DateTo)
                    && !methods.IsValidDate(frve.DateTo));

                foreach(var invalidDateToDataRecord in invalidDateToDataRecords)
                {
                    if(!records[invalidDateToDataRecord.RowId.Value][_customerDataUploadValidationEntityEnums.DateTo].Contains($"Invalid Date to '{invalidDateToDataRecord.DateTo}'"))
                    {
                        records[invalidDateToDataRecord.RowId.Value][_customerDataUploadValidationEntityEnums.DateTo].Add($"Invalid Date to '{invalidDateToDataRecord.DateTo}'");
                    }
                }

                var invalidContractDateDataRecords = flexReferenceVolumeEntities.Where(frve => !string.IsNullOrWhiteSpace(frve.DateFrom)
                    && !string.IsNullOrWhiteSpace(frve.DateTo)
                    && methods.IsValidDate(frve.DateFrom)
                    && methods.IsValidDate(frve.DateTo)
                    && Convert.ToDateTime(frve.DateFrom) >= Convert.ToDateTime(frve.DateTo));

                foreach(var invalidDateToDataRecord in invalidDateToDataRecords)
                {
                    if(!records[invalidDateToDataRecord.RowId.Value][_customerDataUploadValidationEntityEnums.DateFrom].Contains($"Invalid Contract Dates '{invalidDateToDataRecord.DateFrom}' is equal to or later than '{invalidDateToDataRecord.DateTo}'"))
                    {
                        records[invalidDateToDataRecord.RowId.Value][_customerDataUploadValidationEntityEnums.DateFrom].Add($"Invalid Contract Dates '{invalidDateToDataRecord.DateFrom}' is equal to or later than '{invalidDateToDataRecord.DateTo}'");
                    }
                }

                //Validate Volume
                var invalidVolumeDataRecords = flexReferenceVolumeEntities.Where(frve => !string.IsNullOrWhiteSpace(frve.Volume)
                    && !methods.IsValidFlexReferenceVolume(frve.Volume));

                foreach(var invalidVolumeDataRecord in invalidVolumeDataRecords)
                {
                    if(!records[invalidVolumeDataRecord.RowId.Value][_customerDataUploadValidationEntityEnums.Volume].Contains($"Invalid Reference Volume '{invalidVolumeDataRecord.Volume}'"))
                    {
                        records[invalidVolumeDataRecord.RowId.Value][_customerDataUploadValidationEntityEnums.Volume].Add($"Invalid Reference Volume '{invalidVolumeDataRecord.Volume}'");
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.FlexReferenceVolume);
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexReferenceVolumeDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}