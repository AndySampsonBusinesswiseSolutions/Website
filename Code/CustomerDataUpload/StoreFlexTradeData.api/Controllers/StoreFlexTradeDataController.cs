using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreFlexTradeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreFlexTradeDataController : ControllerBase
    {
        private readonly ILogger<StoreFlexTradeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeFlexTradeDataAPIId;

        public StoreFlexTradeDataController(ILogger<StoreFlexTradeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreFlexTradeDataAPI, _systemAPIPasswordEnums.StoreFlexTradeDataAPI);
            storeFlexTradeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("StoreFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(storeFlexTradeDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFlexTradeData/Store")]
        public void Store([FromBody] object data)
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
                    storeFlexTradeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreFlexTradeDataAPI, storeFlexTradeDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeFlexTradeDataAPIId);

                //Get Flex Trade data from Customer Data Upload
                var flexTradeDictionary = _tempCustomerDataUploadMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Flex Trades']");

                foreach(var row in flexTradeDictionary.Keys)
                {
                    var values = flexTradeDictionary[row];
                    var tradeDate = _methods.GetDateTimeSqlParameterFromDateTimeString(values[2]);

                    //Insert flex trade data into [Temp.CustomerDataUpload].[FlexTrade]
                    _tempCustomerDataUploadMethods.FlexTrade_Insert(processQueueGUID, row, values[0], values[1], tradeDate, values[3], values[4], values[5], values[6]);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexTradeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

