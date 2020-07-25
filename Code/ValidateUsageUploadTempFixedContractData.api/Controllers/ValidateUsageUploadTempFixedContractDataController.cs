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

namespace ValidateUsageUploadTempFixedContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateUsageUploadTempFixedContractDataController : ControllerBase
    {
        private readonly ILogger<ValidateUsageUploadTempFixedContractDataController> _logger;
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
        private readonly Int64 validateUsageUploadTempFixedContractDataAPIId;

        public ValidateUsageUploadTempFixedContractDataController(ILogger<ValidateUsageUploadTempFixedContractDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateUsageUploadTempFixedContractDataAPI, _systemAPIPasswordEnums.ValidateUsageUploadTempFixedContractDataAPI);
            validateUsageUploadTempFixedContractDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateUsageUploadTempFixedContractDataAPI);
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempFixedContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());            
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(validateUsageUploadTempFixedContractDataAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempFixedContractData/Validate")]
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
                    validateUsageUploadTempFixedContractDataAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.ValidateUsageUploadTempFixedContractDataAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempFixedContractDataAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get data from [Temp.Customer].[FixedContract] table
                var customerDataRows = _tempCustomerMethods.FixedContract_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempFixedContractDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"ContractReference", "Contract Reference"},
                        {"MPXN", "MPAN/MPRN"},
                        {"Supplier", "Supplier"},
                        {"ContractStartDate", "Contract Start Date"},
                        {"ContractEndDate", "Contract End Date"}
                    };

                var errors = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns).ToList();

                //TODO: If Contract Reference and MPXN doesn't exist then Product, Rate Count, Number of rates and costs are required
                
                //Validate MPXN
                var invalidMPXNDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("MPXN"))
                    && !_methods.IsValidMPXN(r.Field<string>("MPXN")));

                foreach(var invalidMPXNDataRecord in invalidMPXNDataRecords)
                {
                    errors.Add($"Invalid MPAN/MPRN '{invalidMPXNDataRecord["MPXN"]}' in row {invalidMPXNDataRecord["RowId"]}");
                }

                //Validate Supplier
                var invalidSupplierDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Supplier"))
                    && !_methods.IsValidSupplier(r.Field<string>("Supplier")));

                foreach(var invalidSupplierDataRecord in invalidSupplierDataRecords)
                {
                    errors.Add($"Invalid Supplier '{invalidSupplierDataRecord["Supplier"]}' in row {invalidSupplierDataRecord["RowId"]}");
                }

                //Validate Contract Dates
                var invalidContractStartDateDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContractStartDate"))
                    && !_methods.IsValidDate(r.Field<string>("ContractStartDate")));

                foreach(var invalidContractStartDateDataRecord in invalidContractStartDateDataRecords)
                {
                    errors.Add($"Invalid Contract Start Date '{invalidContractStartDateDataRecord["ContractStartDate"]}' in row {invalidContractStartDateDataRecord["RowId"]}");
                }

                var invalidContractEndDateDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContractEndDate"))
                    && !_methods.IsValidDate(r.Field<string>("ContractEndDate")));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    errors.Add($"Invalid Contract End Date '{invalidContractEndDateDataRecord["ContractEndDate"]}' in row {invalidContractEndDateDataRecord["RowId"]}");
                }

                var invalidContractDateDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContractStartDate"))
                    && !string.IsNullOrWhiteSpace(r.Field<string>("ContractEndDate"))
                    && _methods.IsValidDate(r.Field<string>("ContractStartDate"))
                    && _methods.IsValidDate(r.Field<string>("ContractEndDate"))
                    && r.Field<DateTime>("ContractStartDate") >= r.Field<DateTime>("ContractEndDate"));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    errors.Add($"Invalid Contract Dates '{invalidContractEndDateDataRecord["ContractStartDate"]}' is equal to or later than '{invalidContractEndDateDataRecord["ContractEndDate"]}' in row {invalidContractEndDateDataRecord["RowId"]}");
                }

                //Validate Rates
                var invalidRateDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Value"))
                    && !_methods.IsValidFixedContractRate(r.Field<string>("Value")));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    errors.Add($"Invalid Rate Value '{invalidRateDataRecord["Value"]}' in row {invalidRateDataRecord["RowId"]}");
                }

                invalidRateDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("StandingCharge"))
                    && !_methods.IsValidFixedContactStandingCharge(r.Field<string>("StandingCharge")));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    errors.Add($"Invalid Standing Charge Value '{invalidRateDataRecord["StandingCharge"]}' in row {invalidRateDataRecord["RowId"]}");
                }

                invalidRateDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("CapacityCharge"))
                    && !_methods.IsValidFixedContactCapacityCharge(r.Field<string>("CapacityCharge")));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    errors.Add($"Invalid Capacity Charge Value '{invalidRateDataRecord["CapacityCharge"]}' in row {invalidRateDataRecord["RowId"]}");
                }

                //Validate Rate Count
                var validRateDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Value"))
                    && _methods.IsValidFixedContractRate(r.Field<string>("Value")));

                var contractMPXNDictionary = new Dictionary<string, List<string>>();
                foreach(var validRateDataRecord in validRateDataRecords)
                {
                    if(!contractMPXNDictionary.ContainsKey(validRateDataRecord.Field<string>("ContractReference")))
                    {
                        contractMPXNDictionary.Add(validRateDataRecord.Field<string>("ContractReference"), new List<string>());
                    }

                    if(!contractMPXNDictionary[validRateDataRecord.Field<string>("ContractReference")].Contains(validRateDataRecord.Field<string>("MPXN")))
                    {
                        contractMPXNDictionary[validRateDataRecord.Field<string>("ContractReference")].Add(validRateDataRecord.Field<string>("MPXN"));
                    }
                }

                foreach(var contract in contractMPXNDictionary)
                {
                    foreach(var mpxn in contract.Value)
                    {
                        var fixedContractDataRecords = validRateDataRecords.Where(r => r.Field<string>("ContractReference") == contract.Key
                            && r.Field<string>("MPXN") == mpxn);
                        var rateCount = fixedContractDataRecords.Select(r => r.Field<int>("RateCount")).First();

                        if(rateCount != fixedContractDataRecords.Count())
                        {
                            errors.Add($"Rate Count '{rateCount}' does not match number of rates provided in row {fixedContractDataRecords.First()["RowId"]}");
                        }
                    }
                }

                //Update Process Queue
                var errorMessage = errors.Any() ? string.Join(';', errors) : null;
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempFixedContractDataAPIId, errors.Any(), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempFixedContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}