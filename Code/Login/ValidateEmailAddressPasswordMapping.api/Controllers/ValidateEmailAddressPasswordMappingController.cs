using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

namespace ValidateEmailAddressPasswordMapping.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateEmailAddressPasswordMappingController : ControllerBase
    {
        private readonly ILogger<ValidateEmailAddressPasswordMappingController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.System _systemMethods = new Methods.System();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 validateEmailAddressPasswordMappingAPIId;

        public ValidateEmailAddressPasswordMappingController(ILogger<ValidateEmailAddressPasswordMappingController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateEmailAddressPasswordMappingAPI, _systemAPIPasswordEnums.ValidateEmailAddressPasswordMappingAPI);
            validateEmailAddressPasswordMappingAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateEmailAddressPasswordMappingAPI);
        }

        [HttpPost]
        [Route("ValidateEmailAddressPasswordMapping/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateEmailAddressPasswordMappingAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateEmailAddressPasswordMapping/Validate")]
        public void Validate([FromBody] object data)
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
                    validateEmailAddressPasswordMappingAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateEmailAddressPasswordMappingAPI, validateEmailAddressPasswordMappingAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateEmailAddressPasswordMappingAPIId);

                string errorMessage = null;
                long mappingId = 0;

                //Get Password Id
                var password = _systemMethods.GetPasswordFromJObject(jsonObject);
                var passwordId = _administrationMethods.Password_GetPasswordIdByPassword(password);

                //Get User Id
                var userId = _administrationMethods.GetUserIdByEmailAddress(jsonObject);

                //Validate Password and User combination
                mappingId = _mappingMethods.PasswordToUser_GetPasswordFromJObjectToUserIdByPasswordIdAndUserId(passwordId, userId);

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
