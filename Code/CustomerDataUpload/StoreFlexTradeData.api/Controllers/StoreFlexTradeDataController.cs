using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreFlexTradeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreFlexTradeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreFlexTradeDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Int64 storeFlexTradeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreFlexTradeDataController(ILogger<StoreFlexTradeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFlexTradeDataAPI, password);
            storeFlexTradeDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("StoreFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(storeFlexTradeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFlexTradeData/Store")]
        public void Store([FromBody] object data)
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
                    storeFlexTradeDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreFlexTradeDataAPI, storeFlexTradeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeFlexTradeDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadFlexTradeMethods = new Methods.Temp.CustomerDataUpload.FlexTrade();

                //Get Flex Trade data from Customer Data Upload
                var flexTradeDictionary = _tempCustomerDataUploadMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Flex Trades']");

                foreach(var row in flexTradeDictionary.Keys)
                {
                    var values = flexTradeDictionary[row];
                    var tradeDate = methods.GetDateTimeSqlParameterFromDateTimeString(values[2]);

                    //Insert flex trade data into [Temp.CustomerDataUpload].[FlexTrade]
                    tempCustomerDataUploadFlexTradeMethods.FlexTrade_Insert(processQueueGUID, row, values[0], values[1], tradeDate, values[3], values[4], values[5], values[6]);
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