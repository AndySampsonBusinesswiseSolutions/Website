using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitLineLossFactorClassToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitLineLossFactorClassToMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitLineLossFactorClassToMeterDataController> _logger;
        private readonly Int64 commitLineLossFactorClassToMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitLineLossFactorClassToMeterDataController(ILogger<CommitLineLossFactorClassToMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitLineLossFactorClassToMeterDataAPI, password);
            commitLineLossFactorClassToMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitLineLossFactorClassToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitLineLossFactorClassToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitLineLossFactorClassToMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitLineLossFactorClassToMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var systemMethods = new Methods.SystemSchema();
            var informationMethods = new Methods.InformationSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

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
                    commitLineLossFactorClassToMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitLineLossFactorClassToMeterDataAPI, commitLineLossFactorClassToMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //TODO: Build once LLF process is built

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}