using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().GetProfileAPI, password);
            getProfileAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().GetProfileAPI);
        }

        [HttpPost]
        [Route("GetProfile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(getProfileAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetProfile/Get")]
        public string Get([FromBody] object data)
        {
            var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
            var systemAPIMethods = new Methods.SystemSchema.API();
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
                    getProfileAPIId);

                if(!systemAPIMethods.PrerequisiteAPIsAreSuccessful(systemAPIGUIDEnums.GetProfileAPI, getProfileAPIId, hostEnvironment, jsonObject))
                {
                    return JsonConvert.SerializeObject(new Dictionary<long, Dictionary<long, decimal>>());
                }

                //Launch GetProfileId process and wait for response
                var APIId = systemAPIMethods.API_GetAPIIdByAPIGUID(systemAPIGUIDEnums.GetProfileIdAPI);
                var API = systemAPIMethods.PostAsJsonAsync(APIId, systemAPIGUIDEnums.GetProfileAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getProfileAPIId);

                var profileId = Convert.ToInt64(result.Result.ToString());

                //If no profile id returned, create system error
                if(profileId == 0)
                {
                    var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, "No ProfileId Found", "No ProfileId Found", Environment.StackTrace);

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileAPIId, true, $"System Error Id {errorId}");

                    return JsonConvert.SerializeObject(new Dictionary<long, Dictionary<long, decimal>>());
                };

                Parallel.ForEach(new List<long>{1, 2, 3, 4}, process => {
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

                var profile = new ConcurrentDictionary<long, ConcurrentDictionary<long, decimal>>(periodicUsageDateIds.ToDictionary(
                    p => p,
                    p => new ConcurrentDictionary<long, decimal>()
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

                Parallel.ForEach(periodicUsageDateIds, new ParallelOptions{MaxDegreeOfParallelism = 5}, periodicUsageDateId => {
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
                        profileDictionary.TryAdd(forecastGroupToTimePeriodId.Value, usage);
                    }
                });

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileAPIId, false, null);

                return JsonConvert.SerializeObject(profile);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getProfileAPIId, true, $"System Error Id {errorId}");

                return string.Empty;
            }
        }

        private void GetLatestEstimatedAnnualUsage(JObject jsonObject)
        {
            var meterId = new Methods.CustomerSchema().GetMeterId(jsonObject[new Enums.SystemSchema.API.RequiredDataKey().MPXN].ToString());

            //Get latest Estimated Annual Usage
            estimatedAnnualUsage = new Methods.SupplySchema().EstimatedAnnualUsage_GetLatestEstimatedAnnualUsage("Meter", meterId);
        }

        private void GetTimePeriodIdList(long profileId)
        {
            var mappingMethods = new Methods.MappingSchema();
            var informationMethods = new Methods.InformationSchema();

            //Get CommodityId for profileId
            var commodityId = mappingMethods.CommodityToProfile_GetCommodityIdByProfileId(profileId);

            //Get Commodity
            var commodity = informationMethods.Commodity_GetCommodityDescriptionByCommodityId(commodityId);

            var informationGranularityAttributeEnums = new Enums.InformationSchema.Granularity.Attribute();
            var granularityAttributeDescription = commodity == "Electricity"
                ? informationGranularityAttributeEnums.IsElectricityDefault
                : informationGranularityAttributeEnums.IsGasDefault;

            //Get GranularityId
            var granularityDefaultGranularityAttributeId = informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(granularityAttributeDescription);
            var granularityId = informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeId(granularityDefaultGranularityAttributeId);

            //Get TimePeriods for granularity
            timePeriodIdList = mappingMethods.GranularityToTimePeriod_GetList().Where(g => g.GranularityId == granularityId)
                                .Select(g => g.TimePeriodId).Distinct().ToList();
        }

        private void GetProfileValues(long profileId)
        {
            var mappingMethods = new Methods.MappingSchema();

            var forecastGroupToTimePeriodToProfileToProfileValueDictionary = mappingMethods.ForecastGroupToTimePeriodToProfileToProfileValue_GetDictionary();

            forecastGroupToTimePeriodTuple = mappingMethods.ForecastGroupToTimePeriod_GetList()
                .Select(fgttp => Tuple.Create(fgttp.ForecastGroupId, fgttp.ForecastGroupToTimePeriodId, fgttp.TimePeriodId)).ToList();

            var forecastGroupToTimePeriodToProfileDataRowList = mappingMethods.ForecastGroupToTimePeriodToProfile_GetByProfileId(profileId);
            forecastGroupToTimePeriodToProfileDictionary = forecastGroupToTimePeriodToProfileDataRowList.ToDictionary(
                    f => f.ForecastGroupToTimePeriodId,
                    f => f.ForecastGroupToTimePeriodToProfileId
                );

            profileValueIdDictionary = forecastGroupToTimePeriodToProfileDictionary.Values.Distinct().ToList()
                .ToDictionary(f => f, f => forecastGroupToTimePeriodToProfileToProfileValueDictionary[f]);

            var profileValueIdToProfileValueDictionary = new Methods.DemandForecastSchema().ProfileValue_GetDictionary();

            profileValueDictionary = profileValueIdDictionary
                .Select(p => p.Value)
                .Distinct()
                .ToDictionary(p => p, p => profileValueIdToProfileValueDictionary[p]);
        }

        private void GetDates()
        {
            var methods = new Methods();
            
            //Get Date dictionary
            var dateDictionary = new Methods.InformationSchema().Date_GetDateDescriptionIdDictionary();

            var latestPeriodicUsageDate = DateTime.Today;
            var earliestRequiredPeriodicUsageDate = latestPeriodicUsageDate.AddYears(-1).AddDays(1);
            periodicUsageDateIds = Enumerable.Range(0, latestPeriodicUsageDate.Subtract(earliestRequiredPeriodicUsageDate).Days + 1)
                .Select(offset => earliestRequiredPeriodicUsageDate.AddDays(offset))
                .Select(d => dateDictionary[methods.ConvertDateTimeToSqlParameter(d).Substring(0, 10)])
                .ToList();

            //Get Date to ForecastGroup with priority 1
            dateForecastGroupDictionary = new Methods.MappingSchema().DateToForecastGroup_GetDateForecastGroupDictionaryByPriority(1)
                .Where(d => periodicUsageDateIds.Contains(d.Key))
                .ToDictionary(d => d.Key, d=> d.Value);
        }
    }
}