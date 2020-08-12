using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitExemptionToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitExemptionToMeterDataController : ControllerBase
    {
        private readonly ILogger<CommitExemptionToMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 commitExemptionToMeterDataAPIId;

        public CommitExemptionToMeterDataController(ILogger<CommitExemptionToMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitExemptionToMeterDataAPI, _systemAPIPasswordEnums.CommitExemptionToMeterDataAPI);
            commitExemptionToMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitExemptionToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitExemptionToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitExemptionToMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitExemptionToMeterData/Commit")]
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
                    commitExemptionToMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitExemptionToMeterDataAPI, commitExemptionToMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Get data from [Temp.CustomerDataUpload].[MeterExemption] where CanCommit = 1

                //Get MeterId from [Customer].[MeterDetail]
                //If MeterId == 0
                //Throw error as meter should have been invalidated or inserted

                //Get ExemptionId from [Customer].[ExemptionDetail]
                //If ExemptionId == 0
                //Insert into [Customer].[Exemption]

                //If MeterId != 0
                //Insert into [Mapping].[ExemptionToMeter]

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitExemptionToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitExemptionToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

