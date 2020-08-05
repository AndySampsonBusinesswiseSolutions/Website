using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ValidateCrossSheetEntityData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateCrossSheetEntityDataController : ControllerBase
    {
        private readonly ILogger<ValidateCrossSheetEntityDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 validateCrossSheetEntityDataAPIId;

        public ValidateCrossSheetEntityDataController(ILogger<ValidateCrossSheetEntityDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateCrossSheetEntityDataAPI, _systemAPIPasswordEnums.ValidateCrossSheetEntityDataAPI);
            validateCrossSheetEntityDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateCrossSheetEntityDataAPI);
        }

        [HttpPost]
        [Route("ValidateCrossSheetEntityData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(validateCrossSheetEntityDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateCrossSheetEntityData/Validate")]
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
                    validateCrossSheetEntityDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.ValidateCrossSheetEntityDataAPI, validateCrossSheetEntityDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: API Logic

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateCrossSheetEntityDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, validateCrossSheetEntityDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

