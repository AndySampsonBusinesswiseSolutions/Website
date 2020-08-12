using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFlexReferenceVolumeDataController : ControllerBase
    {
        private readonly ILogger<CommitFlexReferenceVolumeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 commitFlexReferenceVolumeDataAPIId;

        public CommitFlexReferenceVolumeDataController(ILogger<CommitFlexReferenceVolumeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitFlexReferenceVolumeDataAPI, _systemAPIPasswordEnums.CommitFlexReferenceVolumeDataAPI);
            commitFlexReferenceVolumeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitFlexReferenceVolumeDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexReferenceVolumeData/Commit")]
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
                    commitFlexReferenceVolumeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitFlexReferenceVolumeDataAPI, commitFlexReferenceVolumeDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Get data from [Temp.CustomerDataUpload].[FlexReferenceVolume] where CanCommit = 1

                //Get ContractId from [Customer].[ContractDetail] by ContractReference
                //If ContractId == 0
                //Throw error as contract should have been invalidated or inserted

                //Get ReferenceVolumeId from [Customer].[ReferenceVolumeDetail] by DateFrom, DateTo and Reference Volume
                //If ReferenceVolumeId == 0
                //Insert into [Customer].[ReferenceVolume]

                //Insert into [Mapping].[ContractToReferenceVolume]

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexReferenceVolumeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

