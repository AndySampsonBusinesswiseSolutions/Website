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

namespace ValidateUsageUploadTempFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateUsageUploadTempFlexReferenceVolumeDataController : ControllerBase
    {
        private readonly ILogger<ValidateUsageUploadTempFlexReferenceVolumeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 validateUsageUploadTempFlexReferenceVolumeDataAPIId;

        public ValidateUsageUploadTempFlexReferenceVolumeDataController(ILogger<ValidateUsageUploadTempFlexReferenceVolumeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateUsageUploadTempFlexReferenceVolumeDataAPI, _systemAPIPasswordEnums.ValidateUsageUploadTempFlexReferenceVolumeDataAPI);
            validateUsageUploadTempFlexReferenceVolumeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateUsageUploadTempFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateUsageUploadTempFlexReferenceVolumeDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempFlexReferenceVolumeData/Validate")]
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
                    validateUsageUploadTempFlexReferenceVolumeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateUsageUploadTempFlexReferenceVolumeDataAPI, validateUsageUploadTempFlexReferenceVolumeDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.Customer].[FlexReferenceVolume] table
                var customerDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempFlexReferenceVolumeDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"ContractReference", "Contract Reference"}
                    };

                var errors = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns).ToList();

                //Validate Contract Dates
                var invalidDateFromDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("DateFrom"))
                    && !_methods.IsValidDate(r.Field<string>("DateFrom")));

                foreach(var invalidDateFromDataRecord in invalidDateFromDataRecords)
                {
                    errors.Add($"Invalid Date From '{invalidDateFromDataRecord["DateFrom"]}' in row {invalidDateFromDataRecord["RowId"]}");
                }

                var invalidDateToDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("DateTo"))
                    && !_methods.IsValidDate(r.Field<string>("DateTo")));

                foreach(var invalidDateToDataRecord in invalidDateToDataRecords)
                {
                    errors.Add($"Invalid Date to '{invalidDateToDataRecord["DateTo"]}' in row {invalidDateToDataRecord["RowId"]}");
                }

                var invalidContractDateDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("DateFrom"))
                    && !string.IsNullOrWhiteSpace(r.Field<string>("DateTo"))
                    && _methods.IsValidDate(r.Field<string>("DateFrom"))
                    && _methods.IsValidDate(r.Field<string>("DateTo"))
                    && r.Field<DateTime>("DateFrom") >= r.Field<DateTime>("DateTo"));

                foreach(var invalidDateToDataRecord in invalidDateToDataRecords)
                {
                    errors.Add($"Invalid Contract Dates '{invalidDateToDataRecord["DateFrom"]}' is equal to or later than '{invalidDateToDataRecord["DateTo"]}' in row {invalidDateToDataRecord["RowId"]}");
                }

                //Validate Volume
                var invalidVolumeDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Volume"))
                    && !_methods.IsValidFlexReferenceVolume(r.Field<string>("Volume")));

                foreach(var invalidVolumeDataRecord in invalidVolumeDataRecords)
                {
                    errors.Add($"Invalid Reference Volume '{invalidVolumeDataRecord["Volume"]}' in row {invalidVolumeDataRecord["RowId"]}");
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempFlexReferenceVolumeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

