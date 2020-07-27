﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreUsageUploadTempFlexTradeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreUsageUploadTempFlexTradeDataController : ControllerBase
    {
        private readonly ILogger<StoreUsageUploadTempFlexTradeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 storeUsageUploadTempFlexTradeDataAPIId;

        public StoreUsageUploadTempFlexTradeDataController(ILogger<StoreUsageUploadTempFlexTradeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreUsageUploadTempFlexTradeDataAPI, _systemAPIPasswordEnums.StoreUsageUploadTempFlexTradeDataAPI);
            storeUsageUploadTempFlexTradeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreUsageUploadTempFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("StoreUsageUploadTempFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(storeUsageUploadTempFlexTradeDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreUsageUploadTempFlexTradeData/Store")]
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
                    storeUsageUploadTempFlexTradeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.StoreUsageUploadTempFlexTradeDataAPI, storeUsageUploadTempFlexTradeDataAPIId, jsonObject))
                {
                    return;
                }

                //Get Flex Trade data from Customer Data Upload
                var flexTradeDictionary = _tempCustomerMethods.ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Flex Trades']");

                foreach(var row in flexTradeDictionary.Keys)
                {
                    var values = flexTradeDictionary[row];
                    var tradeDate = _methods.ConvertDateTimeToSqlParameter(DateTime.FromOADate(Convert.ToInt64(values[1])));

                    //Insert flex trade data into [Temp.Customer].[FlexTrade]
                    _tempCustomerMethods.FlexTrade_Insert(processQueueGUID, row, values[0], tradeDate, values[2], values[3], values[4], values[5]);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempFlexTradeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, storeUsageUploadTempFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

