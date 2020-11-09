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
        private readonly ILogger<ValidateProcessGUIDController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.System _systemMethods = new Methods.System();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 validateProcessGUIDAPIId;
        private readonly string hostEnvironment;

        public ValidateProcessGUIDController(ILogger<ValidateProcessGUIDController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.ValidateProcessGUIDAPI, password);
            validateProcessGUIDAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateProcessGUIDAPI);
        }

        [HttpPost]
        [Route("ValidateProcessGUID/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateProcessGUIDAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateProcessGUID/Validate")]
        public long Validate([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
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
                    validateProcessGUIDAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateProcessGUIDAPIId);

                //Get Process GUID
                var processGUID = _systemMethods.GetProcessGUIDFromJObject(jsonObject);

                //Validate Process GUID
                var processId = _systemMethods.Process_GetProcessIdByProcessGUID(processGUID);

                //If processId == 0 then the GUID provided isn't valid so create an error
                string errorMessage = processId == 0 ? $"Process GUID {processGUID} does not exist in [System].[Process] table" : null;

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateProcessGUIDAPIId, processId == 0, errorMessage);

                return processId;
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateProcessGUIDAPIId, true, $"System Error Id {errorId}");

                return 0;
            }    
        }
    }
}
