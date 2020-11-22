using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace GetFlexSpecificProfile.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class GetFlexSpecificProfileController : ControllerBase
    {
        #region Variables
        private readonly ILogger<GetFlexSpecificProfileController> _logger;
        private readonly Int64 getFlexSpecificProfileAPIId;
        private readonly string hostEnvironment;
        #endregion

        public GetFlexSpecificProfileController(ILogger<GetFlexSpecificProfileController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().GetFlexSpecificProfileAPI, password);
            getFlexSpecificProfileAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().GetFlexSpecificProfileAPI);
        }

        [HttpPost]
        [Route("GetFlexSpecificProfile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(getFlexSpecificProfileAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetFlexSpecificProfile/Get")]
        public long Get([FromBody] object data)
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
                    getFlexSpecificProfileAPIId);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getFlexSpecificProfileAPIId);

                var demandForecastMethods = new Methods.DemandForecastSchema();

                //Get MPXN
                var mpxn = jsonObject[new Enums.SystemSchema.API.RequiredDataKey().MPXN].ToString();

                //Get Name Profile Attribute Id
                var nameProfileAttributeId = demandForecastMethods.ProfileAttribute_GetProfileAttributeIdByProfileAttributeDescription(new Enums.DemandForecastSchema.Profile.Attribute().Name);

                //Get ProfileId from ProfileDetail by MPXN and Name Profile Attribute Id
                var profileId = demandForecastMethods.ProfileDetail_GetProfileIdByProfileAttributeIdAndProfileDetailDescription(nameProfileAttributeId, $"{mpxn} Flex Profile");

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getFlexSpecificProfileAPIId, false, null);

                return profileId;
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getFlexSpecificProfileAPIId, true, $"System Error Id {errorId}");

                return 0;
            }
        }
    }
}