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

namespace ValidateFlexContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFlexContractDataController : ControllerBase
    {
        private readonly ILogger<ValidateFlexContractDataController> _logger;
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
        private readonly Int64 validateFlexContractDataAPIId;

        public ValidateFlexContractDataController(ILogger<ValidateFlexContractDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateFlexContractDataAPI, _systemAPIPasswordEnums.ValidateFlexContractDataAPI);
            validateFlexContractDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateFlexContractDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateFlexContractDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFlexContractData/Validate")]
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
                    validateFlexContractDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateFlexContractDataAPI, validateFlexContractDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[FlexContract] table
                var flexContractDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);

                if(!flexContractDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateFlexContractDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"ContractReference", "Contract Reference"},
                        {"BasketReference", "Basket Reference"},
                        {"MPXN", "MPAN/MPRN"},
                        {"Supplier", "Supplier"},
                        {"ContractStartDate", "Contract Start Date"},
                        {"ContractEndDate", "Contract End Date"}
                    };

                var records = _tempCustomerMethods.GetMissingRecords(flexContractDataRows, requiredColumns);

                //If Contract Reference, Basket Reference and MPXN doesn't exist then Product is required
                //Get new contracts
                var newContractMeterDataRecords = flexContractDataRows.Where(r => 
                    !_customerMethods.ContractBasketMeterExists(r.Field<string>("ContractReference"), r.Field<string>("BasketReference"), r.Field<string>("MPXN")));

                //Product must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {"Product", "Product"}
                    };
                var newContractErrors =_tempCustomerMethods.GetMissingRecords(newContractMeterDataRecords, requiredColumns);
                _tempCustomerMethods.AddErrorsToRecords(records, newContractErrors);

                //Validate Supplier
                var invalidSupplierDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Supplier"))
                    && !_methods.IsValidSupplier(r.Field<string>("Supplier")));

                foreach(var invalidSupplierDataRecord in invalidSupplierDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidSupplierDataRecord["RowId"]);
                    records[rowId]["Supplier"].Add($"Invalid Supplier '{invalidSupplierDataRecord["Supplier"]}'");
                }

                //Validate Contract Dates
                var invalidContractStartDateDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContractStartDate"))
                    && !_methods.IsValidDate(r.Field<string>("ContractStartDate")));

                foreach(var invalidContractStartDateDataRecord in invalidContractStartDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidContractStartDateDataRecord["RowId"]);
                    records[rowId]["TradeRContractStartDateference"].Add($"Invalid Contract Start Date '{invalidContractStartDateDataRecord["ContractStartDate"]}'");
                }

                var invalidContractEndDateDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContractEndDate"))
                    && !_methods.IsValidDate(r.Field<string>("ContractEndDate")));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidContractEndDateDataRecord["RowId"]);
                    records[rowId]["ContractEndDate"].Add($"Invalid Contract End Date '{invalidContractEndDateDataRecord["ContractEndDate"]}'");
                }

                var invalidContractDateDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("ContractStartDate"))
                    && !string.IsNullOrWhiteSpace(r.Field<string>("ContractEndDate"))
                    && _methods.IsValidDate(r.Field<string>("ContractStartDate"))
                    && _methods.IsValidDate(r.Field<string>("ContractEndDate"))
                    && r.Field<DateTime>("ContractStartDate") >= r.Field<DateTime>("ContractEndDate"));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidContractEndDateDataRecord["RowId"]);
                    records[rowId]["ContractStartDate"].Add($"Invalid Contract Dates '{invalidContractEndDateDataRecord["ContractStartDate"]}' is equal to or later than '{invalidContractEndDateDataRecord["ContractEndDate"]}'");
                }

                //Validate Rates
                var invalidRateDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Value"))
                    && !_methods.IsValidFixedContractRate(r.Field<string>("Value")));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateDataRecord["RowId"]);
                    records[rowId]["Value"].Add($"Invalid Rate Value '{invalidRateDataRecord["Value"]}'");
                }

                invalidRateDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("StandingCharge"))
                    && !_methods.IsValidFixedContactStandingCharge(r.Field<string>("StandingCharge")));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateDataRecord["RowId"]);
                    records[rowId]["StandingCharge"].Add($"Invalid Standing Charge Value '{invalidRateDataRecord["StandingCharge"]}'");
                }

                //Update Process Queue
                var errorMessage = _tempCustomerMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.FlexContract);
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateFlexContractDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateFlexContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

