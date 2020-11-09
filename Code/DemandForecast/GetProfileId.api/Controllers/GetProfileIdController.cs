using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace GetProfileId.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class GetProfileIdController : ControllerBase
    {
        private readonly ILogger<GetProfileIdController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.DemandForecast _demandForecastMethods = new Methods.DemandForecast();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private static readonly Enums.DemandForecast.ProfileAgent.Attribute _demandForecastProfileAgentAttributeEnums = new Enums.DemandForecast.ProfileAgent.Attribute();
        private readonly Int64 getProfileIdAPIId;
        private readonly string hostEnvironment;

        public GetProfileIdController(ILogger<GetProfileIdController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.GetProfileIdAPI, password);
            getProfileIdAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetProfileIdAPI);
        }

        [HttpPost]
        [Route("GetProfileId/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(getProfileIdAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetProfileId/Get")]
        public long Get([FromBody] object data)
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
                    getProfileIdAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getProfileIdAPIId);

                var profileId = 0L;

                //Get Priority Profile Agent Attribute Id
                var priorityProfileAgentAttributeId = _demandForecastMethods.ProfileAgentAttribute_GetProfileAgentAttributeIdByProfileAgentAttributeDescription(_demandForecastProfileAgentAttributeEnums.Priority);

                //Get Profile Agent API GUID Profile Agent Attribute Id
                var profileAgentAPIGUIDProfileAgentAttributeId = _demandForecastMethods.ProfileAgentAttribute_GetProfileAgentAttributeIdByProfileAgentAttributeDescription(_demandForecastProfileAgentAttributeEnums.ProfileAgentAPIGUID);                

                //Get Profile Agent Priorities
                var priorityList = _demandForecastMethods.ProfileAgentDetail_GetProfileAgentDetailDescriptionByProfileAgentAttributeId(priorityProfileAgentAttributeId);
                var orderedPriorityList = priorityList.Select(p => Convert.ToInt64(p)).OrderBy(p => p).Select(p => p.ToString());

                //Loop through each priority
                foreach(var priority in orderedPriorityList)
                {
                    //Get Profile Agent Id
                    var profileAgentId = _demandForecastMethods.ProfileAgentDetail_GetProfileAgentIdByProfileAgentAttributeIdAndProfileAgentDetailDescription(priorityProfileAgentAttributeId, priority);

                    //Get Profile Agent API GUID
                    var APIGUID = _demandForecastMethods.ProfileAgentDetail_GetProfileAgentDetailDescriptionByProfileAgentIdAndProfileAgentAttributeId(profileAgentId, profileAgentAPIGUIDProfileAgentAttributeId);
                    
                    //Call API and wait for response
                    var APIId = _systemMethods.API_GetAPIIdByAPIGUID(APIGUID);
                    var API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.GetProfileIdAPI, jsonObject);
                    var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                    profileId = Convert.ToInt64(result.Result.ToString());

                    if(profileId > 0)
                    {
                        break;
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileIdAPIId, false, null);

                return profileId;
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileIdAPIId, true, $"System Error Id {errorId}");

                return 0;
            }
        }
    }
}