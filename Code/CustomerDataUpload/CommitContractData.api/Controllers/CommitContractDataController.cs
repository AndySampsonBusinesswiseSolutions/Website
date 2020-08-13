using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitContractDataController : ControllerBase
    {
        private readonly ILogger<CommitContractDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 commitContractDataAPIId;

        public CommitContractDataController(ILogger<CommitContractDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitContractDataAPI, _systemAPIPasswordEnums.CommitContractDataAPI);
            commitContractDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitContractDataAPI);
        }

        [HttpPost]
        [Route("CommitContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitContractDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitContractData/Commit")]
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
                    commitContractDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitContractDataAPI, commitContractDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Get ContractId from [Customer].[ContractDetail] by ContractReference
                //If ContractId == 0
                //Insert into [Customer].[Contract]
                //Insert into [Mapping].[ContractToContractType]       

                //If ContractId != 0
                //Get ContractToContractTypeId by ContractId and FlexContractType

                //If ContractToContractTypeId == 0
                //Insert into [Mapping].[ContractToContractType]

                //Get ContractMeterId from [Customer].[ContractMeterDetail] by ContractStartDate, ContractEndDate and RateCount
                //If ContractMeterId == 0
                //Insert into [Customer].[ContractMeter]

                //Insert into [Mapping].[ContractToContractMeter]

                //Get MeterId from [Customer].[MeterDetail]
                //If MeterId == 0
                //Throw error as meter should be invalidated or inserted

                //Insert into [Mapping].[ContractMeterToMeter]

                //For each rate and standing and capacity charges
                //Get RateTypeId from [Information].[RateType]
                //Get ContractMeterRateDetailId from [Customer].[ContractMeterRateDetail] by Value
                //If ContractMeterRateDetailId == 0
                //Insert into [Customer].[ContractMeterRateDetail]

                //Insert into [Mapping].[ContractMeterRateDetailToRateType]
                //Insert into [Mapping].[ContractToMeterToContractMeterRateDetail]

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

