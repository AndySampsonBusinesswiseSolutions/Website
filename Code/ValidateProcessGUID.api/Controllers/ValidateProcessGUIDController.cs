using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace ValidateProcessGUID.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateProcessGUIDController : ControllerBase
    {
        private readonly ILogger<ValidateProcessGUIDController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.System _systemMethods = new Methods.System();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Int64 APIId;

        public ValidateProcessGUIDController(ILogger<ValidateProcessGUIDController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.ValidateProcessGUIDAPI, _systemAPIPasswordEnums.ValidateProcessGUIDAPI);
            APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.ValidateProcessGUIDAPI);
        }

        [HttpPost]
        [Route("ValidateProcessGUID/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(APIId, callingGUID, jsonObject);

            return true;
        }

        [HttpPost]
        [Route("ValidateProcessGUID/Validate")]
        public long Validate([FromBody] object data)
        {
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            try
            {
                //Get Queue GUID
                var jsonObject = JObject.Parse(data.ToString());
                var processQueueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessQueueGUID].ToString();

                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    APIId);

                //Get Process GUID
                var processGUID = jsonObject[_systemAPIRequiredDataKeyEnums.ProcessGUID].ToString();

                //Validate Process GUID
                var processId = _systemMethods.Process_GetProcessIdByProcessGUID(processGUID);

                //If processId == 0 then the GUID provided isn't valid so create an error
                string errorMessage = processId == 0 ? $"Process GUID {processGUID} does not exist in [System].[Process] table" : null;

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, APIId, processId == 0, errorMessage);

                return processId;
            }
            catch(Exception error)
            {
                _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                return 0;
            }    
        }
    }
}
