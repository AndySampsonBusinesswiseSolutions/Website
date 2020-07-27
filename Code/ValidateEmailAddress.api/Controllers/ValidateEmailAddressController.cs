﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

namespace ValidateEmailAddress.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateEmailAddressController : ControllerBase
    {
        private readonly ILogger<ValidateEmailAddressController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.System _systemMethods = new Methods.System();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 validateEmailAddressAPIId;

        public ValidateEmailAddressController(ILogger<ValidateEmailAddressController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateEmailAddressAPI, _systemAPIPasswordEnums.ValidateEmailAddressAPI);
            validateEmailAddressAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateEmailAddressAPI);
        }

        [HttpPost]
        [Route("ValidateEmailAddress/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateEmailAddressAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateEmailAddress/Validate")]
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
                    validateEmailAddressAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateEmailAddressAPI, validateEmailAddressAPIId, jsonObject))
                {
                    return;
                }

                string errorMessage = null;
                long emailAddressId = 0;

                //Get Email Address
                var emailAddress = _systemMethods.GetEmailAddressFromJObject(jsonObject);

                //Validate Email Address
                emailAddressId = _administrationMethods.UserDetail_GetUserDetailIdByEmailAddress(emailAddress);

                //If emailAddressId == 0 then the email address provided isn't valid so create an error
                if(emailAddressId == 0)
                {
                    errorMessage = $"Email address {emailAddress} does not exist in [Administration.User].[UserDetail] table";
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateEmailAddressAPIId, emailAddressId == 0, errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateEmailAddressAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
