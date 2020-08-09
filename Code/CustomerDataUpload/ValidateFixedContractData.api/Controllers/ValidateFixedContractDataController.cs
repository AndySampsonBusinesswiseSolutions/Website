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

namespace ValidateFixedContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFixedContractDataController : ControllerBase
    {
        private readonly ILogger<ValidateFixedContractDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private readonly Int64 validateFixedContractDataAPIId;

        public ValidateFixedContractDataController(ILogger<ValidateFixedContractDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateFixedContractDataAPI, _systemAPIPasswordEnums.ValidateFixedContractDataAPI);
            validateFixedContractDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateFixedContractDataAPI);
        }

        [HttpPost]
        [Route("ValidateFixedContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateFixedContractDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFixedContractData/Validate")]
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
                    validateFixedContractDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateFixedContractDataAPI, validateFixedContractDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[FixedContract] table
                var fixedContractDataRows = _tempCustomerMethods.FixedContract_GetByProcessQueueGUID(processQueueGUID);

                if(!fixedContractDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateFixedContractDataAPIId, false, null);
                    return;
                }

                var columns = new Dictionary<string, string>
                    {
                        {"ContractReference", "Contract Reference"},
                        {"MPXN", "MPAN/MPRN"},
                        {"Supplier", "Supplier"},
                        {"ContractStartDate", "Contract Start Date"},
                        {"ContractEndDate", "Contract End Date"},
                        {"Product", "Product"},
                        {"RateCount", "Rate Count"},
                        {"StandingCharge", "Standing Charge"},
                        {"CapacityCharge", "Capacity Charge"},
                        {"Rate", "Rate"},
                        {"Value", "Value"}
                    };

                var records = _tempCustomerMethods.InitialiseRecordsDictionary(fixedContractDataRows, columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"ContractReference", "Contract Reference"},
                        {"MPXN", "MPAN/MPRN"},
                        {"Supplier", "Supplier"},
                        {"ContractStartDate", "Contract Start Date"},
                        {"ContractEndDate", "Contract End Date"}
                    };

                _tempCustomerMethods.GetMissingRecords(records, fixedContractDataRows, requiredColumns);

                //If Contract Reference and MPXN doesn't exist then Product, Rate Count, Number of rates and costs are required
                var newContractMeterDataRecords = fixedContractDataRows.Where(r => 
                    !_customerMethods.ContractMeterExists(r.Field<string>("ContractReference"), r.Field<string>("MPXN")));

                requiredColumns = new Dictionary<string, string>
                    {
                        {"Product", "Product"},
                        {"RateCount", "Rate Count"},
                        {"Standingcharge", "Standing Charge"},
                        {"CapacityCharge", "Capacity Charge"}
                    };
                _tempCustomerMethods.GetMissingRecords(records, newContractMeterDataRecords, requiredColumns);

                //Get new contracts
                var newContractDataRecords = fixedContractDataRows.Where(r => string.IsNullOrWhiteSpace(r.Field<string>("ContractReference")));

                //Product must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {"Product", "Product"},
                        {"RateCount", "Rate Count"}
                    };
                _tempCustomerMethods.GetMissingRecords(records, newContractDataRecords, requiredColumns);

                //Validate Supplier
                var invalidSupplierDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Supplier"))
                    && !_methods.IsValidSupplier(r.Field<string>("Supplier")));

                foreach(var invalidSupplierDataRecord in invalidSupplierDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidSupplierDataRecord["RowId"]);
                    if(!records[rowId]["Supplier"].Contains($"Invalid Supplier '{invalidSupplierDataRecord["Supplier"]}'"))
                    {
                        records[rowId]["Supplier"].Add($"Invalid Supplier '{invalidSupplierDataRecord["Supplier"]}'");
                    }
                }

                //Validate Contract Dates
                var invalidContractStartDateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContractStartDate"))
                    && !_methods.IsValidDate(r.Field<string>("ContractStartDate")));

                foreach(var invalidContractStartDateDataRecord in invalidContractStartDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidContractStartDateDataRecord["RowId"]);
                    if(!records[rowId]["ContractStartDate"].Contains($"Invalid Contract Start Date '{invalidContractStartDateDataRecord["ContractStartDate"]}'"))
                    {
                        records[rowId]["ContractStartDate"].Add($"Invalid Contract Start Date '{invalidContractStartDateDataRecord["ContractStartDate"]}'");
                    }
                }

                var invalidContractEndDateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContractEndDate"))
                    && !_methods.IsValidDate(r.Field<string>("ContractEndDate")));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidContractEndDateDataRecord["RowId"]);
                    if(!records[rowId]["ContractEndDate"].Contains($"Invalid Contract End Date '{invalidContractEndDateDataRecord["ContractEndDate"]}'"))
                    {
                        records[rowId]["ContractEndDate"].Add($"Invalid Contract End Date '{invalidContractEndDateDataRecord["ContractEndDate"]}'");
                    }
                }

                var invalidContractDateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContractStartDate"))
                    && !string.IsNullOrWhiteSpace(r.Field<string>("ContractEndDate"))
                    && _methods.IsValidDate(r.Field<string>("ContractStartDate"))
                    && _methods.IsValidDate(r.Field<string>("ContractEndDate"))
                    && r.Field<DateTime>("ContractStartDate") >= r.Field<DateTime>("ContractEndDate"));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidContractEndDateDataRecord["RowId"]);
                    if(!records[rowId]["ContractStartDate"].Contains($"Invalid Contract Dates '{invalidContractEndDateDataRecord["ContractStartDate"]}' is equal to or later than '{invalidContractEndDateDataRecord["ContractEndDate"]}'"))
                    {
                        records[rowId]["ContractStartDate"].Add($"Invalid Contract Dates '{invalidContractEndDateDataRecord["ContractStartDate"]}' is equal to or later than '{invalidContractEndDateDataRecord["ContractEndDate"]}'");
                    }
                }

                //Validate Rates
                var invalidRateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Value"))
                    && !_methods.IsValidFixedContractRate(r.Field<string>("Value")));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateDataRecord["RowId"]);
                    if(!records[rowId]["Value"].Contains($"Invalid Rate Value '{invalidRateDataRecord["Value"]}' for 'Rate {invalidRateDataRecord["Rate"]}'"))
                    {
                        records[rowId]["Value"].Add($"Invalid Rate Value '{invalidRateDataRecord["Value"]}' for 'Rate {invalidRateDataRecord["Rate"]}'");
                    }
                }

                invalidRateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("StandingCharge"))
                    && !_methods.IsValidFixedContractStandingCharge(r.Field<string>("StandingCharge")));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateDataRecord["RowId"]);
                    if(!records[rowId]["StandingCharge"].Contains($"Invalid Standing Charge Value '{invalidRateDataRecord["StandingCharge"]}'"))
                    {
                        records[rowId]["StandingCharge"].Add($"Invalid Standing Charge Value '{invalidRateDataRecord["StandingCharge"]}'");
                    }
                }

                invalidRateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("CapacityCharge"))
                    && !_methods.IsValidFixedContractCapacityCharge(r.Field<string>("CapacityCharge")));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateDataRecord["RowId"]);
                    if(!records[rowId]["CapacityCharge"].Contains($"Invalid Capacity Charge Value '{invalidRateDataRecord["CapacityCharge"]}'"))
                    {
                        records[rowId]["CapacityCharge"].Add($"Invalid Capacity Charge Value '{invalidRateDataRecord["CapacityCharge"]}'");
                    }
                }

                //Validate Rate Count
                var invalidRateCountDataRecords = fixedContractDataRows.Where(r => string.IsNullOrWhiteSpace(r.Field<string>("RateCount"))
                    || !_methods.IsValidFixedContractRateCount(r.Field<string>("RateCount")));

                foreach(var invalidRateCountDataRecord in invalidRateCountDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateCountDataRecord["RowId"]);
                    if(!records[rowId]["RateCount"].Contains($"Invalid Rate Count Value '{invalidRateCountDataRecord["RateCount"]}'"))
                    {
                        records[rowId]["RateCount"].Add($"Invalid Rate Count Value '{invalidRateCountDataRecord["RateCount"]}'");
                    }
                }

                var validRateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Value"))
                    && _methods.IsValidFixedContractRate(r.Field<string>("Value"))
                    && _methods.IsValidFixedContractRateCount(r.Field<string>("RateCount")));

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
                        var rateCount = fixedContractDataRecords.Select(r => r.Field<string>("RateCount")).First();

                        if(rateCount != fixedContractDataRecords.Count().ToString())
                        {
                            var rowId = Convert.ToInt32(fixedContractDataRecords.First()["RowId"]);
                            if(!records[rowId]["RateCount"].Contains($"Rate Count '{rateCount}' does not match number of rates provided"))
                            {
                                records[rowId]["RateCount"].Add($"Rate Count '{rateCount}' does not match number of rates provided");
                            }
                        }
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.FixedContract);
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateFixedContractDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateFixedContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}