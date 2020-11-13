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
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.DemandForecast _demandForecastMethods = new Methods.DemandForecast();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.SystemSchema.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
        private static readonly Enums.DemandForecastSchema.Profile.Attribute _demandForecastProfileAttributeEnums = new Enums.DemandForecastSchema.Profile.Attribute();
        private readonly Int64 getFlexSpecificProfileAPIId;
        private readonly string hostEnvironment;
        #endregion

        public GetFlexSpecificProfileController(ILogger<GetFlexSpecificProfileController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().GetFlexSpecificProfileAPI, password);
            getFlexSpecificProfileAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetFlexSpecificProfileAPI);
        }

        [HttpPost]
        [Route("GetFlexSpecificProfile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(getFlexSpecificProfileAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetFlexSpecificProfile/Get")]
        public long Get([FromBody] object data)
        {

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
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
                    getFlexSpecificProfileAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getFlexSpecificProfileAPIId);

                //Get MPXN
                var mpxn = jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString();

                //Get Name Profile Attribute Id
                var nameProfileAttributeId = _demandForecastMethods.ProfileAttribute_GetProfileAttributeIdByProfileAttributeDescription(_demandForecastProfileAttributeEnums.Name);

                //Get ProfileId from ProfileDetail by MPXN and Name Profile Attribute Id
                var profileId = _demandForecastMethods.ProfileDetail_GetProfileIdByProfileAttributeIdAndProfileDetailDescription(nameProfileAttributeId, $"{mpxn} Flex Profile");

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getFlexSpecificProfileAPIId, false, null);

                return profileId;
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getFlexSpecificProfileAPIId, true, $"System Error Id {errorId}");

                return 0;
            }
        }
    }
}