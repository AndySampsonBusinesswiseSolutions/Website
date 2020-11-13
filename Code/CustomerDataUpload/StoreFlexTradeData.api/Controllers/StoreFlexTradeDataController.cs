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
        private readonly Int64 storeFlexTradeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreFlexTradeDataController(ILogger<StoreFlexTradeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFlexTradeDataAPI, password);
            storeFlexTradeDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("StoreFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(storeFlexTradeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFlexTradeData/Store")]
        public void Store([FromBody] object data)
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
                    storeFlexTradeDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreFlexTradeDataAPI, storeFlexTradeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeFlexTradeDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadFlexTradeMethods = new Methods.Temp.CustomerDataUpload.FlexTrade();

                //Get Flex Trade data from Customer Data Upload
                //TODO: Make into Bulk Insert
                var flexTradeDictionary = new Methods.Temp.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Flex Trades']");

                foreach(var row in flexTradeDictionary.Keys)
                {
                    var values = flexTradeDictionary[row];
                    var tradeDate = methods.GetDateTimeSqlParameterFromDateTimeString(values[2]);

                    //Insert flex trade data into [Temp.CustomerDataUpload].[FlexTrade]
                    tempCustomerDataUploadFlexTradeMethods.FlexTrade_Insert(processQueueGUID, row, values[0], values[1], tradeDate, values[3], values[4], values[5], values[6]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexTradeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}