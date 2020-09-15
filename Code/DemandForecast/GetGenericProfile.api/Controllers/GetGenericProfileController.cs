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

namespace GetGenericProfile.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class GetGenericProfileController : ControllerBase
    {
        private readonly ILogger<GetGenericProfileController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.DemandForecast _demandForecastMethods = new Methods.DemandForecast();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.DemandForecast.Profile.Attribute _demandForecastProfileAttributeEnums = new Enums.DemandForecast.Profile.Attribute();
        private static readonly Enums.DemandForecast.Profile.EntityToMatch _demandForecastProfileEntityToMatchEnums = new Enums.DemandForecast.Profile.EntityToMatch();
        private readonly Int64 getGenericProfileAPIId;

        public GetGenericProfileController(ILogger<GetGenericProfileController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.GetGenericProfileAPI, _systemAPIPasswordEnums.GetGenericProfileAPI);
            getGenericProfileAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetGenericProfileAPI);
        }

        [HttpPost]
        [Route("GetGenericProfile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(getGenericProfileAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetGenericProfile/Get")]
        public long Get([FromBody] object data)
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
                    getGenericProfileAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getGenericProfileAPIId);

                //Get meterId/subMeterId
                var meterId = GetMeterId(jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString());

                //Get Is Generic Profile Attribute Id
                var isGenericProfileAttributeId = _demandForecastMethods.ProfileAttribute_GetProfileAttributeIdByProfileAttributeDescription(_demandForecastProfileAttributeEnums.IsGeneric);

                //Get Entity To Match Profile Attribute Id
                var entityToMatchProfileAttributeId = _demandForecastMethods.ProfileAttribute_GetProfileAttributeIdByProfileAttributeDescription(_demandForecastProfileAttributeEnums.EntityToMatch);

                //Get all generic profiles
                var genericProfileIds = _demandForecastMethods.ProfileDetail_GetProfileIdListByProfileAttributeIdAndProfileDetailDescription(isGenericProfileAttributeId, "True");

                //Store matched profileIds
                var matchedProfileIds = new ConcurrentBag<long>();
                var parallelOptions = new ParallelOptions{MaxDegreeOfParallelism = 5};

                //For each generic profile, find all the entities to match against
                Parallel.ForEach(genericProfileIds, parallelOptions, genericProfileId => {
                    var entitiesToMatchDictionary = _demandForecastMethods.ProfileDetail_GetProfileDetailDescriptionListByProfileIdAndProfileAttributeId(genericProfileId, entityToMatchProfileAttributeId)
                        .ToDictionary(e => e, e => EntityMatches(e, genericProfileId, meterId));

                    if(entitiesToMatchDictionary.Values.All(v => v))
                    {
                        matchedProfileIds.Add(genericProfileId);
                    }
                });

                var profileId = (!matchedProfileIds.Any() || matchedProfileIds.Distinct().Count() > 1) ? 0 : matchedProfileIds.First();

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getGenericProfileAPIId, false, null);

                return profileId;
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getGenericProfileAPIId, true, $"System Error Id {errorId}");

                return 0;
            }
        }

        private long GetMeterId(string mpxn)
        {
            //Get MeterIdentifierMeterAttributeId
            var _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
            var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

            //Get MeterId
            return _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn).FirstOrDefault();
        }

        private bool EntityMatches(string entity, long profileId, long meterId)
        {
            if(entity == _demandForecastProfileEntityToMatchEnums.Commodity)
            {
                return CommodityMatches(profileId, meterId);
            }

            if(entity == _demandForecastProfileEntityToMatchEnums.ProfileClass)
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

