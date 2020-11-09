using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidatePassword.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidatePasswordController : ControllerBase
    {
        private readonly ILogger<ValidatePasswordController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 validatePasswordAPIId;
        private readonly string hostEnvironment;

        public ValidatePasswordController(ILogger<ValidatePasswordController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().ValidatePasswordAPI, password);
            validatePasswordAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidatePasswordAPI);
        }

        [HttpPost]
        [Route("ValidatePassword/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validatePasswordAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidatePassword/Validate")]
        public void Validate([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();
            var informationMethods = new Methods.Information();            

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

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
                    validatePasswordAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidatePasswordAPI, validatePasswordAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validatePasswordAPIId);

                //Get Password
                var password = _systemMethods.GetPasswordFromJObject(jsonObject);

                //Validate Password
                var administrationPasswordMethods = new Methods.Administration.Password();
                var passwordId = administrationPasswordMethods.Password_GetPasswordIdByPassword(password);
                string errorMessage = null;

                //If passwordId == 0 then the password provided isn't valid so create an error
                if(passwordId == 0)
                {
                    errorMessage = $"Password {password} does not exist in [Administration.User].[Password] table";
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validatePasswordAPIId, passwordId == 0, errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validatePasswordAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}