using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitFlexContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFlexContractDataController : ControllerBase
    {
        private readonly ILogger<CommitFlexContractDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 commitFlexContractDataAPIId;

        public CommitFlexContractDataController(ILogger<CommitFlexContractDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitFlexContractDataAPI, _systemAPIPasswordEnums.CommitFlexContractDataAPI);
            commitFlexContractDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitFlexContractDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitFlexContractDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexContractData/Commit")]
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
                    commitFlexContractDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitFlexContractDataAPI, commitFlexContractDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Get Flex ContractTypeId from [Information].[ContractType]

                //Get data from [Temp.CustomerDataUpload].[FlexContract] where CanCommit = 1

                //Call CommitContractData API

                //Get BasketId from [Customer].[Basket] by BasketReference
                //If BasketId == 0
                //Insert into [Customer].[Basket]

                //Insert into [Mapping].[BasketToContractMeter]
                //Insert into [Mapping].[ContractToSupplier]
                //Insert into [Mapping].[ContractMeterToProduct]

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitFlexContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

