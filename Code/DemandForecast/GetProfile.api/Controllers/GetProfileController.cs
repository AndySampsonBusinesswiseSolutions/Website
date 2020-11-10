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
using Microsoft.Extensions.Configuration;

namespace GetProfile.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class GetProfileController : ControllerBase
    {
        #region Variables
        private readonly ILogger<GetProfileController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.DemandForecast _demandForecastMethods = new Methods.DemandForecast();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Int64 getProfileAPIId;
        private decimal estimatedAnnualUsage;
        private List<long> timePeriodIdList;
        private List<long> periodicUsageDateIds;
        private Dictionary<long, long> dateForecastGroupDictionary;
        private Dictionary<long, long> profileValueIdDictionary;
        private Dictionary<long, decimal> profileValueDictionary;
        private Dictionary<long, long> forecastGroupToTimePeriodToProfileDictionary;
        private List<Tuple<long, long, long>> forecastGroupToTimePeriodTuple;
        private readonly string hostEnvironment;
        #endregion

        public GetProfileController(ILogger<GetProfileController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().GetProfileAPI, password);
            getProfileAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetProfileAPI);
        }

        [HttpPost]
        [Route("GetProfile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(getProfileAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetProfile/Get")]
        public string Get([FromBody] object data)
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
                    getProfileAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.GetProfileAPI, getProfileAPIId, hostEnvironment, jsonObject))
                {
                    return JsonConvert.SerializeObject(new Dictionary<long, Dictionary<long, decimal>>());
                }

                //Launch GetProfileId process and wait for response
                var APIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetProfileIdAPI);
                var API = _systemAPIMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.GetProfileAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getProfileAPIId);

                var profileId = Convert.ToInt64(result.Result.ToString());

                //If no profile id returned, create system error
                if(profileId == 0)
                {
                    var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, "No ProfileId Found", "No ProfileId Found", Environment.StackTrace);

                    //Update Process Queue
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileAPIId, true, $"System Error Id {errorId}");

                    return JsonConvert.SerializeObject(new Dictionary<long, Dictionary<long, decimal>>());
                }

                var parallelOptions = new ParallelOptions{MaxDegreeOfParallelism = 4};
                var processList = new List<long>{1, 2, 3, 4};

                Parallel.ForEach(processList, parallelOptions, process => {
                    if(process == 1)
                    {
                        GetLatestEstimatedAnnualUsage(jsonObject);
                    }
                    else if(process == 2)
                    {
                        GetTimePeriodIdList(profileId);
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

                var profile = new Dictionary<long, Dictionary<long, decimal>>(periodicUsageDateIds.ToDictionary(
                    p => p,
                    p => new Dictionary<long, decimal>()
                ));

                var forecastGroupToValidTimePeriodTuple = forecastGroupToTimePeriodTuple.Where(d => timePeriodIdList.Contains(d.Item3)).ToList();
                var forecastGroupIds = forecastGroupToValidTimePeriodTuple.Select(d => d.Item1).Distinct().ToList();
                var forecastGroupToTimePeriodIdDictionary = forecastGroupIds.ToDictionary(
                    f => f,
                    f => forecastGroupToValidTimePeriodTuple.Where(f1 => f1.Item1 == f).ToDictionary(
                        f1 => f1.Item2,
                        f1 => f1.Item3
                    )
                );

                foreach(var periodicUsageDateId in periodicUsageDateIds)
                {
                    //Get ForecastGroupId
                    var forecastGroupId = dateForecastGroupDictionary[periodicUsageDateId];

                    //Get ForecastGroupToTimePeriodIds
                    var forecastGroupToTimePeriodIds = forecastGroupToTimePeriodIdDictionary[forecastGroupId];

                    var profileDictionary = profile[periodicUsageDateId];

                    //Loop through each ForecastGroupToTimePeriodId
                    foreach(var forecastGroupToTimePeriodId in forecastGroupToTimePeriodIds)
                    {
                        var forecastGroupToTimePeriodToProfileId = forecastGroupToTimePeriodToProfileDictionary[forecastGroupToTimePeriodId.Key];

                        //Get ProfileValue
                        var profileValueId = profileValueIdDictionary[forecastGroupToTimePeriodToProfileId];
                        var profileValue = profileValueDictionary[profileValueId];

                        //Calculate usage
                        var usage = profileValue * estimatedAnnualUsage;

                        //Add to dictionary
                        profileDictionary.Add(forecastGroupToTimePeriodId.Value, usage);
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

        private void GetTimePeriodIdList(long profileId)
        {
            //Get CommodityId for profileId
            var commodityId = _mappingMethods.CommodityToProfile_GetCommodityIdByProfileId(profileId);

            //Get Commodity
            var commodity = _informationMethods.Commodity_GetCommodityDescriptionByCommodityId(commodityId);

            var informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();
            var granularityAttributeDescription = commodity == "Electricity"
                ? informationGranularityAttributeEnums.IsElectricityDefault
                : informationGranularityAttributeEnums.IsGasDefault;

            //Get GranularityId
            var granularityDefaultGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(granularityAttributeDescription);
            var granularityId = _informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeId(granularityDefaultGranularityAttributeId);

            //Get TimePeriods for granularity
            timePeriodIdList = _mappingMethods.GranularityToTimePeriod_GetList().Where(g => g.Field<long>("GranularityId") == granularityId)
                                .Select(g => g.Field<long>("TimePeriodId")).Distinct().ToList();
        }

        private void GetProfileValues(long profileId)
        {
            var forecastGroupToTimePeriodToProfileToProfileValueDictionary = _mappingMethods.ForecastGroupToTimePeriodToProfileToProfileValue_GetList()
                .ToDictionary(
                    d => d.Field<long>("ForecastGroupToTimePeriodToProfileId"), 
                    d => d.Field<long>("ProfileValueId")
                );

            forecastGroupToTimePeriodTuple = new List<Tuple<long, long, long>>();
            var forecastGroupToTimePeriodDataRowList = _mappingMethods.ForecastGroupToTimePeriod_GetList();
            foreach (DataRow r in forecastGroupToTimePeriodDataRowList)
            {
                var tup = Tuple.Create((long)r["ForecastGroupId"], (long)r["ForecastGroupToTimePeriodId"], (long)r["TimePeriodId"]);
                forecastGroupToTimePeriodTuple.Add(tup);
            }

            var profileValueIdToProfileValueDictionary = _demandForecastMethods.ProfileValue_GetList().ToDictionary(
                p => p.Field<long>("ProfileValueId"),
                p => p.Field<decimal>("ProfileValue")
            );

            var forecastGroupToTimePeriodToProfileDataRowList = _mappingMethods.ForecastGroupToTimePeriodToProfile_GetByProfileId(profileId);
            forecastGroupToTimePeriodToProfileDictionary = forecastGroupToTimePeriodToProfileDataRowList.ToDictionary(
                    f => f.Field<long>("ForecastGroupToTimePeriodId"),
                    f => f.Field<long>("ForecastGroupToTimePeriodToProfileId")
                );

            profileValueIdDictionary = forecastGroupToTimePeriodToProfileDictionary.Values.Distinct().ToList()
                .ToDictionary(f => f, f => forecastGroupToTimePeriodToProfileToProfileValueDictionary[f]);

            profileValueDictionary = profileValueIdDictionary
                .Select(p => p.Value)
                .Distinct()
                .ToDictionary(p => p, p => profileValueIdToProfileValueDictionary[p]);
        }

        private void GetDates()
        {
            var methods = new Methods();
            
            //Get Date dictionary
            var dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary();

            var latestPeriodicUsageDate = DateTime.Today;
            var earliestRequiredPeriodicUsageDate = latestPeriodicUsageDate.AddYears(-1).AddDays(1);
            periodicUsageDateIds = Enumerable.Range(0, latestPeriodicUsageDate.Subtract(earliestRequiredPeriodicUsageDate).Days + 1)
                .Select(offset => earliestRequiredPeriodicUsageDate.AddDays(offset))
                .Select(d => dateDictionary[methods.ConvertDateTimeToSqlParameter(d).Substring(0, 10)])
                .ToList();

            //Get Date to ForecastGroup with priority 1
            dateForecastGroupDictionary = _mappingMethods.DateToForecastGroup_GetDateForecastGroupDictionaryByPriority(1)
                .Where(d => periodicUsageDateIds.Contains(d.Key))
                .ToDictionary(d => d.Key, d=> d.Value);
        }
    }
}