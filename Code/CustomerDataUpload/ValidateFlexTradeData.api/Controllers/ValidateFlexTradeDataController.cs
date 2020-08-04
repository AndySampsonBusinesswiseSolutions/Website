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

namespace ValidateFlexTradeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFlexTradeDataController : ControllerBase
    {
        private readonly ILogger<ValidateFlexTradeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private readonly Int64 validateFlexTradeDataAPIId;

        public ValidateFlexTradeDataController(ILogger<ValidateFlexTradeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateFlexTradeDataAPI, _systemAPIPasswordEnums.ValidateFlexTradeDataAPI);
            validateFlexTradeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateFlexTradeDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFlexTradeData/Validate")]
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
                    validateFlexTradeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateFlexTradeDataAPI, validateFlexTradeDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[FlexTrade] table
                var customerDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateFlexTradeDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"BasketReference", "Basket Reference"}
                    };

                var records = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns);

                //If Trade Reference is not populated, all other fields are required
                //Get Trade References not populated
                var emptyTradeReferenceDataRecords = customerDataRows.Where(r => string.IsNullOrWhiteSpace(r.Field<string>("TradeReference")));

                //Trade Date, Trade Product, Volume, Price and Direction must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {"TradeDate", "Trade Date"},
                        {"TradeProduct", "Trade Product"},
                        {"Volume", "Volume"},
                        {"Price", "Price"},
                        {"Direction (B/S)", "Trade Direction"}
                    };
                var emptyTradeReferenceErrors =_tempCustomerMethods.GetMissingRecords(emptyTradeReferenceDataRecords, requiredColumns);
                _tempCustomerMethods.AddErrorsToRecords(records, emptyTradeReferenceErrors);

                //Validate Trade Reference
                var invalidTradeReferenceDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("TradeReference"))
                    && !_methods.IsValidFlexTradeReference(r.Field<string>("TradeReference")));

                foreach(var invalidTradeReferenceDataRecord in invalidTradeReferenceDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidTradeReferenceDataRecord["RowId"]);
                    records[rowId]["TradeReference"].Add($"Invalid Trade Reference '{invalidTradeReferenceDataRecord["TradeReference"]}'");
                }

                //Validate Trade Date
                var invalidTradeDateDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("TradeDate"))
                    && !_methods.IsValidDate(r.Field<string>("TradeDate")));

                foreach(var invalidTradeDateDataRecord in invalidTradeDateDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidTradeDateDataRecord["RowId"]);
                    records[rowId]["TradeDate"].Add($"Invalid Trade Date '{invalidTradeDateDataRecord["TradeDate"]}'");
                }

                //Validate Trade Product
                var invalidTradeProductDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("TradeProduct"))
                    && !_methods.IsValidFlexTradeProduct(r.Field<string>("TradeProduct")));

                foreach(var invalidTradeProductDataRecord in invalidTradeProductDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidTradeProductDataRecord["RowId"]);
                    records[rowId]["TradeProduct"].Add($"Invalid Trade Product '{invalidTradeProductDataRecord["TradeProduct"]}'");
                }

                //Validate Volume
                var invalidVolumeDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Volume"))
                    && !_methods.IsValidFlexTradeVolume(r.Field<string>("Volume")));

                foreach(var invalidVolumeDataRecord in invalidVolumeDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidVolumeDataRecord["RowId"]);
                    records[rowId]["Volume"].Add($"Invalid Trade Volume '{invalidVolumeDataRecord["Volume"]}'");
                }

                //Validate Price
                var invalidPriceDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Price"))
                    && !_methods.IsValidFlexTradePrice(r.Field<string>("Price")));

                foreach(var invalidPriceDataRecord in invalidPriceDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidPriceDataRecord["RowId"]);
                    records[rowId]["Price"].Add($"Invalid Trade Price '{invalidPriceDataRecord["Price"]}'");
                }

                //Validate Direction
                var invalidDirectionDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Direction"))
                    && !_methods.IsValidFlexTradeDirection(r.Field<string>("Direction")));

                foreach(var invalidDirectionDataRecord in invalidDirectionDataRecords)
                {
                    var rowId = Convert.ToInt32(invalidDirectionDataRecord["RowId"]);
                    records[rowId]["Direction"].Add($"Invalid Trade Direction '{invalidDirectionDataRecord["Direction"]}'");
                }

                //Update Process Queue
                var errorMessage = _tempCustomerMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.FlexTrade);
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateFlexTradeDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

