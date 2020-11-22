using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
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
        private readonly Int64 validatePageGUIDAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidatePageGUIDController(ILogger<ValidatePageGUIDController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidatePageGUIDAPI, password);
            validatePageGUIDAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidatePageGUIDAPI);
        }

        [HttpPost]
        [Route("ValidatePageGUID/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validatePageGUIDAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidatePageGUID/Validate")]
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
                    validatePageGUIDAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidatePageGUIDAPI, validatePageGUIDAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validatePageGUIDAPIId);

                //Get Page GUID
                var pageGUID = systemMethods.GetPageGUIDFromJObject(jsonObject);

                //Validate Page GUID
                var pageId = systemMethods.Page_GetPageIdByGUID(pageGUID);

                //If pageId == 0 then the GUID provided isn't valid so create an error
                string errorMessage = null;
                if(pageId == 0)
                {
                    errorMessage = $"Page GUID {pageGUID} does not exist in [System].[Page] table";
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validatePageGUIDAPIId, pageId == 0, errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validatePageGUIDAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}