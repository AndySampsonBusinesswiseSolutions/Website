using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitMeterExemptionData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterExemptionDataController : ControllerBase
    {
        private readonly ILogger<CommitMeterExemptionDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 commitMeterExemptionDataAPIId;

        public CommitMeterExemptionDataController(ILogger<CommitMeterExemptionDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitMeterExemptionDataAPI, _systemAPIPasswordEnums.CommitMeterExemptionDataAPI);
            commitMeterExemptionDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitMeterExemptionDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterExemptionData/Commit")]
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
                    commitMeterExemptionDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitMeterExemptionDataAPI, commitMeterExemptionDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Get data from [Temp.CustomerDataUpload].[MeterExemption] where CanCommit = 1

                //Get MeterId from [Customer].[MeterDetail] by MPXN
                //If MeterId == 0
                //Throw error as meter should have been invalidated or inserted

                //Get ExemptionId from [Customer].[ExemptionDetail] by DateFrom, DateTo and ExemptionProportion
                //If ExemptionId == 0
                //Insert into [Customer].[Exemption]

                //Insert into [Mapping].[ExemptionToMeter]
                //Insert into [Mapping].[ExemptionToExemptionProduct]

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterExemptionDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterExemptionDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

