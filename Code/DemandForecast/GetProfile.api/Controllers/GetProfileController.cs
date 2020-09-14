using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.DemandForecast _demandForecastMethods = new Methods.DemandForecast();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
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

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get meterId/subMeterId
                var meterId = meterType == "Meter"
                    ? GetMeterId(jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString())
                    : GetSubMeterId(jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString());
                
                //TODO: Create profiled usage
                var forecastGroupToTimePeriodDataRowList = _mappingMethods.ForecastGroupToTimePeriod_GetList();
                var forecastGroupToTimePeriodToProfileDataRowList = _mappingMethods.ForecastGroupToTimePeriodToProfile_GetByProfileId(profileId);
                var forecastGroupToTimePeriodToProfileIdList = forecastGroupToTimePeriodToProfileDataRowList.Select(d => d.Field<long>("ForecastGroupToTimePeriodToProfileId")).Distinct();
                var profileValueIdDictionary = forecastGroupToTimePeriodToProfileIdList
                    .ToDictionary(f => f, f => _mappingMethods.ForecastGroupToTimePeriodToProfileToProfileValue_GetProfileValueIdByForecastGroupToTimePeriodToProfileId(f));
                var profileValueDictionary = profileValueIdDictionary
                    .Select(p => p.Value)
                    .Distinct()
                    .ToDictionary(p => p, p => _demandForecastMethods.ProfileValue_GetProfileValueByProfileValueId(p));

                //Get Date dictionary
                var dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary();

                //Get latest Estimated Annual Usage
                var estimatedAnnualUsage = _supplyMethods.EstimatedAnnualUsage_GetLatestEstimatedAnnualUsage(meterType, meterId);

                var latestPeriodicUsageDate = DateTime.Today;
                var earliestRequiredPeriodicUsageDate = latestPeriodicUsageDate.AddYears(-1).AddDays(1);
                var periodicUsageDateIds = Enumerable.Range(0, latestPeriodicUsageDate.Subtract(earliestRequiredPeriodicUsageDate).Days + 1)
                    .Select(offset => earliestRequiredPeriodicUsageDate.AddDays(offset))
                    .Select(d => dateDictionary[_methods.ConvertDateTimeToSqlParameter(d).Substring(0, 10)]);

                //Get Date to ForecastGroup with priority 1
                var dateForecastGroupDictionary = _mappingMethods.DateToForecastGroup_GetDateForecastGroupDictionaryByPriority(1)
                    .Where(d => periodicUsageDateIds.Contains(d.Key))
                    .ToDictionary(d => d.Key, d=> d.Value);

                foreach(var dateId in periodicUsageDateIds)
                {
                    profile.Add(dateId, new Dictionary<long, decimal>());

                    //Get ForecastGroupId
                    var forecastGroupId = dateForecastGroupDictionary[dateId];

                    //Get ForecastGroupToTimePeriodIds
                    var forecastGroupToTimePeriodIds = forecastGroupToTimePeriodDataRowList.Where(d => d.Field<long>("ForecastGroupId") == forecastGroupId)
                        .ToDictionary(d => d.Field<long>("ForecastGroupToTimePeriodId"), d => d.Field<long>("TimePeriodId"));

                    //Loop through each ForecastGroupToTimePeriodId
                    foreach(var forecastGroupToTimePeriodId in forecastGroupToTimePeriodIds)
                    {
                        //Get ProfileValue
                        var profileValueId = profileValueIdDictionary[forecastGroupToTimePeriodId.Key];
                        var profileValue = profileValueDictionary[profileValueId];

                        //Calculate usage
                        var usage = profileValue * estimatedAnnualUsage;

                        //Add to dictionary
                        profile[dateId].Add(forecastGroupToTimePeriodId.Value, usage);
                    }
                }

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

        private long GetMeterId(string mpxn)
        {
            //Get MeterIdentifierMeterAttributeId
            var _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
            var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

            //Get MeterId
            return _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn).FirstOrDefault();
        }

        private long GetSubMeterId(string mpxn)
        {
            //Get SubMeterIdentifierSubMeterAttributeId
            var _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
            var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);

            //Get SubMeterId
            return _customerMethods.SubMeterDetail_GetSubMeterIdListBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, mpxn).FirstOrDefault();
        }
    }
}