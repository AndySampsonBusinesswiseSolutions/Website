using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GetGenericProfile.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class GetGenericProfileController : ControllerBase
    {
        #region Variables
        private readonly ILogger<GetGenericProfileController> _logger;
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Int64 getGenericProfileAPIId;
        private readonly string hostEnvironment;
        #endregion

        public GetGenericProfileController(ILogger<GetGenericProfileController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().GetGenericProfileAPI, password);
            getGenericProfileAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().GetGenericProfileAPI);
        }

        [HttpPost]
        [Route("GetGenericProfile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(getGenericProfileAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetGenericProfile/Get")]
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
                    getGenericProfileAPIId);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getGenericProfileAPIId);

                var demandForecastProfileAttributeEnums = new Enums.DemandForecastSchema.Profile.Attribute();
                var demandForecastMethods = new Methods.DemandForecast();

                //Get meterId/subMeterId
                var meterId = new Methods.Customer().GetMeterId(jsonObject[new Enums.SystemSchema.API.RequiredDataKey().MPXN].ToString());

                //Get Is Generic Profile Attribute Id
                var isGenericProfileAttributeId = demandForecastMethods.ProfileAttribute_GetProfileAttributeIdByProfileAttributeDescription(demandForecastProfileAttributeEnums.IsGeneric);

                //Get Entity To Match Profile Attribute Id
                var entityToMatchProfileAttributeId = demandForecastMethods.ProfileAttribute_GetProfileAttributeIdByProfileAttributeDescription(demandForecastProfileAttributeEnums.EntityToMatch);

                //Get all generic profiles
                var genericProfileIds = demandForecastMethods.ProfileDetail_GetProfileIdListByProfileAttributeIdAndProfileDetailDescription(isGenericProfileAttributeId, "True");

                //Store matched profileIds
                var matchedProfileIds = new ConcurrentBag<long>();
                var parallelOptions = new ParallelOptions{MaxDegreeOfParallelism = 5};

                //For each generic profile, find all the entities to match against
                Parallel.ForEach(genericProfileIds, parallelOptions, genericProfileId => {
                    var entitiesToMatchDictionary = demandForecastMethods.ProfileDetail_GetProfileDetailDescriptionListByProfileIdAndProfileAttributeId(genericProfileId, entityToMatchProfileAttributeId)
                        .ToDictionary(e => e, e => EntityMatches(e, genericProfileId, meterId));

                    if(entitiesToMatchDictionary.Values.All(v => v))
                    {
                        matchedProfileIds.Add(genericProfileId);
                    }
                });

                var profileId = (!matchedProfileIds.Any() || matchedProfileIds.Distinct().Count() > 1) ? 0 : matchedProfileIds.First();

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getGenericProfileAPIId, false, null);

                return profileId;
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getGenericProfileAPIId, true, $"System Error Id {errorId}");

                return 0;
            }
        }

        private bool EntityMatches(string entity, long profileId, long meterId)
        {
            var demandForecastProfileEntityToMatchEnums = new Enums.DemandForecastSchema.Profile.EntityToMatch();

            if(entity == demandForecastProfileEntityToMatchEnums.Commodity)
            {
                return CommodityMatches(profileId, meterId);
            }

            if(entity == demandForecastProfileEntityToMatchEnums.ProfileClass)
            {
                return ProfileClassMatches(profileId, meterId);
            }

            return false;
        }

        private bool CommodityMatches(long profileId, long meterId)
        {
            //Get CommodityId for Profile Id
            var profileCommodityId = _mappingMethods.CommodityToProfile_GetCommodityIdByProfileId(profileId);

            //Get CommodityId for Meter Id
            var meterCommodityId = _mappingMethods.CommodityToMeter_GetCommodityIdByMeterId(meterId);

            return profileCommodityId == meterCommodityId;
        }

        private bool ProfileClassMatches(long profileId, long meterId)
        {
            //Get ProfileClassId for Profile Id
            var profileProfileClassId = _mappingMethods.ProfileToProfileClass_GetProfileClassIdByProfileId(profileId);

            //Get ProfileClassId for Meter Id
            var meterProfileClassId = _mappingMethods.MeterToProfileClass_GetProfileClassIdByMeterId(meterId);

            return profileProfileClassId == meterProfileClassId;
        }
    }
}