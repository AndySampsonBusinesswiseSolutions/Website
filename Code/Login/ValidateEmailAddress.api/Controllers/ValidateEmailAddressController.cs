using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateEmailAddress.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateEmailAddressController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateEmailAddressController> _logger;
        private readonly Int64 validateEmailAddressAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateEmailAddressController(ILogger<ValidateEmailAddressController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateEmailAddressAPI, password);
            validateEmailAddressAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateEmailAddressAPI);
        }

        [HttpPost]
        [Route("ValidateEmailAddress/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(validateEmailAddressAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateEmailAddress/Validate")]
        public void Validate([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
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
                    validateEmailAddressAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateEmailAddressAPI, validateEmailAddressAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateEmailAddressAPIId);

                //Get Email Address
                var emailAddress = systemMethods.GetEmailAddressFromJObject(jsonObject);

                //Validate Email Address
                var emailAddressId = administrationUserMethods.UserDetail_GetUserDetailIdByEmailAddress(emailAddress);

                //If emailAddressId == 0 then the email address provided isn't valid so create an error
                string errorMessage = null;
                if(emailAddressId == 0)
                {
                    errorMessage = $"Email address {emailAddress} does not exist in [Administration.User].[UserDetail] table";
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateEmailAddressAPIId, emailAddressId == 0, errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateEmailAddressAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}