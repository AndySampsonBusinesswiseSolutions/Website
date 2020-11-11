using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateEmailAddressPasswordMapping.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateEmailAddressPasswordMappingController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateEmailAddressPasswordMappingController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Int64 validateEmailAddressPasswordMappingAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateEmailAddressPasswordMappingController(ILogger<ValidateEmailAddressPasswordMappingController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateEmailAddressPasswordMappingAPI, password);
            validateEmailAddressPasswordMappingAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateEmailAddressPasswordMappingAPI);
        }

        [HttpPost]
        [Route("ValidateEmailAddressPasswordMapping/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validateEmailAddressPasswordMappingAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateEmailAddressPasswordMapping/Validate")]
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
                    validateEmailAddressPasswordMappingAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateEmailAddressPasswordMappingAPI, validateEmailAddressPasswordMappingAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateEmailAddressPasswordMappingAPIId);

                //Get Password Id
                var password = _systemMethods.GetPasswordFromJObject(jsonObject);

                var administrationPasswordMethods = new Methods.Administration.Password();
                var passwordId = administrationPasswordMethods.Password_GetPasswordIdByPassword(password);

                //Get User Id
                var userId = administrationUserMethods.GetUserIdByEmailAddress(jsonObject);

                //Validate Password and User combination
                var mappingMethods = new Methods.Mapping();
                var mappingId = mappingMethods.PasswordToUser_GetPasswordFromJObjectToUserIdByPasswordIdAndUserId(passwordId, userId);
                string errorMessage = null;

                //If mappingId == 0 then the combination of email address and password provided isn't valid so create an error
                if(mappingId == 0)
                {
                    errorMessage = $"UserId/PasswordId combination {userId}/{passwordId} does not exist in [Mapping].[PasswordToUser] table";
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateEmailAddressPasswordMappingAPIId, mappingId == 0, errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateEmailAddressPasswordMappingAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}