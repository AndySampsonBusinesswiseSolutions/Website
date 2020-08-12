using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitFlexTradeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFlexTradeDataController : ControllerBase
    {
        private readonly ILogger<CommitFlexTradeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 commitFlexTradeDataAPIId;

        public CommitFlexTradeDataController(ILogger<CommitFlexTradeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitFlexTradeDataAPI, _systemAPIPasswordEnums.CommitFlexTradeDataAPI);
            commitFlexTradeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitFlexTradeDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexTradeData/Commit")]
        public void Commit([FromBody] object data)
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
                    commitFlexTradeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitFlexTradeDataAPI, commitFlexTradeDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Get data from [Temp.CustomerDataUpload].[FlexTrade] where CanCommit = 1

                //Get BasketId from [Customer].[BasketDetail] by BasketReference
                //If BasketId == 0
                //Insert into [Customer].[Basket]

                //Get TradeId from [Customer].[TradeDetail] by TradeReference, TradeDate, TradeVolume, Trade Price
                //If TradeId == 0
                //Insert into [Customer].[Trade]

                //Insert into [Mapping].[BasketToTrade]
                //Insert into [Mapping].[RateUnitToTradeDetail]
                //Insert into [Mapping].[TradeDetailToVolumeUnit]
                //Insert into [Mapping].[TradeDetailToTradeProduct]

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexTradeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexTradeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

