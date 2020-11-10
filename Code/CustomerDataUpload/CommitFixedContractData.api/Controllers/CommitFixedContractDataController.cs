using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CommitFixedContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFixedContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitFixedContractDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Information.ContractType _informationContractTypeEnums = new Enums.Information.ContractType();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitFixedContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitFixedContractDataController(ILogger<CommitFixedContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().CommitFixedContractDataAPI, password);
            commitFixedContractDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitFixedContractDataAPI);
        }

        [HttpPost]
        [Route("CommitFixedContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitFixedContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFixedContractData/Commit")]
        public void Commit([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            var customerDataUploadProcessQueueGUID = _systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitFixedContractDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitFixedContractDataAPI, commitFixedContractDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitFixedContractDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FixedContract] where CanCommit = 1
                var tempCustomerDataUploadFixedContractMethods = new Methods.Temp.CustomerDataUpload.FixedContract();
                var fixedContractEntities = tempCustomerDataUploadFixedContractMethods.FixedContract_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableFixedContractEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(fixedContractEntities);

                if(!commitableFixedContractEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFixedContractDataAPIId, false, null);
                    return;
                }

                //Add ContractType to jsonObject
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.ContractType, _informationContractTypeEnums.Fixed);

                //Convert dataRows to a string
                var commitableEntityJSON = JsonConvert.SerializeObject(commitableFixedContractEntities.Where(cfce => !string.IsNullOrWhiteSpace(cfce.Value)));

                //Add ContractData to jsonObject
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.ContractData, commitableEntityJSON);

                //Call CommitContractData API and wait for response
                var APIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitContractDataAPI);
                var API = _systemAPIMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.CommitFixedContractDataAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFixedContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFixedContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}