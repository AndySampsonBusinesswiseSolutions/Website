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

namespace ValidateUsageUploadTempMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateUsageUploadTempMeterDataController : ControllerBase
    {
        private readonly ILogger<ValidateUsageUploadTempMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Int64 validateUsageUploadTempMeterDataAPIId;

        public ValidateUsageUploadTempMeterDataController(ILogger<ValidateUsageUploadTempMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateUsageUploadTempMeterDataAPI, _systemAPIPasswordEnums.ValidateUsageUploadTempMeterDataAPI);
            validateUsageUploadTempMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateUsageUploadTempMeterDataAPI);
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());            
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(validateUsageUploadTempMeterDataAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempMeterData/Validate")]
        public void Validate([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    validateUsageUploadTempMeterDataAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.ValidateUsageUploadTempMeterDataAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempMeterDataAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get data from [Temp.Customer].[Meter] table
                var customerDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempMeterDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"MPXN", "MPAN/MPRN"},
                        {"AnnualUsage", "Annual Usage"}
                    };
                
                var errors = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns).ToList();

                //Get MPANs
                var mpanDataRecords = customerDataRows.Where(r => _methods.IsValidMPAN(r.Field<string>("MPXN")));
                var mpanRows = mpanDataRecords.Select(r => r.Field<int>("RowId"));

                //Get MPRNs
                var mprnDataRecords = customerDataRows.Where(r => _methods.IsValidMPRN(r.Field<string>("MPXN")));
                var mprnRows = mprnDataRecords.Select(r => r.Field<int>("RowId"));

                //Get any MPXNs not in MPANs or MPRNs
                var invalidMPXNRows = customerDataRows.Select(r => r.Field<int>("RowId")).Except(mpanRows).Except(mprnRows);

                foreach(var row in invalidMPXNRows)
                {
                    var dataRow = customerDataRows.First(r => r.Field<int>("RowId") == row);
                    errors.Add($"Invalid MPAN/MPRN {dataRow["MPXN"]} in row {row}");
                }

                //Get MPANs not stored in database
                //GSP, PC, MTC, LLFC and Area must be populated
                //Validate Capacity if it is populated

                //Get MPRNs not stored in database
                //LDZ and Area must be populated
                //Validate SOQ if it is populated

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

