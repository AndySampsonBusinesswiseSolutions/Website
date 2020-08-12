using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitSubMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<CommitSubMeterUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 commitSubMeterUsageDataAPIId;

        public CommitSubMeterUsageDataController(ILogger<CommitSubMeterUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitSubMeterUsageDataAPI, _systemAPIPasswordEnums.CommitSubMeterUsageDataAPI);
            commitSubMeterUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("CommitSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitSubMeterUsageDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitSubMeterUsageData/Commit")]
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
                    commitSubMeterUsageDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitSubMeterUsageDataAPI, commitSubMeterUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Get data from [Temp.CustomerDataUpload].[SubMeterUsage] where CanCommit = 1

                //Get SubMeterId from [Customer].[SubMeterDetail] by SubMeterIdentifier and Serial Number
                //If SubMeterId == 0
                //Throw error because Submeter should have been invalidated or inserted

                //Insert into [SubMeterId].[HalfHourUsage]

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

