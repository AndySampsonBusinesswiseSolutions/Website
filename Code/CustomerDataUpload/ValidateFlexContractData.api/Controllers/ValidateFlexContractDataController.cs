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
        private readonly ILogger<ValidateFlexContractDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 validateFlexContractDataAPIId;
        private readonly string hostEnvironment;

        public ValidateFlexContractDataController(ILogger<ValidateFlexContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.ValidateFlexContractDataAPI, password);
            validateFlexContractDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateFlexContractDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateFlexContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateFlexContractDataAPI, validateFlexContractDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateFlexContractDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexContract] table
                var flexContractDataRows = _tempCustomerDataUploadMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);

                if(!flexContractDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexContractDataAPIId, false, null);
                    return;
                }

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

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(flexContractDataRows.Select(d => Convert.ToInt32(d["RowId"].ToString())).Distinct().ToList(), columns);

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

                _tempCustomerDataUploadMethods.GetMissingRecords(records, flexContractDataRows, requiredColumns);

                //If Contract Reference, Basket Reference and MPXN doesn't exist then Product is required
                //Get new contracts
                var newContractMeterDataRecords = flexContractDataRows.Where(r => 
                    !_customerMethods.ContractBasketMeterExists(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference), r.Field<string>(_customerDataUploadValidationEntityEnums.BasketReference), r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)))
                    .ToList();

                //Product must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.Product, _customerDataUploadValidationEntityEnums.Product}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, newContractMeterDataRecords, requiredColumns);

                //Validate Supplier
                var invalidSupplierDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.Supplier))
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
                var invalidContractStartDateDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractStartDate))
                    && !_methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractStartDate)));

                foreach(var invalidContractStartDateDataRecord in invalidContractStartDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidContractStartDateDataRecord["RowId"]);
                    if(!records[rowId]["TradeRContractStartDateference"].Contains($"Invalid Contract Start Date '{invalidContractStartDateDataRecord[_customerDataUploadValidationEntityEnums.ContractStartDate]}'"))
                    {
                        records[rowId]["TradeRContractStartDateference"].Add($"Invalid Contract Start Date '{invalidContractStartDateDataRecord[_customerDataUploadValidationEntityEnums.ContractStartDate]}'");
                    }
                }

                var invalidContractEndDateDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractEndDate))
                    && !_methods.IsValidDate(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractEndDate)));

                foreach(var invalidContractEndDateDataRecord in invalidContractEndDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidContractEndDateDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.ContractEndDate].Contains($"Invalid Contract End Date '{invalidContractEndDateDataRecord[_customerDataUploadValidationEntityEnums.ContractEndDate]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.ContractEndDate].Add($"Invalid Contract End Date '{invalidContractEndDateDataRecord[_customerDataUploadValidationEntityEnums.ContractEndDate]}'");
                    }
                }

                var invalidContractDateDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.ContractStartDate))
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
                var invalidRateDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.Value))
                    && r.Field<string>(_customerDataUploadValidationEntityEnums.RateType).StartsWith("Rate")
                    && !_methods.IsValidFlexContractRate(r.Field<string>(_customerDataUploadValidationEntityEnums.Value)));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Rate Value '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.Value]}' for '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.RateType]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid Rate Value '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.Value]}' for '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.RateType]}'");
                    }
                }

                invalidRateDataRecords = flexContractDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>(_customerDataUploadValidationEntityEnums.Value))
                    && r.Field<string>(_customerDataUploadValidationEntityEnums.RateType).StartsWith("StandingCharge")
                    && !_methods.IsValidFixedContractStandingCharge(r.Field<string>(_customerDataUploadValidationEntityEnums.Value)));

                foreach(var invalidRateDataRecord in invalidRateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidRateDataRecord["RowId"]);
                    if(!records[rowId][_customerDataUploadValidationEntityEnums.Value].Contains($"Invalid Standing Charge Value '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.Value]}'"))
                    {
                        records[rowId][_customerDataUploadValidationEntityEnums.Value].Add($"Invalid Standing Charge Value '{invalidRateDataRecord[_customerDataUploadValidationEntityEnums.Value]}'");
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