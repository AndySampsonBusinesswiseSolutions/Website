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
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
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
                        {_customerDataUploadValidationEntityEnums.ContractReference, "Contract Reference"},
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.Supplier, _customerDataUploadValidationEntityEnums.Supplier},
                        {_customerDataUploadValidationEntityEnums.ContractStartDate, "Contract Start Date"},
                        {_customerDataUploadValidationEntityEnums.ContractEndDate, "Contract End Date"},
                        {_customerDataUploadValidationEntityEnums.Product, _customerDataUploadValidationEntityEnums.Product},
                        {_customerDataUploadValidationEntityEnums.RateCount, "Rate Count"},
                        {_customerDataUploadValidationEntityEnums.StandingCharge, "Standing Charge"},
                        {_customerDataUploadValidationEntityEnums.CapacityCharge, "Capacity Charge"},
                        {_customerDataUploadValidationEntityEnums.Rate, _customerDataUploadValidationEntityEnums.Rate},
                        {_customerDataUploadValidationEntityEnums.Value, _customerDataUploadValidationEntityEnums.Value}
                    };

                var records = _tempCustomerMethods.InitialiseRecordsDictionary(fixedContractDataRows, columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.ContractReference, "Contract Reference"},
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.Supplier, _customerDataUploadValidationEntityEnums.Supplier},
                        {_customerDataUploadValidationEntityEnums.ContractStartDate, "Contract Start Date"},
                        {_customerDataUploadValidationEntityEnums.ContractEndDate, "Contract End Date"}
                    };

                _tempCustomerMethods.GetMissingRecords(records, fixedContractDataRows, requiredColumns);

                //If Contract Reference and MPXN doesn't exist then Product, Rate Count, Number of rates and costs are required
                var newContractMeterDataRecords = fixedContractDataRows.Where(r => 
                    !_customerMethods.ContractMeterExists(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference), r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)));

                requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.Product, _customerDataUploadValidationEntityEnums.Product},
                        {_customerDataUploadValidationEntityEnums.RateCount, "Rate Count"},
                        {"Standingcharge", "Standing Charge"},
                        {_customerDataUploadValidationEntityEnums.CapacityCharge, "Capacity Charge"}
                    };
                _tempCustomerMethods.GetMissingRecords(records, newContractMeterDataRecords, requiredColumns);

                //Get new contracts
                var newContractDataRecords = fixedContractDataRows.Where(r => string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference)));

                //Product must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.Product, _customerDataUploadValidationEntityEnums.Product},
                        {_customerDataUploadValidationEntityEnums.RateCount, "Rate Count"}
                    };
                _tempCustomerMethods.GetMissingRecords(records, newContractDataRecords, requiredColumns);

                //Validate Supplier
                var invalidSupplierDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.Supplier))
                    && !_methods.IsValidSupplier(r.Field<string>(_customerDataUploadValidationEntityEnums.Supplier)));

                foreach(var invalidSupplierDataRecord in invalidSupplierDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidSupplierDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.Supplier].Contains($"Invalid Supplier '{invalidSupplierDataRecord[_customerDataUploadValidationEntityEnums.Supplier]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.Supplier].Add($"Invalid Supplier '{invalidSupplierDataRecord[_customerDataUploadValidationEntityEnums.Supplier]}'");
                    }
                }

                //Validate Contract Dates
                var invalidContractStartDateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractStartDate))
                    && !_methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractStartDate)));

                foreach(var invalidContractStartDateDataRecord in invalidContractStartDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidContractStartDateDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.ContractStartDate].Contains($"Invalid Contract Start Date '{invalidContractStartDateDataRecord[_customerDataUploadValidationEntityEnums.ContractStartDate]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.ContractStartDate].Add($"Invalid Contract Start Date '{invalidContractStartDateDataRecord[_customerDataUploadValidationEntityEnums.ContractStartDate]}'");
                    }
                }

                var invalidContractEndDateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractEndDate))
                    && !_methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractEndDate)));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidContractEndDateDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.ContractEndDate].Contains($"Invalid Contract End Date '{invalidContractEndDateDataRecord[_customerDataUploadValidationEntityEnums.ContractEndDate]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.ContractEndDate].Add($"Invalid Contract End Date '{invalidContractEndDateDataRecord[_customerDataUploadValidationEntityEnums.ContractEndDate]}'");
                    }
                }

                var invalidContractDateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractStartDate))
                    && !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractEndDate))
                    && _methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractStartDate))
                    && _methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractEndDate))
                    && r.Field<DateTime>(_customerDataUploadValidationEntityEnums.ContractStartDate) >= r.Field<DateTime>(_customerDataUploadValidationEntityEnums.ContractEndDate));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidContractEndDateDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.ContractStartDate].Contains($"Invalid Contract Dates '{invalidContractEndDateDataRecord[_customerDataUploadValidationEntityEnums.ContractStartDate]}' is equal to or later than '{invalidContractEndDateDataRecord[_customerDataUploadValidationEntityEnums.ContractEndDate]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.ContractStartDate].Add($"Invalid Contract Dates '{invalidContractEndDateDataRecord[_customerDataUploadValidationEntityEnums.ContractStartDate]}' is equal to or later than '{invalidContractEndDateDataRecord[_customerDataUploadValidationEntityEnums.ContractEndDate]}'");
                    }
                }

                //Validate Rates
                var invalidRateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.Value))
                    && !_methods.IsValidFixedContractRate(r.Field<string>(_customerDataUploadValidationEntityEnums.Value)));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Rate Value '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.Value]}' for 'Rate {invalidRateDataRecord[_customerDataUploadValidationEntityEnums.Rate]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid Rate Value '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.Value]}' for 'Rate {invalidRateDataRecord[_customerDataUploadValidationEntityEnums.Rate]}'");
                    }
                }

                invalidRateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.StandingCharge))
                    && !_methods.IsValidFixedContractStandingCharge(r.Field<string>(_customerDataUploadValidationEntityEnums.StandingCharge)));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.StandingCharge].Contains($"Invalid Standing Charge Value '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.StandingCharge]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.StandingCharge].Add($"Invalid Standing Charge Value '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.StandingCharge]}'");
                    }
                }

                invalidRateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.CapacityCharge))
                    && !_methods.IsValidFixedContractCapacityCharge(r.Field<string>(_customerDataUploadValidationEntityEnums.CapacityCharge)));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.CapacityCharge].Contains($"Invalid Capacity Charge Value '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.CapacityCharge]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.CapacityCharge].Add($"Invalid Capacity Charge Value '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.CapacityCharge]}'");
                    }
                }

                //Validate Rate Count
                var invalidRateCountDataRecords = fixedContractDataRows.Where(r => string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.RateCount))
                    || !_methods.IsValidFixedContractRateCount(r.Field<string>(_customerDataUploadValidationEntityEnums.RateCount)));

                foreach(var invalidRateCountDataRecord in invalidRateCountDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateCountDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.RateCount].Contains($"Invalid Rate Count Value '{invalidRateCountDataRecord[_customerDataUploadValidationEntityEnums.RateCount]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.RateCount].Add($"Invalid Rate Count Value '{invalidRateCountDataRecord[_customerDataUploadValidationEntityEnums.RateCount]}'");
                    }
                }

                var validRateDataRecords = fixedContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.Value))
                    && _methods.IsValidFixedContractRate(r.Field<string>(_customerDataUploadValidationEntityEnums.Value))
                    && _methods.IsValidFixedContractRateCount(r.Field<string>(_customerDataUploadValidationEntityEnums.RateCount)));

                var contractMPXNDictionary = new Dictionary<string, List<string>>();
                foreach(var validRateDataRecord in validRateDataRecords)
                {
                    if(!contractMPXNDictionary.ContainsKey(validRateDataRecord.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference)))
                    {
                        contractMPXNDictionary.Add(validRateDataRecord.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference), new List<string>());
                    }

                    if(!contractMPXNDictionary[validRateDataRecord.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference)].Contains(validRateDataRecord.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)))
                    {
                        contractMPXNDictionary[validRateDataRecord.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference)].Add(validRateDataRecord.Field<string>(_customerDataUploadValidationEntityEnums.MPXN));
                    }
                }

                foreach(var contract in contractMPXNDictionary)
                {
                    foreach(var mpxn in contract.Value)
                    {
                        var fixedContractDataRecords = validRateDataRecords.Where(r => r.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference) == contract.Key
                            && r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN) == mpxn);
                        var rateCount = fixedContractDataRecords.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.RateCount)).First();

                        if(rateCount != fixedContractDataRecords.Count().ToString())
                        {
                            var rowId = Convert.ToInt32(fixedContractDataRecords.First()["RowId"]);
                            if(!records[rowId][_customerDataUploadValidationEntityEnums.RateCount].Contains($"Rate Count '{rateCount}' does not match number of rates provided"))
                            {
                                records[rowId][_customerDataUploadValidationEntityEnums.RateCount].Add($"Rate Count '{rateCount}' does not match number of rates provided");
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