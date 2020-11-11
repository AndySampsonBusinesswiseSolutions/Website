using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidatePageGUID.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidatePageGUIDController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidatePageGUIDController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Int64 validatePageGUIDAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidatePageGUIDController(ILogger<ValidatePageGUIDController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidatePageGUIDAPI, password);
            validatePageGUIDAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidatePageGUIDAPI);
        }

        [HttpPost]
        [Route("ValidatePageGUID/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(validatePageGUIDAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidatePageGUID/Validate")]
        public void Validate([FromBody] object data)
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
                    validatePageGUIDAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidatePageGUIDAPI, validatePageGUIDAPIId, hostEnvironment, jsonObject))
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
