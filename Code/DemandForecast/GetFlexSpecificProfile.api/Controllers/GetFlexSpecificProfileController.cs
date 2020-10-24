using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace GetFlexSpecificProfile.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class GetFlexSpecificProfileController : ControllerBase
    {
        private readonly ILogger<GetFlexSpecificProfileController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.DemandForecast _demandForecastMethods = new Methods.DemandForecast();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.DemandForecast.Profile.Attribute _demandForecastProfileAttributeEnums = new Enums.DemandForecast.Profile.Attribute();
        private readonly Int64 getFlexSpecificProfileAPIId;

        public GetFlexSpecificProfileController(ILogger<GetFlexSpecificProfileController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.GetFlexSpecificProfileAPI, _systemAPIPasswordEnums.GetFlexSpecificProfileAPI);
            getFlexSpecificProfileAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetFlexSpecificProfileAPI);
        }

        [HttpPost]
        [Route("GetFlexSpecificProfile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(getFlexSpecificProfileAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetFlexSpecificProfile/Get")]
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