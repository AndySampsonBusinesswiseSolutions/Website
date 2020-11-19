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
        #region Variables
        private readonly ILogger<GetProfileIdController> _logger;
        private readonly Int64 getProfileIdAPIId;
        private readonly string hostEnvironment;
        #endregion

        public GetProfileIdController(ILogger<GetProfileIdController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().GetProfileIdAPI, password);
            getProfileIdAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().GetProfileIdAPI);
        }

        [HttpPost]
        [Route("GetProfileId/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(getProfileIdAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetProfileId/Get")]
        public long Get([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

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
                    getProfileIdAPIId);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getProfileIdAPIId);

                var demandForecastProfileAgentAttributeEnums = new Enums.DemandForecastSchema.ProfileAgent.Attribute();
                var demandForecastMethods = new Methods.DemandForecast();
                var systemAPIMethods = new Methods.System.API();

                var profileId = 0L;
                var getProfileIdAPI = new Enums.SystemSchema.API.GUID().GetProfileIdAPI;

                //Get Priority Profile Agent Attribute Id
                var priorityProfileAgentAttributeId = demandForecastMethods.ProfileAgentAttribute_GetProfileAgentAttributeIdByProfileAgentAttributeDescription(demandForecastProfileAgentAttributeEnums.Priority);

                //Get Profile Agent API GUID Profile Agent Attribute Id
                var profileAgentAPIGUIDProfileAgentAttributeId = demandForecastMethods.ProfileAgentAttribute_GetProfileAgentAttributeIdByProfileAgentAttributeDescription(demandForecastProfileAgentAttributeEnums.ProfileAgentAPIGUID);                

                //Get Profile Agent Priorities
                var priorityList = demandForecastMethods.ProfileAgentDetail_GetProfileAgentDetailDescriptionByProfileAgentAttributeId(priorityProfileAgentAttributeId);
                var orderedPriorityList = priorityList.Select(p => Convert.ToInt64(p)).OrderBy(p => p).Select(p => p.ToString());

                //Loop through each priority
                foreach(var priority in orderedPriorityList)
                {
                    //Get Profile Agent Id
                    var profileAgentId = demandForecastMethods.ProfileAgentDetail_GetProfileAgentIdByProfileAgentAttributeIdAndProfileAgentDetailDescription(priorityProfileAgentAttributeId, priority);

                    //Get Profile Agent API GUID
                    var profileAgentAPIGUID = demandForecastMethods.ProfileAgentDetail_GetProfileAgentDetailDescriptionByProfileAgentIdAndProfileAgentAttributeId(profileAgentId, profileAgentAPIGUIDProfileAgentAttributeId);
                    
                    //Call API and wait for response
                    var profileAgentAPIId = systemAPIMethods.API_GetAPIIdByAPIGUID(profileAgentAPIGUID);
                    var result = systemAPIMethods.PostAsJsonAsyncAndAwaitResult(profileAgentAPIId, getProfileIdAPI, hostEnvironment, jsonObject);
                    profileId = Convert.ToInt64(result);

                    if(profileId > 0)
                    {
                        break;
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileIdAPIId, false, null);

                return profileId;
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileIdAPIId, true, $"System Error Id {errorId}");

                return 0;
            }
        }
    }
}