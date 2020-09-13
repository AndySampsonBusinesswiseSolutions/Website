using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace GetProfile.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class GetProfileController : ControllerBase
    {
        private readonly ILogger<GetProfileController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Int64 getProfileAPIId;

        public GetProfileController(ILogger<GetProfileController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.GetProfileAPI, _systemAPIPasswordEnums.GetProfileAPI);
            getProfileAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetProfileAPI);
        }

        [HttpPost]
        [Route("GetProfile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(getProfileAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetProfile/Get")]
        public Dictionary<long, Dictionary<long, decimal>> Get([FromBody] object data)
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
                    getProfileAPIId);

                var profile = new Dictionary<long, Dictionary<long, decimal>>();

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.GetProfileAPI, getProfileAPIId, jsonObject))
                {
                    return profile;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getProfileAPIId);

                //Launch GetProfileId process and wait for response
                var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetProfileIdAPI);
                var API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.GetProfileAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var profileId = Convert.ToInt64(result);

                //If no profile id returned, create system error
                if(profileId == 0)
                {
                    var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, "No ProfileId Found", "No ProfileId Found", Environment.StackTrace);

                    //Update Process Queue
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileAPIId, true, $"System Error Id {errorId}");

                    return profile;
                }
                
                //TODO: Create profiled usage

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileAPIId, false, null);

                return profile;
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileAPIId, true, $"System Error Id {errorId}");

                return new Dictionary<long, Dictionary<long, decimal>>();
            }
        }
    }
}