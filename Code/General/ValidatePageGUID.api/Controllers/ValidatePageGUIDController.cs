﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

namespace ValidatePageGUID.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidatePageGUIDController : ControllerBase
    {
        private readonly ILogger<ValidatePageGUIDController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.System _systemMethods = new Methods.System();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 validatePageGUIDAPIId;

        public ValidatePageGUIDController(ILogger<ValidatePageGUIDController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidatePageGUIDAPI, _systemAPIPasswordEnums.ValidatePageGUIDAPI);
            validatePageGUIDAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidatePageGUIDAPI);
        }

        [HttpPost]
        [Route("ValidatePageGUID/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validatePageGUIDAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidatePageGUID/Validate")]
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
                    validatePageGUIDAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidatePageGUIDAPI, validatePageGUIDAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validatePageGUIDAPIId);

                string errorMessage = null;
                long pageId = 0;

                //Get Page GUID
                var pageGUID = _systemMethods.GetPageGUIDFromJObject(jsonObject);

                //Validate Page GUID
                pageId = _systemMethods.Page_GetPageIdByGUID(pageGUID);

                //If pageId == 0 then the GUID provided isn't valid so create an error
                if(pageId == 0)
                {
                    errorMessage = $"Page GUID {pageGUID} does not exist in [System].[Page] table";
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validatePageGUIDAPIId, pageId == 0, errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validatePageGUIDAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
