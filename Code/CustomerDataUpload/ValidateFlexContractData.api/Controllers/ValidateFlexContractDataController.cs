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

namespace ValidateFlexContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFlexContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateFlexContractDataController> _logger;
        private readonly Int64 validateFlexContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateFlexContractDataController(ILogger<ValidateFlexContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateFlexContractDataAPI, password);
            validateFlexContractDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateFlexContractDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(validateFlexContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFlexContractData/Validate")]
        public void Validate([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

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
                    validateFlexContractDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateFlexContractDataAPI, validateFlexContractDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateFlexContractDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexContract] table
                var flexContractEntities = new Methods.Temp.CustomerDataUpload.FlexContract().FlexContract_GetByProcessQueueGUID(processQueueGUID);

                if(!flexContractEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexContractDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
                var customerMethods = new Methods.Customer();

                var columns = new List<string>
                    {
                        customerDataUploadValidationEntityEnums.ContractReference,
                        customerDataUploadValidationEntityEnums.BasketReference,
                        customerDataUploadValidationEntityEnums.MPXN,
                        customerDataUploadValidationEntityEnums.Supplier,
                        customerDataUploadValidationEntityEnums.ContractStartDate,
                        customerDataUploadValidationEntityEnums.ContractEndDate,
                        customerDataUploadValidationEntityEnums.Product,
                        customerDataUploadValidationEntityEnums.RateType,
                        customerDataUploadValidationEntityEnums.Value,
                    };

                var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(flexContractEntities.Select(fce => fce.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.ContractReference, "Contract Reference"},
                        {customerDataUploadValidationEntityEnums.BasketReference, "Basket Reference"},
                        {customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {customerDataUploadValidationEntityEnums.Supplier, customerDataUploadValidationEntityEnums.Supplier},
                        {customerDataUploadValidationEntityEnums.ContractStartDate, "Contract Start Date"},
                        {customerDataUploadValidationEntityEnums.ContractEndDate, "Contract End Date"}
                    };

                tempCustomerDataUploadMethods.GetMissingRecords(records, flexContractEntities, requiredColumns);

                //If Contract Reference, Basket Reference and MPXN doesn't exist then Product is required
                //Get new contracts
                var newContractMeterDataRecords = flexContractEntities.Where(fce => !customerMethods.ContractBasketMeterExists(fce.ContractReference, fce.BasketReference, fce.MPXN)).ToList();

                //Product must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.Product, customerDataUploadValidationEntityEnums.Product}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, newContractMeterDataRecords, requiredColumns);

                //Validate Supplier
                var invalidSupplierDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Supplier)
                    && !methods.IsValidSupplier(fce.Supplier));

                foreach(var invalidSupplierDataRecord in invalidSupplierDataRecords)
                {
                    if(!records[invalidSupplierDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Supplier].Contains($"Invalid Supplier '{invalidSupplierDataRecord.Supplier}'"))
                    {
                        records[invalidSupplierDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Supplier].Add($"Invalid Supplier '{invalidSupplierDataRecord.Supplier}'");
                    }
                }

                //Validate Contract Dates
                var invalidContractStartDateDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractStartDate)
                    && !methods.IsValidDate(fce.ContractStartDate));

                foreach(var invalidContractStartDateDataRecord in invalidContractStartDateDataRecords)
                {
                    if(!records[invalidContractStartDateDataRecord.RowId.Value]["TradeRContractStartDateference"].Contains($"Invalid Contract Start Date '{invalidContractStartDateDataRecord.ContractStartDate}'"))
                    {
                        records[invalidContractStartDateDataRecord.RowId.Value]["TradeRContractStartDateference"].Add($"Invalid Contract Start Date '{invalidContractStartDateDataRecord.ContractStartDate}'");
                    }
                }

                var invalidContractEndDateDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractEndDate)
                    && !methods.IsValidDate(fce.ContractEndDate));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    if(!records[invalidContractEndDateDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.ContractEndDate].Contains($"Invalid Contract End Date '{invalidContractEndDateDataRecord.ContractEndDate}'"))
                    {
                        records[invalidContractEndDateDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.ContractEndDate].Add($"Invalid Contract End Date '{invalidContractEndDateDataRecord.ContractEndDate}'");
                    }
                }

                var invalidContractDateDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractStartDate)
                    && !string.IsNullOrWhiteSpace(fce.ContractEndDate)
                    && methods.IsValidDate(fce.ContractStartDate)
                    && methods.IsValidDate(fce.ContractEndDate)
                    && Convert.ToDateTime(fce.ContractStartDate) >= Convert.ToDateTime(fce.ContractEndDate));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    if(!records[invalidContractEndDateDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.ContractStartDate].Contains($"Invalid Contract Dates '{invalidContractEndDateDataRecord.ContractStartDate}' is equal to or later than '{invalidContractEndDateDataRecord.ContractEndDate}'"))
                    {
                        records[invalidContractEndDateDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.ContractStartDate].Add($"Invalid Contract Dates '{invalidContractEndDateDataRecord.ContractStartDate}' is equal to or later than '{invalidContractEndDateDataRecord.ContractEndDate}'");
                    }
                }

                //Validate Rates
                var invalidRateDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Value)
                    && fce.RateType.StartsWith("Rate")
                    && !methods.IsValidFlexContractRate(fce.Value));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    if(!records[invalidRateDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Rate Value '{invalidRateDataRecord.Value}' for '{invalidRateDataRecord.RateType}'"))
                    {
                        records[invalidRateDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Value].Add($"Invalid Rate Value '{invalidRateDataRecord.Value}' for '{invalidRateDataRecord.RateType}'");
                    }
                }

                invalidRateDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Value)
                    && fce.RateType.StartsWith("StandingCharge")
                    && !methods.IsValidFixedContractStandingCharge(fce.Value));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    if(!records[invalidRateDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Standing Charge Value '{invalidRateDataRecord.Value}'"))
                    {
                        records[invalidRateDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Value].Add($"Invalid Standing Charge Value '{invalidRateDataRecord.Value}'");
                    }
                }

                //Update Process Queue
                var errorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, new Enums.CustomerSchema.DataUploadValidation.SheetName().FlexContract);
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexContractDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}