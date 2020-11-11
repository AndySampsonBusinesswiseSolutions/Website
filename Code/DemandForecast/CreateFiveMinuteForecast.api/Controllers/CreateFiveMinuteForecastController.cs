using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CreateFiveMinuteForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateFiveMinuteForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateFiveMinuteForecastController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.SystemSchema.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
        private readonly Enums.InformationSchema.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.InformationSchema.Granularity.Attribute();
        private readonly Int64 createFiveMinuteForecastAPIId;
        private readonly string granularityCode = "FiveMinute";
        private List<Tuple<long, long, decimal>> existingFiveMinuteForecasts;
        private Dictionary<long, Dictionary<long, decimal>> existingFiveMinuteForecastDictionary;
        private Dictionary<long, Dictionary<long, decimal>> forecastDictionary;
        private readonly string hostEnvironment;
        #endregion

        public CreateFiveMinuteForecastController(ILogger<CreateFiveMinuteForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateFiveMinuteForecastAPI, password);
            createFiveMinuteForecastAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateFiveMinuteForecastAPI);
        }

        [HttpPost]
        [Route("CreateFiveMinuteForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(createFiveMinuteForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateFiveMinuteForecast/Create")]
        public void Create([FromBody] object data)
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
                    createFiveMinuteForecastAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CreateFiveMinuteForecastAPI, createFiveMinuteForecastAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createFiveMinuteForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                Parallel.ForEach(new List<bool>{true, false}, getForecast => {
                    if(getForecast)
                    {
                        GetForecastDictionary(meterType, meterId);
                    }
                    else
                    {
                        GetExistingForecast(meterType, meterId);
                    }
                });

                var newFiveMinuteForecastTuples = new List<Tuple<long, long, decimal>>();
                var oldFiveMinuteForecastTuples = new List<Tuple<long, long, decimal>>();

                foreach (var forecastDate in forecastDictionary)
                {
                    foreach (var forecastTimePeriod in forecastDate.Value)
                    {
                        var isNewPeriod = !existingFiveMinuteForecastDictionary.ContainsKey(forecastDate.Key)
                            || !existingFiveMinuteForecastDictionary[forecastDate.Key].ContainsKey(forecastTimePeriod.Key);
                        var addUsageToDataTable = isNewPeriod
                            || existingFiveMinuteForecastDictionary[forecastDate.Key][forecastTimePeriod.Key] != forecastTimePeriod.Value;

                        if (addUsageToDataTable)
                        {
                            if (!isNewPeriod)
                            {
                                oldFiveMinuteForecastTuples.Add(new Tuple<long, long, decimal>(forecastDate.Key, forecastTimePeriod.Key, existingFiveMinuteForecastDictionary[forecastDate.Key][forecastTimePeriod.Key]));
                            }

                            var newFiveMinuteForecastTuple = new Tuple<long, long, decimal>(forecastDate.Key, forecastTimePeriod.Key, forecastTimePeriod.Value);
                            newFiveMinuteForecastTuples.Add(newFiveMinuteForecastTuple);
                        }
                    }
                }

                if (newFiveMinuteForecastTuples.Any())
                {
                    existingFiveMinuteForecasts = existingFiveMinuteForecasts.Except(oldFiveMinuteForecastTuples).ToList();
                    existingFiveMinuteForecasts.AddRange(newFiveMinuteForecastTuples);

                    //Insert into history and latest tables
                    _supplyMethods.CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "DateId", "TimePeriodId" }, newFiveMinuteForecastTuples, existingFiveMinuteForecasts);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createFiveMinuteForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createFiveMinuteForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing five minute forecast
            existingFiveMinuteForecasts = _supplyMethods.ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "DateId", "TimePeriodId");
            existingFiveMinuteForecastDictionary = existingFiveMinuteForecasts.Select(f => f.Item1).Distinct()
                .ToDictionary(
                    d => d,
                    d => existingFiveMinuteForecasts.Where(f => f.Item1 == d).ToDictionary(
                        t => t.Item2,
                        t => t.Item3
                    )
            );
        }

        private void GetForecastDictionary(string meterType, long meterId)
        {
            //Get latest loaded usage
            var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatestTuple(meterType, meterId);

            //Get GranularityId
            var granularityCodeGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityCode);
            var granularityId = _informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(granularityCodeGranularityAttributeId, granularityCode);

            //Get required time periods
            var nonStandardGranularityToTimePeriodDataRows = _mappingMethods.GranularityToTimePeriod_NonStandardDate_GetListByGranularityId(granularityId);
            var standardGranularityToTimePeriodDataRows = _mappingMethods.GranularityToTimePeriod_StandardDate_GetListByGranularityId(granularityId);
            var standardGranularityToTimePeriods = standardGranularityToTimePeriodDataRows.Select(d => d.Field<long>("TimePeriodId")).ToList();

            //Set up forecast dictionary
            var futureDateToUsageDateDictionary = _supplyMethods.DateMapping_GetLatestDictionary(meterType, meterId);

            var timePeriodToTimePeriodDictionary = _mappingMethods.TimePeriodToTimePeriod_GetDictionary();
            var nonStandardGranularityDates = nonStandardGranularityToTimePeriodDataRows.Select(d => d.Field<long>("DateId")).Distinct()
                .ToDictionary(
                    d => d,
                    d => nonStandardGranularityToTimePeriodDataRows.Where(n => n.Field<long>("DateId") == d).Select(d => d.Field<long>("TimePeriodId")).ToList()
                );

            forecastDictionary = new Dictionary<long, Dictionary<long, decimal>>(
                futureDateToUsageDateDictionary.ToDictionary(
                    f => f.Key,
                    f => (nonStandardGranularityDates.ContainsKey(f.Key)
                            ? nonStandardGranularityDates[f.Key]
                            : standardGranularityToTimePeriods).ToDictionary(t => t, t => new decimal())
                )
            );

            var forecastFoundDictionary = new Dictionary<long, Dictionary<long, bool>>(
                futureDateToUsageDateDictionary.ToDictionary(
                    f => f.Key,
                    f => (nonStandardGranularityDates.ContainsKey(f.Key)
                            ? nonStandardGranularityDates[f.Key]
                            : standardGranularityToTimePeriods).ToDictionary(t => t, t => new bool())
                )
            );

            var timePeriodToMappedTimePeriodDictionary = new Dictionary<long, Dictionary<long, List<long>>>(
                timePeriodToTimePeriodDictionary.ToDictionary(
                    t => t.Key,
                    t => t.Value.Select(m => m).Distinct()
                        .ToDictionary(m => m, m => timePeriodToTimePeriodDictionary.Where(t => t.Value.Contains(m)).Select(t => t.Key).ToList())
                        .OrderBy(m => m.Value.Count())
                        .ToDictionary(o => o.Key, o => o.Value)
                )
            );

            //Loop through future date ids
            foreach (var forecast in forecastDictionary)
            {
                //Get usage date id
                var usageDateId = futureDateToUsageDateDictionary[forecast.Key];

                //Get usage for date
                var usageForDateList = latestLoadedUsage.Where(u => u.Item1 == usageDateId).ToList();
                var timePeriodIds = forecast.Value.Keys.ToList();
                var forecastFound = forecastFoundDictionary[forecast.Key];

                foreach (var timePeriodId in timePeriodIds)
                {
                    if(forecastFound[timePeriodId])
                    {
                        continue;
                    }
                        
                    //Get usage for time period
                    var usageForTimePeriodList = usageForDateList.Where(u => u.Item2 == timePeriodId).ToList();

                    if (usageForTimePeriodList.Any())
                    {
                        _supplyMethods.SetForecastValue(forecast.Value, forecastFound, timePeriodId, _supplyMethods.GetUsageByUsageType(usageForTimePeriodList));
                    }
                    else
                    {
                        var mappedTimePeriodDictionary = timePeriodToMappedTimePeriodDictionary[timePeriodId]
                            .First(t => usageForDateList.Any(u => u.Item2 == t.Key));

                        usageForTimePeriodList = usageForDateList.Where(u => u.Item2 == mappedTimePeriodDictionary.Key).ToList();

                        //Get usage based on usage type priority
                        var mappedUsage = _supplyMethods.GetUsageByUsageType(usageForTimePeriodList);
                        var mappedTimePeriodIdsWithUsageList = mappedTimePeriodDictionary.Value.Where(v => usageForDateList.Any(u => u.Item2 == v)).ToList();
                        var missingTimePeriodIds = mappedTimePeriodDictionary.Value.Except(mappedTimePeriodIdsWithUsageList);

                        foreach (var mappedtimePeriodId in mappedTimePeriodIdsWithUsageList)
                        {
                            //Get usage for time period
                            usageForTimePeriodList = usageForDateList.Where(u => u.Item2 == mappedtimePeriodId).ToList();

                            _supplyMethods.SetForecastValue(forecast.Value, forecastFound, timePeriodId, _supplyMethods.GetUsageByUsageType(usageForTimePeriodList));
                        }

                        if (missingTimePeriodIds.Any())
                        {
                            var timePeriodUsage = mappedTimePeriodDictionary.Value
                                .Where(t => forecastDictionary[forecast.Key].ContainsKey(t))
                                .Sum(t => forecastDictionary[forecast.Key][t]);
                            var missingTimePeriodUsage = (mappedUsage - timePeriodUsage) / missingTimePeriodIds.Count();

                            foreach (var missingTimePeriodId in missingTimePeriodIds)
                            {
                                _supplyMethods.SetForecastValue(forecast.Value, forecastFound, missingTimePeriodId, missingTimePeriodUsage);
                            }
                        }
                    }
                }
            }
        }
    }
}