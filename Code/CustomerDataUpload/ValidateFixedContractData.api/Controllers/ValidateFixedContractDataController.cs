using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

namespace ValidateFixedContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFixedContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateFixedContractDataController> _logger;
        private readonly Int64 validateFixedContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateFixedContractDataController(ILogger<ValidateFixedContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateFixedContractDataAPI, password);
            validateFixedContractDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateFixedContractDataAPI);
        }

        [HttpPost]
        [Route("ValidateFixedContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateFixedContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFixedContractData/Validate")]
        public void Validate([FromBody] object data)
        {
            var systemMethods = new Methods.SystemSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    validateFixedContractDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateFixedContractDataAPI, validateFixedContractDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateFixedContractDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FixedContract] table
                var fixedContractEntities = new Methods.TempSchema.CustomerDataUpload.FixedContract().FixedContract_GetByProcessQueueGUID(processQueueGUID);

                if(!fixedContractEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFixedContractDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.TempSchema.CustomerDataUpload();
                var customerMethods = new Methods.CustomerSchema();

                var columns = new List<string>
                    {
                        customerDataUploadValidationEntityEnums.ContractReference,
                        customerDataUploadValidationEntityEnums.MPXN,
                        customerDataUploadValidationEntityEnums.Supplier,
                        customerDataUploadValidationEntityEnums.ContractStartDate,
                        customerDataUploadValidationEntityEnums.ContractEndDate,
                        customerDataUploadValidationEntityEnums.Product,
                        customerDataUploadValidationEntityEnums.RateCount,
                        customerDataUploadValidationEntityEnums.RateType,
                        customerDataUploadValidationEntityEnums.Value,
                    };

                var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(fixedContractEntities.Select(fce => fce.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.ContractReference, "Contract Reference"},
                        {customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {customerDataUploadValidationEntityEnums.Supplier, customerDataUploadValidationEntityEnums.Supplier},
                        {customerDataUploadValidationEntityEnums.ContractStartDate, "Contract Start Date"},
                        {customerDataUploadValidationEntityEnums.ContractEndDate, "Contract End Date"}
                    };

                tempCustomerDataUploadMethods.GetMissingRecords(records, fixedContractEntities, requiredColumns);

                //If Contract Reference and MPXN doesn't exist then Product, Rate Count, Number of rates and costs are required
                var newContractMeterEntities = fixedContractEntities.Where(fce => !customerMethods.ContractMeterExists(fce.ContractReference, fce.MPXN)).ToList();

                requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.Product, customerDataUploadValidationEntityEnums.Product},
                        {customerDataUploadValidationEntityEnums.RateCount, "Rate Count"},
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, newContractMeterEntities, requiredColumns);

                //Get new contracts
                var newContractEntities = fixedContractEntities.Where(fce => string.IsNullOrWhiteSpace(fce.ContractReference)).ToList();

                //Product must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.Product, customerDataUploadValidationEntityEnums.Product},
                        {customerDataUploadValidationEntityEnums.RateCount, "Rate Count"}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, newContractEntities, requiredColumns);

                //Validate Supplier
                var invalidSupplierEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Supplier)
                    && !methods.IsValidSupplier(fce.Supplier));

                foreach(var invalidSupplierEntity in invalidSupplierEntities)
                {
                    if(!records[invalidSupplierEntity.RowId.Value][customerDataUploadValidationEntityEnums.Supplier].Contains($"Invalid Supplier '{invalidSupplierEntity.Supplier}'"))
                    {
                        records[invalidSupplierEntity.RowId.Value][customerDataUploadValidationEntityEnums.Supplier].Add($"Invalid Supplier '{invalidSupplierEntity.Supplier}'");
                    }
                }

                //Validate Contract Dates
                var invalidContractStartDateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractStartDate)
                    && !methods.IsValidDate(fce.ContractStartDate));

                foreach(var invalidContractStartDateEntity in invalidContractStartDateEntities)
                {
                    if(!records[invalidContractStartDateEntity.RowId.Value][customerDataUploadValidationEntityEnums.ContractStartDate].Contains($"Invalid Contract Start Date '{invalidContractStartDateEntity.ContractStartDate}'"))
                    {
                        records[invalidContractStartDateEntity.RowId.Value][customerDataUploadValidationEntityEnums.ContractStartDate].Add($"Invalid Contract Start Date '{invalidContractStartDateEntity.ContractStartDate}'");
                    }
                }

                var invalidContractEndDateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractEndDate)
                    && !methods.IsValidDate(fce.ContractEndDate));

                foreach(var invalidContractEndDateEntity in invalidContractEndDateEntities)
                {
                    if(!records[invalidContractEndDateEntity.RowId.Value][customerDataUploadValidationEntityEnums.ContractEndDate].Contains($"Invalid Contract End Date '{invalidContractEndDateEntity.ContractEndDate}'"))
                    {
                        records[invalidContractEndDateEntity.RowId.Value][customerDataUploadValidationEntityEnums.ContractEndDate].Add($"Invalid Contract End Date '{invalidContractEndDateEntity.ContractEndDate}'");
                    }
                }

                var invalidContractDateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractStartDate)
                    && !string.IsNullOrWhiteSpace(fce.ContractEndDate) && methods.IsValidDate(fce.ContractStartDate)
                    && methods.IsValidDate(fce.ContractEndDate) && Convert.ToDateTime(fce.ContractStartDate) >= Convert.ToDateTime(fce.ContractEndDate));

                foreach(var invalidContractEndDateEntity in invalidContractEndDateEntities)
                {
                    if(!records[invalidContractEndDateEntity.RowId.Value][customerDataUploadValidationEntityEnums.ContractStartDate].Contains($"Invalid Contract Dates '{invalidContractEndDateEntity.ContractStartDate}' is equal to or later than '{invalidContractEndDateEntity.ContractEndDate}'"))
                    {
                        records[invalidContractEndDateEntity.RowId.Value][customerDataUploadValidationEntityEnums.ContractStartDate].Add($"Invalid Contract Dates '{invalidContractEndDateEntity.ContractStartDate}' is equal to or later than '{invalidContractEndDateEntity.ContractEndDate}'");
                    }
                }

                //Validate Rates
                var invalidRateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Value)
                    && fce.RateType.StartsWith("Rate")
                    && !methods.IsValidFixedContractRate(fce.Value));

                foreach(var invalidRateEntity in invalidRateEntities)
                {
                    if(!records[invalidRateEntity.RowId.Value][customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Rate Value '{invalidRateEntity.Value}' for '{invalidRateEntity.RateType}'"))
                    {
                        records[invalidRateEntity.RowId.Value][customerDataUploadValidationEntityEnums.Value].Add($"Invalid Rate Value '{invalidRateEntity.Value}' for '{invalidRateEntity.RateType}'");
                    }
                }

                invalidRateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Value)
                    && fce.RateType.StartsWith("StandingCharge")
                    && !methods.IsValidFixedContractStandingCharge(fce.Value));

                foreach(var invalidRateEntity in invalidRateEntities)
                {
                    if(!records[invalidRateEntity.RowId.Value][customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Standing Charge Value '{invalidRateEntity.Value}'"))
                    {
                        records[invalidRateEntity.RowId.Value][customerDataUploadValidationEntityEnums.Value].Add($"Invalid Standing Charge Value '{invalidRateEntity.Value}'");
                    }
                }

                invalidRateEntities = fixedContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Value)
                    && fce.RateType.StartsWith("CapacityCharge")
                    && !methods.IsValidFixedContractCapacityCharge(fce.Value));

                foreach(var invalidRateEntity in invalidRateEntities)
                {
                    if(!records[invalidRateEntity.RowId.Value][customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Capacity Charge Value '{invalidRateEntity.Value}'"))
                    {
                        records[invalidRateEntity.RowId.Value][customerDataUploadValidationEntityEnums.Value].Add($"Invalid Capacity Charge Value '{invalidRateEntity.Value}'");
                    }
                }

                //Validate Rate Count
                var invalidRateCountEntities = fixedContractEntities.Where(fce => string.IsNullOrWhiteSpace(fce.RateCount)
                    || !methods.IsValidFixedContractRateCount(fce.RateCount));

                foreach(var invalidRateCountEntity in invalidRateCountEntities)
                {
                    if(!records[invalidRateCountEntity.RowId.Value][customerDataUploadValidationEntityEnums.RateCount].Contains($"Invalid Rate Count Value '{invalidRateCountEntity.RateCount}'"))
                    {
                        records[invalidRateCountEntity.RowId.Value][customerDataUploadValidationEntityEnums.RateCount].Add($"Invalid Rate Count Value '{invalidRateCountEntity.RateCount}'");
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
                            if(!records[validFixedContractEntities.First().RowId.Value][customerDataUploadValidationEntityEnums.RateCount].Contains($"Rate Count '{rateCount}' does not match number of rates provided"))
                            {
                                records[validFixedContractEntities.First().RowId.Value][customerDataUploadValidationEntityEnums.RateCount].Add($"Rate Count '{rateCount}' does not match number of rates provided");
                            }
                        }
                    }
                }

                //Update Process Queue
                var errorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, new Enums.CustomerSchema.DataUploadValidation.SheetName().FixedContract);
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFixedContractDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFixedContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}