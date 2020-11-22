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
        #region Variables
        private readonly ILogger<ValidatePasswordController> _logger;
        private readonly Int64 validatePasswordAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidatePasswordController(ILogger<ValidatePasswordController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidatePasswordAPI, password);
            validatePasswordAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidatePasswordAPI);
        }

        [HttpPost]
        [Route("ValidatePassword/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validatePasswordAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidatePassword/Validate")]
        public void Validate([FromBody] object data)
        {
            var systemMethods = new Methods.SystemSchema();         

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

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
                    validatePasswordAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidatePasswordAPI, validatePasswordAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validatePasswordAPIId);

                //Get Password
                var password = systemMethods.GetPasswordFromJObject(jsonObject);

                //Validate Password
                var passwordId = new Methods.AdministrationSchema.Password().Password_GetPasswordIdByPassword(password);
                string errorMessage = null;

                //If passwordId == 0 then the password provided isn't valid so create an error
                if(passwordId == 0)
                {
                    errorMessage = $"Password {password} does not exist in [Administration.User].[Password] table";
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validatePasswordAPIId, passwordId == 0, errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validatePasswordAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}