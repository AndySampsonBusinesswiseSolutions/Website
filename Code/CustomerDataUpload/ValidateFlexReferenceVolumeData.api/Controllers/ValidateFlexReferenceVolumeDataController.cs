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

namespace ValidateFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFlexReferenceVolumeDataController : ControllerBase
    {
        private readonly ILogger<ValidateFlexReferenceVolumeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 validateFlexReferenceVolumeDataAPIId;

        public ValidateFlexReferenceVolumeDataController(ILogger<ValidateFlexReferenceVolumeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateFlexReferenceVolumeDataAPI, _systemAPIPasswordEnums.ValidateFlexReferenceVolumeDataAPI);
            validateFlexReferenceVolumeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateFlexReferenceVolumeDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFlexReferenceVolumeData/Validate")]
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
                    validateFlexReferenceVolumeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateFlexReferenceVolumeDataAPI, validateFlexReferenceVolumeDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateFlexReferenceVolumeDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexReferenceVolume] table
                var flexReferenceVolumeDataRows = _tempCustomerDataUploadMethods.FlexReferenceVolume_GetByProcessQueueGUID(processQueueGUID);

                if(!flexReferenceVolumeDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexReferenceVolumeDataAPIId, false, null);
                    return;
                }

                var columns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.ContractReference, "Contract Reference"},
                        {_customerDataUploadValidationEntityEnums.DateFrom, "Date From"},
                        {_customerDataUploadValidationEntityEnums.DateTo, "Date To"},
                        {_customerDataUploadValidationEntityEnums.Volume, _customerDataUploadValidationEntityEnums.Volume}
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(flexReferenceVolumeDataRows, columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.ContractReference, "Contract Reference"}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, flexReferenceVolumeDataRows, requiredColumns);

                //Validate Contract Dates
                var invalidDateFromDataRecords = flexReferenceVolumeDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.DateFrom))
                    && !_methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.DateFrom)));

                foreach(var invalidDateFromDataRecord in invalidDateFromDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidDateFromDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.DateFrom].Contains($"Invalid Date From '{invalidDateFromDataRecord[_customerDataUploadValidationEntityEnums.DateFrom]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.DateFrom].Add($"Invalid Date From '{invalidDateFromDataRecord[_customerDataUploadValidationEntityEnums.DateFrom]}'");
                    }
                }

                var invalidDateToDataRecords = flexReferenceVolumeDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.DateTo))
                    && !_methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.DateTo)));

                foreach(var invalidDateToDataRecord in invalidDateToDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidDateToDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.DateTo].Contains($"Invalid Date to '{invalidDateToDataRecord[_customerDataUploadValidationEntityEnums.DateTo]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.DateTo].Add($"Invalid Date to '{invalidDateToDataRecord[_customerDataUploadValidationEntityEnums.DateTo]}'");
                    }
                }

                var invalidContractDateDataRecords = flexReferenceVolumeDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.DateFrom))
                    && !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.DateTo))
                    && _methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.DateFrom))
                    && _methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.DateTo))
                    && r.Field<DateTime>(_customerDataUploadValidationEntityEnums.DateFrom) >= r.Field<DateTime>(_customerDataUploadValidationEntityEnums.DateTo));

                foreach(var invalidDateToDataRecord in invalidDateToDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidDateToDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.DateFrom].Contains($"Invalid Contract Dates '{invalidDateToDataRecord[_customerDataUploadValidationEntityEnums.DateFrom]}' is equal to or later than '{invalidDateToDataRecord[_customerDataUploadValidationEntityEnums.DateTo]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.DateFrom].Add($"Invalid Contract Dates '{invalidDateToDataRecord[_customerDataUploadValidationEntityEnums.DateFrom]}' is equal to or later than '{invalidDateToDataRecord[_customerDataUploadValidationEntityEnums.DateTo]}'");
                    }
                }

                //Validate Volume
                var invalidVolumeDataRecords = flexReferenceVolumeDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.Volume))
                    && !_methods.IsValidFlexReferenceVolume(r.Field<string>(_customerDataUploadValidationEntityEnums.Volume)));

                foreach(var invalidVolumeDataRecord in invalidVolumeDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidVolumeDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.Volume].Contains($"Invalid Reference Volume '{invalidVolumeDataRecord[_customerDataUploadValidationEntityEnums.Volume]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.Volume].Add($"Invalid Reference Volume '{invalidVolumeDataRecord[_customerDataUploadValidationEntityEnums.Volume]}'");
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