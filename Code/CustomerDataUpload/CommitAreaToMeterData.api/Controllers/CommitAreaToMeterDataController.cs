using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitAreaToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitAreaToMeterDataController : ControllerBase
    {
        private readonly ILogger<CommitAreaToMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 commitAreaToMeterDataAPIId;

        public CommitAreaToMeterDataController(ILogger<CommitAreaToMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitAreaToMeterDataAPI, _systemAPIPasswordEnums.CommitAreaToMeterDataAPI);
            commitAreaToMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitAreaToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitAreaToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitAreaToMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitAreaToMeterData/Commit")]
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
                    commitAreaToMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitAreaToMeterDataAPI, commitAreaToMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1

                //Get AreaId from [Information].[Area]
                //If AreaId == 0
                //Insert into [Information].[Area]

                //Get MeterId from [Customer].[MeterDetail]
                //If MeterId == 0
                //Throw error as meter should have been invalidated or inserted

                //If MeterId != 0
                //Insert into [Mapping].[AreaToMeter]

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitAreaToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitAreaToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

