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
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
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

                //Get data from [Temp.CustomerDataUpload].[FlexReferenceVolume] table
                var flexReferenceVolumeDataRows = _tempCustomerMethods.FlexReferenceVolume_GetByProcessQueueGUID(processQueueGUID);

                if(!flexReferenceVolumeDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateFlexReferenceVolumeDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"ContractReference", "Contract Reference"}
                    };

                var records = _tempCustomerMethods.GetMissingRecords(flexReferenceVolumeDataRows, requiredColumns);

                //Validate Contract Dates
                var invalidDateFromDataRecords = flexReferenceVolumeDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("DateFrom"))
                    && !_methods.IsValidDate(r.Field<string>("DateFrom")));

                foreach(var invalidDateFromDataRecord in invalidDateFromDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidDateFromDataRecord["RowId"]);
                    records[rowId]["DateFrom"].Add($"Invalid Date From '{invalidDateFromDataRecord["DateFrom"]}'");
                }

                var invalidDateToDataRecords = flexReferenceVolumeDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("DateTo"))
                    && !_methods.IsValidDate(r.Field<string>("DateTo")));

                foreach(var invalidDateToDataRecord in invalidDateToDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidDateToDataRecord["RowId"]);
                    records[rowId]["DateTo"].Add($"Invalid Date to '{invalidDateToDataRecord["DateTo"]}'");
                }

                var invalidContractDateDataRecords = flexReferenceVolumeDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("DateFrom"))
                    && !string.IsNullOrWhiteSpace(r.Field<string>("DateTo"))
                    && _methods.IsValidDate(r.Field<string>("DateFrom"))
                    && _methods.IsValidDate(r.Field<string>("DateTo"))
                    && r.Field<DateTime>("DateFrom") >= r.Field<DateTime>("DateTo"));

                foreach(var invalidDateToDataRecord in invalidDateToDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidDateToDataRecord["RowId"]);
                    records[rowId]["DateFrom"].Add($"Invalid Contract Dates '{invalidDateToDataRecord["DateFrom"]}' is equal to or later than '{invalidDateToDataRecord["DateTo"]}'");
                }

                //Validate Volume
                var invalidVolumeDataRecords = flexReferenceVolumeDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Volume"))
                    && !_methods.IsValidFlexReferenceVolume(r.Field<string>("Volume")));

                foreach(var invalidVolumeDataRecord in invalidVolumeDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidVolumeDataRecord["RowId"]);
                    records[rowId]["Volume"].Add($"Invalid Reference Volume '{invalidVolumeDataRecord["Volume"]}'");
                }

                //Update Process Queue
                var errorMessage = _tempCustomerMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.FlexReferenceVolume);
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateFlexReferenceVolumeDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

