using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitLocalDistributionZoneToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitLocalDistributionZoneToMeterDataController : ControllerBase
    {
        private readonly ILogger<CommitLocalDistributionZoneToMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 commitLocalDistributionZoneToMeterDataAPIId;

        public CommitLocalDistributionZoneToMeterDataController(ILogger<CommitLocalDistributionZoneToMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitLocalDistributionZoneToMeterDataAPI, _systemAPIPasswordEnums.CommitLocalDistributionZoneToMeterDataAPI);
            commitLocalDistributionZoneToMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitLocalDistributionZoneToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitLocalDistributionZoneToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitLocalDistributionZoneToMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitLocalDistributionZoneToMeterData/Commit")]
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
                    commitLocalDistributionZoneToMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitLocalDistributionZoneToMeterDataAPI, commitLocalDistributionZoneToMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                //Insert into [Mapping].[LocalDistributionZoneToMeter]

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

