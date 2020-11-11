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

namespace ValidateFixedContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFixedContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateFixedContractDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private static readonly Enums.CustomerSchema.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.CustomerSchema.DataUploadValidation.SheetName();
        private static readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 validateFixedContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateFixedContractDataController(ILogger<ValidateFixedContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateFixedContractDataAPI, password);
            validateFixedContractDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateFixedContractDataAPI);
        }

        [HttpPost]
        [Route("ValidateFixedContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateFixedContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFixedContractData/Validate")]
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
                    validateFixedContractDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateFixedContractDataAPI, validateFixedContractDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateFixedContractDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FixedContract] table
                var fixedContractEntities = new Methods.Temp.CustomerDataUpload.FixedContract().FixedContract_GetByProcessQueueGUID(processQueueGUID);

                if(!fixedContractEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFixedContractDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();

                var columns = new List<string>
                    {
                        _customerDataUploadValidationEntityEnums.ContractReference,
                        _customerDataUploadValidationEntityEnums.MPXN,
                        _customerDataUploadValidationEntityEnums.Supplier,
                        _customerDataUploadValidationEntityEnums.ContractStartDate,
                        _customerDataUploadValidationEntityEnums.ContractEndDate,
                        _customerDataUploadValidationEntityEnums.Product,
                        _customerDataUploadValidationEntityEnums.RateCount,
                        _customerDataUploadValidationEntityEnums.RateType,
                        _customerDataUploadValidationEntityEnums.Value,
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(fixedContractEntities.Select(fce => fce.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.ContractReference, "Contract Reference"},
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.Supplier, _customerDataUploadValidationEntityEnums.Supplier},
                        {_customerDataUploadValidationEntityEnums.ContractStartDate, "Contract Start Date"},
                        {_customerDataUploadValidationEntityEnums.ContractEndDate, "Contract End Date"}
                    };

                _tempCustomerDataUploadMethods.GetMissingRecords(records, fixedContractEntities, requiredColumns);

                //If Contract Reference and MPXN doesn't exist then Product, Rate Count, Number of rates and costs are required
                var newContractMeterEntities = fixedContractEntities.Where(fce => 
                    !_customerMethods.ContractMeterExists(fce.ContractReference, fce.MPXN))
                    .ToList();

                requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.Product, _customerDataUploadValidationEntityEnums.Product},
                        {_customerDataUploadValidationEntityEnums.RateCount, "Rate Count"},
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, newContractMeterEntities, requiredColumns);

                //Get new contracts
                var newContractEntities = fixedContractEntities.Where(fce => string.IsNullOrWhiteSpace(fce.ContractReference))
                    .ToList();

                //Product must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.Product, _customerDataUploadValidationEntityEnums.Product},
                        {_customerDataUploadValidationEntityEnums.RateCount, "Rate Count"}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, newContractEntities, requiredColumns);

                //Validate Supplier
                var invalidSupplierEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Supplier)
                    && !methods.IsValidSupplier(fce.Supplier));

                foreach(var invalidSupplierEntity in invalidSupplierEntities)
                {
                    if(!records[invalidSupplierEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Supplier].Contains($"Invalid Supplier '{invalidSupplierEntity.Supplier}'"))
                    {
                        records[invalidSupplierEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Supplier].Add($"Invalid Supplier '{invalidSupplierEntity.Supplier}'");
                    }
                }

                //Validate Contract Dates
                var invalidContractStartDateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractStartDate)
                    && !methods.IsValidDate(fce.ContractStartDate));

                foreach(var invalidContractStartDateEntity in invalidContractStartDateEntities)
                {
                    if(!records[invalidContractStartDateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ContractStartDate].Contains($"Invalid Contract Start Date '{invalidContractStartDateEntity.ContractStartDate}'"))
                    {
                        records[invalidContractStartDateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ContractStartDate].Add($"Invalid Contract Start Date '{invalidContractStartDateEntity.ContractStartDate}'");
                    }
                }

                var invalidContractEndDateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractEndDate)
                    && !methods.IsValidDate(fce.ContractEndDate));

                foreach(var invalidContractEndDateEntity in invalidContractEndDateEntities)
                {
                    if(!records[invalidContractEndDateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ContractEndDate].Contains($"Invalid Contract End Date '{invalidContractEndDateEntity.ContractEndDate}'"))
                    {
                        records[invalidContractEndDateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ContractEndDate].Add($"Invalid Contract End Date '{invalidContractEndDateEntity.ContractEndDate}'");
                    }
                }

                var invalidContractDateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractStartDate)
                    && !string.IsNullOrWhiteSpace(fce.ContractEndDate) && methods.IsValidDate(fce.ContractStartDate)
                    && methods.IsValidDate(fce.ContractEndDate) && Convert.ToDateTime(fce.ContractStartDate) >= Convert.ToDateTime(fce.ContractEndDate));

                foreach(var invalidContractEndDateEntity in invalidContractEndDateEntities)
                {
                    if(!records[invalidContractEndDateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ContractStartDate].Contains($"Invalid Contract Dates '{invalidContractEndDateEntity.ContractStartDate}' is equal to or later than '{invalidContractEndDateEntity.ContractEndDate}'"))
                    {
                        records[invalidContractEndDateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.ContractStartDate].Add($"Invalid Contract Dates '{invalidContractEndDateEntity.ContractStartDate}' is equal to or later than '{invalidContractEndDateEntity.ContractEndDate}'");
                    }
                }

                //Validate Rates
                var invalidRateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Value)
                    && fce.RateType.StartsWith("Rate")
                    && !methods.IsValidFixedContractRate(fce.Value));

                foreach(var invalidRateEntity in invalidRateEntities)
                {
                    if(!records[invalidRateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Rate Value '{invalidRateEntity.Value}' for '{invalidRateEntity.RateType}'"))
                    {
                        records[invalidRateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid Rate Value '{invalidRateEntity.Value}' for '{invalidRateEntity.RateType}'");
                    }
                }

                invalidRateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Value)
                    && fce.RateType.StartsWith("StandingCharge")
                    && !methods.IsValidFixedContractStandingCharge(fce.Value));

                foreach(var invalidRateEntity in invalidRateEntities)
                {
                    if(!records[invalidRateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Standing Charge Value '{invalidRateEntity.Value}'"))
                    {
                        records[invalidRateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid Standing Charge Value '{invalidRateEntity.Value}'");
                    }
                }

                invalidRateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Value)
                    && fce.RateType.StartsWith("CapacityCharge")
                    && !methods.IsValidFixedContractCapacityCharge(fce.Value));

                foreach(var invalidRateEntity in invalidRateEntities)
                {
                    if(!records[invalidRateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Capacity Charge Value '{invalidRateEntity.Value}'"))
                    {
                        records[invalidRateEntity.RowId.Value][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid Capacity Charge Value '{invalidRateEntity.Value}'");
                    }
                }

                //Validate Rate Count
                var invalidRateCountEntities = fixedContractEntities.Where(fce => string.IsNullOrWhiteSpace(fce.RateCount)
                    || !methods.IsValidFixedContractRateCount(fce.RateCount));

                foreach(var invalidRateCountEntity in invalidRateCountEntities)
                {
                    if(!records[invalidRateCountEntity.RowId.Value][_customerDataUploadValidationEntityEnums.RateCount].Contains($"Invalid Rate Count Value '{invalidRateCountEntity.RateCount}'"))
                    {
                        records[invalidRateCountEntity.RowId.Value][_customerDataUploadValidationEntityEnums.RateCount].Add($"Invalid Rate Count Value '{invalidRateCountEntity.RateCount}'");
                    }
                }

                var validRateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Value)
                    && fce.RateType.StartsWith("Rate")
                    && methods.IsValidFixedContractRate(fce.Value)
                    && methods.IsValidFixedContractRateCount(fce.RateCount));

                var contractMPXNDictionary = new Dictionary<string, List<string>>();
                foreach(var validRateEntity in validRateEntities)
                {
                    if(!contractMPXNDictionary.ContainsKey(validRateEntity.ContractReference))
                    {
                        contractMPXNDictionary.Add(validRateEntity.ContractReference, new List<string>());
                    }

                    if(!contractMPXNDictionary[validRateEntity.ContractReference].Contains(validRateEntity.MPXN))
                    {
                        contractMPXNDictionary[validRateEntity.ContractReference].Add(validRateEntity.MPXN);
                    }
                }

                foreach(var contract in contractMPXNDictionary)
                {
                    foreach(var mpxn in contract.Value)
                    {
                        var validFixedContractEntities = validRateEntities.Where(fce => fce.ContractReference == contract.Key && fce.MPXN == mpxn);
                        var rateCount = validFixedContractEntities.Select(fce => fce.RateCount).FirstOrDefault();

                        if(rateCount != validFixedContractEntities.Count().ToString())
                        {
                            if(!records[validFixedContractEntities.First().RowId.Value][_customerDataUploadValidationEntityEnums.RateCount].Contains($"Rate Count '{rateCount}' does not match number of rates provided"))
                            {
                                records[validFixedContractEntities.First().RowId.Value][_customerDataUploadValidationEntityEnums.RateCount].Add($"Rate Count '{rateCount}' does not match number of rates provided");
                            }
                        }
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.FixedContract);
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFixedContractDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFixedContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}