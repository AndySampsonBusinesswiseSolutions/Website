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
        private readonly Int64 commitFixedContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitFixedContractDataController(ILogger<CommitFixedContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitFixedContractDataAPI, password);
            commitFixedContractDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitFixedContractDataAPI);
        }

        [HttpPost]
        [Route("CommitFixedContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(commitFixedContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFixedContractData/Commit")]
        public void Commit([FromBody] object data)
        {
            var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
            var systemAPIMethods = new Methods.System.API();
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitFixedContractDataAPIId);

                if(!systemAPIMethods.PrerequisiteAPIsAreSuccessful(systemAPIGUIDEnums.CommitFixedContractDataAPI, commitFixedContractDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitFixedContractDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[FixedContract] where CanCommit = 1
                var fixedContractEntities = new Methods.Temp.CustomerDataUpload.FixedContract().FixedContract_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableFixedContractEntities = new Methods.Temp.CustomerDataUpload().GetCommitableEntities(fixedContractEntities);

                if(!commitableFixedContractEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFixedContractDataAPIId, false, null);
                    return;
                }

                var systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();

                //Add ContractType to jsonObject
                jsonObject.Add(systemAPIRequiredDataKeyEnums.ContractType, new Enums.InformationSchema.ContractType().Fixed);

                //Convert entities to a string
                var commitableEntityJSON = JsonConvert.SerializeObject(commitableFixedContractEntities.Where(cfce => !string.IsNullOrWhiteSpace(cfce.Value)));

                //Add ContractData to jsonObject
                jsonObject.Add(systemAPIRequiredDataKeyEnums.ContractData, commitableEntityJSON);

                //Call CommitContractData API and wait for response
                var APIId = systemAPIMethods.API_GetAPIIdByAPIGUID(systemAPIGUIDEnums.CommitContractDataAPI);
                var API = systemAPIMethods.PostAsJsonAsync(APIId, systemAPIGUIDEnums.CommitFixedContractDataAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFixedContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFixedContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}