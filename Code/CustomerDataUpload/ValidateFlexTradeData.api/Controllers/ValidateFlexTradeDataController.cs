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

namespace ValidateFlexTradeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFlexTradeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateFlexTradeDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.Customer.DataUploadValidation.SheetName _customerDataUploadValidationSheetNameEnums = new Enums.Customer.DataUploadValidation.SheetName();
        private static readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 validateFlexTradeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateFlexTradeDataController(ILogger<ValidateFlexTradeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().ValidateFlexTradeDataAPI, password);
            validateFlexTradeDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateFlexTradeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFlexTradeData/Validate")]
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
                    validateFlexTradeDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateFlexTradeDataAPI, validateFlexTradeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateFlexTradeDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexTrade] table
                var flexTradeEntities = new Methods.Temp.CustomerDataUpload.FlexTrade().FlexTrade_GetByProcessQueueGUID(processQueueGUID);

                if(!flexTradeEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexTradeDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();

                var columns = new List<string>
                    {
                        _customerDataUploadValidationEntityEnums.BasketReference,
                        _customerDataUploadValidationEntityEnums.TradeReference,
                        _customerDataUploadValidationEntityEnums.TradeDate,
                        _customerDataUploadValidationEntityEnums.TradeProduct,
                        _customerDataUploadValidationEntityEnums.Volume,
                        _customerDataUploadValidationEntityEnums.Price,
                        _customerDataUploadValidationEntityEnums.Direction,
                    };

                var records = _tempCustomerDataUploadMethods.InitialiseRecordsDictionary(flexTradeEntities.Select(fte => fte.RowId).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.BasketReference, "Basket Reference"}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, flexTradeEntities, requiredColumns);

                //If Trade Reference is not populated, all other fields are required
                //Get Trade References not populated
                var emptyTradeReferenceDataRecords = flexTradeEntities.Where(fte => string.IsNullOrWhiteSpace(fte.TradeReference)).ToList();

                //Trade Date, Trade Product, Volume, Price and Direction must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {_customerDataUploadValidationEntityEnums.TradeDate, "Trade Date"},
                        {_customerDataUploadValidationEntityEnums.TradeProduct, "Trade Product"},
                        {_customerDataUploadValidationEntityEnums.Volume, _customerDataUploadValidationEntityEnums.Volume},
                        {_customerDataUploadValidationEntityEnums.Price, _customerDataUploadValidationEntityEnums.Price},
                        {_customerDataUploadValidationEntityEnums.Direction, "Trade Direction"}
                    };
                _tempCustomerDataUploadMethods.GetMissingRecords(records, emptyTradeReferenceDataRecords, requiredColumns);

                //Validate Trade Reference
                var invalidTradeReferenceDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.TradeReference)
                    && !methods.IsValidFlexTradeReference(fte.TradeReference));

                foreach(var invalidTradeReferenceDataRecord in invalidTradeReferenceDataRecords)
                {
                    if(!records[invalidTradeReferenceDataRecord.RowId][_customerDataUploadValidationEntityEnums.TradeReference].Contains($"Invalid Trade Reference '{invalidTradeReferenceDataRecord.TradeReference}'"))
                    {
                        records[invalidTradeReferenceDataRecord.RowId][_customerDataUploadValidationEntityEnums.TradeReference].Add($"Invalid Trade Reference '{invalidTradeReferenceDataRecord.TradeReference}'");
                    }
                }

                //Validate Trade Date
                var invalidTradeDateDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.TradeDate)
                    && !methods.IsValidDate(fte.TradeDate));

                foreach(var invalidTradeDateDataRecord in invalidTradeDateDataRecords)
                {
                    if(!records[invalidTradeDateDataRecord.RowId][_customerDataUploadValidationEntityEnums.TradeDate].Contains($"Invalid Trade Date '{invalidTradeDateDataRecord.TradeDate}'"))
                    {
                        records[invalidTradeDateDataRecord.RowId][_customerDataUploadValidationEntityEnums.TradeDate].Add($"Invalid Trade Date '{invalidTradeDateDataRecord.TradeDate}'");
                    }
                }

                //Validate Trade Product
                var invalidTradeProductDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.TradeProduct)
                    && !methods.IsValidFlexTradeProduct(fte.TradeProduct));

                foreach(var invalidTradeProductDataRecord in invalidTradeProductDataRecords)
                {
                    if(!records[invalidTradeProductDataRecord.RowId][_customerDataUploadValidationEntityEnums.TradeProduct].Contains($"Invalid Trade Product '{invalidTradeProductDataRecord.TradeProduct}'"))
                    {
                        records[invalidTradeProductDataRecord.RowId][_customerDataUploadValidationEntityEnums.TradeProduct].Add($"Invalid Trade Product '{invalidTradeProductDataRecord.TradeProduct}'");
                    }
                }

                //Validate Volume
                var invalidVolumeDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.Volume)
                    && !methods.IsValidFlexTradeVolume(fte.Volume));

                foreach(var invalidVolumeDataRecord in invalidVolumeDataRecords)
                {
                    if(!records[invalidVolumeDataRecord.RowId][_customerDataUploadValidationEntityEnums.Volume].Contains($"Invalid Trade Volume '{invalidVolumeDataRecord.Volume}'"))
                    {
                        records[invalidVolumeDataRecord.RowId][_customerDataUploadValidationEntityEnums.Volume].Add($"Invalid Trade Volume '{invalidVolumeDataRecord.Volume}'");
                    }
                }

                //Validate Price
                var invalidPriceDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.Price)
                    && !methods.IsValidFlexTradePrice(fte.Price));

                foreach(var invalidPriceDataRecord in invalidPriceDataRecords)
                {
                    if(!records[invalidPriceDataRecord.RowId][_customerDataUploadValidationEntityEnums.Price].Contains($"Invalid Trade Price '{invalidPriceDataRecord.Price}'"))
                    {
                        records[invalidPriceDataRecord.RowId][_customerDataUploadValidationEntityEnums.Price].Add($"Invalid Trade Price '{invalidPriceDataRecord.Price}'");
                    }
                }

                //Validate Direction
                var invalidDirectionDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.Direction)
                    && !methods.IsValidFlexTradeDirection(fte.Direction));

                foreach(var invalidDirectionDataRecord in invalidDirectionDataRecords)
                {
                    if(!records[invalidDirectionDataRecord.RowId][_customerDataUploadValidationEntityEnums.Direction].Contains($"Invalid Trade Direction '{invalidDirectionDataRecord.Direction}'"))
                    {
                        records[invalidDirectionDataRecord.RowId][_customerDataUploadValidationEntityEnums.Direction].Add($"Invalid Trade Direction '{invalidDirectionDataRecord.Direction}'");
                    }
                }

                //Update Process Queue
                var errorMessage = _tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, _customerDataUploadValidationSheetNameEnums.FlexTrade);
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexTradeDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}