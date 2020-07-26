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

namespace ValidateUsageUploadTempFlexTradeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateUsageUploadTempFlexTradeDataController : ControllerBase
    {
        private readonly ILogger<ValidateUsageUploadTempFlexTradeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Int64 validateUsageUploadTempFlexTradeDataAPIId;

        public ValidateUsageUploadTempFlexTradeDataController(ILogger<ValidateUsageUploadTempFlexTradeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateUsageUploadTempFlexTradeDataAPI, _systemAPIPasswordEnums.ValidateUsageUploadTempFlexTradeDataAPI);
            validateUsageUploadTempFlexTradeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateUsageUploadTempFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());            
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(validateUsageUploadTempFlexTradeDataAPIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("ValidateUsageUploadTempFlexTradeData/Validate")]
        public void Validate([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    validateUsageUploadTempFlexTradeDataAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.ValidateUsageUploadTempFlexTradeDataAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempFlexTradeDataAPIId, true, $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored");
                    return;
                }

                //Get data from [Temp.Customer].[FlexTrade] table
                var customerDataRows = _tempCustomerMethods.FlexContract_GetByProcessQueueGUID(processQueueGUID);               

                if(!customerDataRows.Any())
                {
                    //Nothing to validate so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempFlexTradeDataAPIId, false, null);
                    return;
                }               

                //If any are empty records, store error
                var requiredColumns = new Dictionary<string, string>
                    {
                        {"BasketReference", "Basket Reference"}
                    };

                var errors = _tempCustomerMethods.GetMissingRecords(customerDataRows, requiredColumns).ToList();

                //TODO: If Trade Reference is not populated, all other fields are required

                //Validate Trade Reference
                var invalidTradeReferenceDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("TradeReference"))
                    && !_methods.IsValidFlexTradeReference(r.Field<string>("TradeReference")));

                foreach(var invalidTradeReferenceDataRecord in invalidTradeReferenceDataRecords)
                {
                    errors.Add($"Invalid Trade Reference '{invalidTradeReferenceDataRecord["TradeReference"]}' in row {invalidTradeReferenceDataRecord["RowId"]}");
                }

                //Validate Trade Date
                var invalidTradeDateDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("TradeDate"))
                    && !_methods.IsValidDate(r.Field<string>("TradeDate")));

                foreach(var invalidTradeDateDataRecord in invalidTradeDateDataRecords)
                {
                    errors.Add($"Invalid Trade Date '{invalidTradeDateDataRecord["TradeDate"]}' in row {invalidTradeDateDataRecord["RowId"]}");
                }

                //Validate Trade Product
                var invalidTradeProductDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("TradeProduct"))
                    && !_methods.IsValidFlexTradeProduct(r.Field<string>("TradeProduct")));

                foreach(var invalidTradeProductDataRecord in invalidTradeProductDataRecords)
                {
                    errors.Add($"Invalid Trade Product '{invalidTradeProductDataRecord["TradeProduct"]}' in row {invalidTradeProductDataRecord["RowId"]}");
                }

                //Validate Volume
                var invalidVolumeDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Volume"))
                    && !_methods.IsValidFlexTradeVolume(r.Field<string>("Volume")));

                foreach(var invalidVolumeDataRecord in invalidVolumeDataRecords)
                {
                    errors.Add($"Invalid Trade Volume '{invalidVolumeDataRecord["Volume"]}' in row {invalidVolumeDataRecord["RowId"]}");
                }

                //Validate Price
                var invalidPriceDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Price"))
                    && !_methods.IsValidFlexTradePrice(r.Field<string>("Price")));

                foreach(var invalidPriceDataRecord in invalidPriceDataRecords)
                {
                    errors.Add($"Invalid Trade Price '{invalidPriceDataRecord["Price"]}' in row {invalidPriceDataRecord["RowId"]}");
                }

                //Validate Direction
                var invalidDirectionDataRecords = customerDataRows.Where(r => !string.IsNullOrWhiteSpace(r.Field<string>("Direction"))
                    && !_methods.IsValidFlexTradeDirection(r.Field<string>("Direction")));

                foreach(var invalidDirectionDataRecord in invalidDirectionDataRecords)
                {
                    errors.Add($"Invalid Trade Direction '{invalidDirectionDataRecord["Direction"]}' in row {invalidDirectionDataRecord["RowId"]}");
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempFlexTradeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateUsageUploadTempFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

