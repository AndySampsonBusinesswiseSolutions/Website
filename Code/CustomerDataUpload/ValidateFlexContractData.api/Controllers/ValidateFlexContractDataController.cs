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

namespace ValidateFlexContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFlexContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateFlexContractDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 validateFlexContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateFlexContractDataController(ILogger<ValidateFlexContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().ValidateFlexContractDataAPI, password);
            validateFlexContractDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateFlexContractDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateFlexContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFlexContractData/Validate")]
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
                    validateFlexContractDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateFlexContractDataAPI, validateFlexContractDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateFlexContractDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexContract] table
                var flexContractEntities = new Methods.Temp.CustomerDataUpload.FlexContract().FlexContract_GetByProcessQueueGUID(processQueueGUID);

                if(!flexContractEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexContractDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();

                var columns = new List<string>
                    {
                        _customerDataUploadValidationEntityEnums.ContractReference,
                        _customerDataUploadValidationEntityEnums.BasketReference,
                        _customerDataUploadValidationEntityEnums.MPXN,
                        _customerDataUploadValidationEntityEnums.Supplier,
                        _customerDataUploadValidationEntityEnums.ContractStartDate,
                        _customerDataUploadValidationEntityEnums.ContractEndDate,
                        _customerDataUploadValidationEntityEnums.Product,
                        _customerDataUploadValidationEntityEnums.RateType,
                        _customerDataUploadValidationEntityEnums.Value,
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(flexContractEntities.Select(fce => fce.RowId).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.ContractReference, "Contract Reference"},
                        {_customerDataUploadValidationEntityEnums.BasketReference, "Basket Reference"},
                        {_customerDataUploadValidationEntityEnums.MPXN, "MPAN/MPRN"},
                        {_customerDataUploadValidationEntityEnums.Supplier, _customerDataUploadValidationEntityEnums.Supplier},
                        {_customerDataUploadValidationEntityEnums.ContractStartDate, "Contract Start Date"},
                        {_customerDataUploadValidationEntityEnums.ContractEndDate, "Contract End Date"}
                    };

                _tempCustomerDataUploadMethods.GetMissingRecords(records, flexContractEntities, requiredColumns);

                //If Contract Reference, Basket Reference and MPXN doesn't exist then Product is required
                //Get new contracts
                var newContractMeterDataRecords = flexContractEntities.Where(fce => 
                    !_customerMethods.ContractBasketMeterExists(fce.ContractReference, fce.BasketReference, fce.MPXN))
                    .ToList();

                //Product must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.Product, _customerDataUploadValidationEntityEnums.Product}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, newContractMeterDataRecords, requiredColumns);

                //Validate Supplier
                var invalidSupplierDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Supplier)
                    && !methods.IsValidSupplier(fce.Supplier));

                foreach(var invalidSupplierDataRecord in invalidSupplierDataRecords)
                {
                    if(!records[invalidSupplierDataRecord.RowId][_customerDataUploadValidationEntityEnums.Supplier].Contains($"Invalid Supplier '{invalidSupplierDataRecord.Supplier}'"))
                    {
                        records[invalidSupplierDataRecord.RowId][_customerDataUploadValidationEntityEnums.Supplier].Add($"Invalid Supplier '{invalidSupplierDataRecord.Supplier}'");
                    }
                }

                //Validate Contract Dates
                var invalidContractStartDateDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractStartDate)
                    && !methods.IsValidDate(fce.ContractStartDate));

                foreach(var invalidContractStartDateDataRecord in invalidContractStartDateDataRecords)
                {
                    if(!records[invalidContractStartDateDataRecord.RowId]["TradeRContractStartDateference"].Contains($"Invalid Contract Start Date '{invalidContractStartDateDataRecord.ContractStartDate}'"))
                    {
                        records[invalidContractStartDateDataRecord.RowId]["TradeRContractStartDateference"].Add($"Invalid Contract Start Date '{invalidContractStartDateDataRecord.ContractStartDate}'");
                    }
                }

                var invalidContractEndDateDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractEndDate)
                    && !methods.IsValidDate(fce.ContractEndDate));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    if(!records[invalidContractEndDateDataRecord.RowId][_customerDataUploadValidationEntityEnums.ContractEndDate].Contains($"Invalid Contract End Date '{invalidContractEndDateDataRecord.ContractEndDate}'"))
                    {
                        records[invalidContractEndDateDataRecord.RowId][_customerDataUploadValidationEntityEnums.ContractEndDate].Add($"Invalid Contract End Date '{invalidContractEndDateDataRecord.ContractEndDate}'");
                    }
                }

                var invalidContractDateDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.ContractStartDate)
                    && !string.IsNullOrWhiteSpace(fce.ContractEndDate)
                    && methods.IsValidDate(fce.ContractStartDate)
                    && methods.IsValidDate(fce.ContractEndDate)
                    && Convert.ToDateTime(fce.ContractStartDate) >= Convert.ToDateTime(fce.ContractEndDate));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    if(!records[invalidContractEndDateDataRecord.RowId][_customerDataUploadValidationEntityEnums.ContractStartDate].Contains($"Invalid Contract Dates '{invalidContractEndDateDataRecord.ContractStartDate}' is equal to or later than '{invalidContractEndDateDataRecord.ContractEndDate}'"))
                    {
                        records[invalidContractEndDateDataRecord.RowId][_customerDataUploadValidationEntityEnums.ContractStartDate].Add($"Invalid Contract Dates '{invalidContractEndDateDataRecord.ContractStartDate}' is equal to or later than '{invalidContractEndDateDataRecord.ContractEndDate}'");
                    }
                }

                //Validate Rates
                var invalidRateDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Value)
                    && fce.RateType.StartsWith("Rate")
                    && !methods.IsValidFlexContractRate(fce.Value));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    if(!records[invalidRateDataRecord.RowId][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Rate Value '{invalidRateDataRecord.Value}' for '{invalidRateDataRecord.RateType}'"))
                    {
                        records[invalidRateDataRecord.RowId][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid Rate Value '{invalidRateDataRecord.Value}' for '{invalidRateDataRecord.RateType}'");
                    }
                }

                invalidRateDataRecords = flexContractEntities.Where(fce => !string.IsNullOrWhiteSpace(fce.Value)
                    && fce.RateType.StartsWith("StandingCharge")
                    && !methods.IsValidFixedContractStandingCharge(fce.Value));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    if(!records[invalidRateDataRecord.RowId][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Standing Charge Value '{invalidRateDataRecord.Value}'"))
                    {
                        records[invalidRateDataRecord.RowId][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid Standing Charge Value '{invalidRateDataRecord.Value}'");
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.FlexContract);
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexContractDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}