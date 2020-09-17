using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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
        private decimal estimatedAnnualUsage;
        private IEnumerable<long> timePeriodIdList;
        private IEnumerable<long> periodicUsageDateIds;
        private Dictionary<long, long> dateForecastGroupDictionary;
        private IEnumerable<DataRow> forecastGroupToTimePeriodDataRowList;
        private IEnumerable<DataRow> forecastGroupToTimePeriodToProfileDataRowList;
        private Dictionary<long, long> profileValueIdDictionary;
        private Dictionary<long, decimal> profileValueDictionary;

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
        public string Get([FromBody] object data)
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
                    return JsonConvert.SerializeObject(profile);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getProfileAPIId);

                //Launch GetProfileId process and wait for response
                var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetProfileIdAPI);
                var API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.GetProfileAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var profileId = Convert.ToInt64(result.Result.ToString());

                //If no profile id returned, create system error
                if(profileId == 0)
                {
                    var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, "No ProfileId Found", "No ProfileId Found", Environment.StackTrace);

                    //Update Process Queue
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileAPIId, true, $"System Error Id {errorId}");

                    return JsonConvert.SerializeObject(profile);
                }

                var parallelOptions = new ParallelOptions{MaxDegreeOfParallelism = 5};
                var processList = new List<long>{1, 2, 3, 4, 5};

                Parallel.ForEach(processList, parallelOptions, process => {
                    if(process == 1)
                    {
                        GetLatestEstimatedAnnualUsage(jsonObject);
                    }
                    else if(process == 2)
                    {
                        GetTimePeriodIdList();
                    }
                    else if(process == 3)
                    {
                        GetProfileValues(profileId);
                    }
                    else if(process == 4)
                    {
                        GetDates();
                    }
                });

                foreach(var periodicUsageDateId in periodicUsageDateIds)
                {
                    profile.Add(periodicUsageDateId, new Dictionary<long, decimal>());

                    //Get ForecastGroupId
                    var forecastGroupId = dateForecastGroupDictionary[periodicUsageDateId];

                    //Get ForecastGroupToTimePeriodIds
                    var forecastGroupToTimePeriodIds = forecastGroupToTimePeriodDataRowList
                        .Where(d => d.Field<long>("ForecastGroupId") == forecastGroupId)
                        .Where(d => timePeriodIdList.Contains(d.Field<long>("TimePeriodId")))
                        .ToDictionary(d => d.Field<long>("ForecastGroupToTimePeriodId"), d => d.Field<long>("TimePeriodId"));

                    //Loop through each ForecastGroupToTimePeriodId
                    foreach(var forecastGroupToTimePeriodId in forecastGroupToTimePeriodIds)
                    {
                        var forecastGroupToTimePeriodToProfileId = forecastGroupToTimePeriodToProfileDataRowList
                            .First(f => f.Field<long>("ForecastGroupToTimePeriodId") == forecastGroupToTimePeriodId.Key)
                            .Field<long>("ForecastGroupToTimePeriodToProfileId");

                        //Get ProfileValue
                        var profileValueId = profileValueIdDictionary[forecastGroupToTimePeriodToProfileId];
                        var profileValue = profileValueDictionary[profileValueId];

                        //Calculate usage
                        var usage = profileValue * estimatedAnnualUsage;

                        //Add to dictionary
                        profile[periodicUsageDateId].Add(forecastGroupToTimePeriodId.Value, usage);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileAPIId, false, null);

                return JsonConvert.SerializeObject(profile);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileAPIId, true, $"System Error Id {errorId}");

                return string.Empty;
            }
        }

        private void GetLatestEstimatedAnnualUsage(JObject jsonObject)
        {
            //Get MeterIdentifierMeterAttributeId
            var _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
            var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

            //Get MeterId
            var meterId =  _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString()).FirstOrDefault();

            //Get latest Estimated Annual Usage
            estimatedAnnualUsage = _supplyMethods.EstimatedAnnualUsage_GetLatestEstimatedAnnualUsage("Meter", meterId);
        }

        private void GetTimePeriodIdList()
        {
            var informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();

            //Get GranularityId
            var granularityDefaultGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(informationGranularityAttributeEnums.IsElectricityDefault);
            var granularityId = _informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeId(granularityDefaultGranularityAttributeId);

            //Get TimePeriods for granularity
            timePeriodIdList = _mappingMethods.GranularityToTimePeriod_GetList().Where(g => g.Field<long>("GranularityId") == granularityId)
                                .Select(g => g.Field<long>("TimePeriodId")).Distinct();
        }

        private void GetProfileValues(long profileId)
        {
            var forecastGroupToTimePeriodToProfileToProfileValueDataRowList = _mappingMethods.ForecastGroupToTimePeriodToProfileToProfileValue_GetList()
                .ToDictionary(d => d.Field<long>("ForecastGroupToTimePeriodToProfileId"), d => d.Field<long>("ProfileValueId"));
            forecastGroupToTimePeriodDataRowList = _mappingMethods.ForecastGroupToTimePeriod_GetList();
            var profileValueDataRowList = _demandForecastMethods.ProfileValue_GetList();

            forecastGroupToTimePeriodToProfileDataRowList = _mappingMethods.ForecastGroupToTimePeriodToProfile_GetByProfileId(profileId);
            var forecastGroupToTimePeriodToProfileIdList = forecastGroupToTimePeriodToProfileDataRowList.Select(d => d.Field<long>("ForecastGroupToTimePeriodToProfileId")).Distinct();
            profileValueIdDictionary = forecastGroupToTimePeriodToProfileIdList
                .ToDictionary(f => f, f => forecastGroupToTimePeriodToProfileToProfileValueDataRowList.First(v => v.Key == f).Value);
            profileValueDictionary = profileValueIdDictionary
                .Select(p => p.Value)
                .Distinct()
                .ToDictionary(p => p, p => profileValueDataRowList.First(v => v.Field<long>("ProfileValueId") == p).Field<decimal>("ProfileValue"));
        }

        private void GetDates()
        {
            //Get Date dictionary
            var dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary();

            var latestPeriodicUsageDate = DateTime.Today;
            var earliestRequiredPeriodicUsageDate = latestPeriodicUsageDate.AddYears(-1).AddDays(1);
            periodicUsageDateIds = Enumerable.Range(0, latestPeriodicUsageDate.Subtract(earliestRequiredPeriodicUsageDate).Days + 1)
                .Select(offset => earliestRequiredPeriodicUsageDate.AddDays(offset))
                .Select(d => dateDictionary[_methods.ConvertDateTimeToSqlParameter(d).Substring(0, 10)]);

            //Get Date to ForecastGroup with priority 1
            dateForecastGroupDictionary = _mappingMethods.DateToForecastGroup_GetDateForecastGroupDictionaryByPriority(1)
                .Where(d => periodicUsageDateIds.Contains(d.Key))
                .ToDictionary(d => d.Key, d=> d.Value);
        }
    }
}