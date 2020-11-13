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
        private readonly Int64 validateFlexTradeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateFlexTradeDataController(ILogger<ValidateFlexTradeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateFlexTradeDataAPI, password);
            validateFlexTradeDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(validateFlexTradeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFlexTradeData/Validate")]
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
                    validateFlexTradeDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateFlexTradeDataAPI, validateFlexTradeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateFlexTradeDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexTrade] table
                var flexTradeEntities = new Methods.Temp.CustomerDataUpload.FlexTrade().FlexTrade_GetByProcessQueueGUID(processQueueGUID);

                if(!flexTradeEntities.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexTradeDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();

                var columns = new List<string>
                    {
                        customerDataUploadValidationEntityEnums.BasketReference,
                        customerDataUploadValidationEntityEnums.TradeReference,
                        customerDataUploadValidationEntityEnums.TradeDate,
                        customerDataUploadValidationEntityEnums.TradeProduct,
                        customerDataUploadValidationEntityEnums.Volume,
                        customerDataUploadValidationEntityEnums.Price,
                        customerDataUploadValidationEntityEnums.Direction,
                    };

                var records = tempCustomerDataUploadMethods.InitialiseRecordsDictionary(flexTradeEntities.Select(fte => fte.RowId.Value).Distinct().ToList(), columns);

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.BasketReference, "Basket Reference"}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, flexTradeEntities, requiredColumns);

                //If Trade Reference is not populated, all other fields are required
                //Get Trade References not populated
                var emptyTradeReferenceDataRecords = flexTradeEntities.Where(fte => string.IsNullOrWhiteSpace(fte.TradeReference)).ToList();

                //Trade Date, Trade Product, Volume, Price and Direction must be populated
                requiredColumns = new Dictionary<string, string>
                    {
                        {customerDataUploadValidationEntityEnums.TradeDate, "Trade Date"},
                        {customerDataUploadValidationEntityEnums.TradeProduct, "Trade Product"},
                        {customerDataUploadValidationEntityEnums.Volume, customerDataUploadValidationEntityEnums.Volume},
                        {customerDataUploadValidationEntityEnums.Price, customerDataUploadValidationEntityEnums.Price},
                        {customerDataUploadValidationEntityEnums.Direction, "Trade Direction"}
                    };
                tempCustomerDataUploadMethods.GetMissingRecords(records, emptyTradeReferenceDataRecords, requiredColumns);

                //Validate Trade Reference
                var invalidTradeReferenceDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.TradeReference)
                    && !methods.IsValidFlexTradeReference(fte.TradeReference));

                foreach(var invalidTradeReferenceDataRecord in invalidTradeReferenceDataRecords)
                {
                    if(!records[invalidTradeReferenceDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.TradeReference].Contains($"Invalid Trade Reference '{invalidTradeReferenceDataRecord.TradeReference}'"))
                    {
                        records[invalidTradeReferenceDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.TradeReference].Add($"Invalid Trade Reference '{invalidTradeReferenceDataRecord.TradeReference}'");
                    }
                }

                //Validate Trade Date
                var invalidTradeDateDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.TradeDate)
                    && !methods.IsValidDate(fte.TradeDate));

                foreach(var invalidTradeDateDataRecord in invalidTradeDateDataRecords)
                {
                    if(!records[invalidTradeDateDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.TradeDate].Contains($"Invalid Trade Date '{invalidTradeDateDataRecord.TradeDate}'"))
                    {
                        records[invalidTradeDateDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.TradeDate].Add($"Invalid Trade Date '{invalidTradeDateDataRecord.TradeDate}'");
                    }
                }

                //Validate Trade Product
                var invalidTradeProductDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.TradeProduct)
                    && !methods.IsValidFlexTradeProduct(fte.TradeProduct));

                foreach(var invalidTradeProductDataRecord in invalidTradeProductDataRecords)
                {
                    if(!records[invalidTradeProductDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.TradeProduct].Contains($"Invalid Trade Product '{invalidTradeProductDataRecord.TradeProduct}'"))
                    {
                        records[invalidTradeProductDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.TradeProduct].Add($"Invalid Trade Product '{invalidTradeProductDataRecord.TradeProduct}'");
                    }
                }

                //Validate Volume
                var invalidVolumeDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.Volume)
                    && !methods.IsValidFlexTradeVolume(fte.Volume));

                foreach(var invalidVolumeDataRecord in invalidVolumeDataRecords)
                {
                    if(!records[invalidVolumeDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Volume].Contains($"Invalid Trade Volume '{invalidVolumeDataRecord.Volume}'"))
                    {
                        records[invalidVolumeDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Volume].Add($"Invalid Trade Volume '{invalidVolumeDataRecord.Volume}'");
                    }
                }

                //Validate Price
                var invalidPriceDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.Price)
                    && !methods.IsValidFlexTradePrice(fte.Price));

                foreach(var invalidPriceDataRecord in invalidPriceDataRecords)
                {
                    if(!records[invalidPriceDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Price].Contains($"Invalid Trade Price '{invalidPriceDataRecord.Price}'"))
                    {
                        records[invalidPriceDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Price].Add($"Invalid Trade Price '{invalidPriceDataRecord.Price}'");
                    }
                }

                //Validate Direction
                var invalidDirectionDataRecords = flexTradeEntities.Where(fte => !string.IsNullOrWhiteSpace(fte.Direction)
                    && !methods.IsValidFlexTradeDirection(fte.Direction));

                foreach(var invalidDirectionDataRecord in invalidDirectionDataRecords)
                {
                    if(!records[invalidDirectionDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Direction].Contains($"Invalid Trade Direction '{invalidDirectionDataRecord.Direction}'"))
                    {
                        records[invalidDirectionDataRecord.RowId.Value][customerDataUploadValidationEntityEnums.Direction].Add($"Invalid Trade Direction '{invalidDirectionDataRecord.Direction}'");
                    }
                }

                //Update Process Queue
                var errorMessage = tempCustomerDataUploadMethods.FinaliseValidation(records, processQueueGUID, createdByUserId, sourceId, new Enums.CustomerSchema.DataUploadValidation.SheetName().FlexTrade);
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexTradeDataAPIId, !string.IsNullOrWhiteSpace(errorMessage), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}