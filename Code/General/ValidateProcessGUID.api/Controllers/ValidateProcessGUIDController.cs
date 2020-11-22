using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateProcessGUID.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateProcessGUIDController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateProcessGUIDController> _logger;
        private readonly Int64 validateProcessGUIDAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateProcessGUIDController(ILogger<ValidateProcessGUIDController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateProcessGUIDAPI, password);
            validateProcessGUIDAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateProcessGUIDAPI);
        }

        [HttpPost]
        [Route("ValidateProcessGUID/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(validateProcessGUIDAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateProcessGUID/Validate")]
        public long Validate([FromBody] object data)
        {
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
                    validateProcessGUIDAPIId);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateProcessGUIDAPIId);

                //Get Process GUID
                var processGUID = systemMethods.GetProcessGUIDFromJObject(jsonObject);

                //Validate Process GUID
                var processId = systemMethods.Process_GetProcessIdByProcessGUID(processGUID);

                //If processId == 0 then the GUID provided isn't valid so create an error
                string errorMessage = processId == 0 ? $"Process GUID {processGUID} does not exist in [System].[Process] table" : null;

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateProcessGUIDAPIId, processId == 0, errorMessage);

                return processId;
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateProcessGUIDAPIId, true, $"System Error Id {errorId}");

                return 0;
            }    
        }
    }
}
