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

namespace CreateHalfHourForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateHalfHourForecastController : ControllerBase
    {
        private readonly ILogger<CreateHalfHourForecastController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Information.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();
        private readonly Int64 createHalfHourForecastAPIId;
        private readonly string granularityCode = "HalfHour";
        private List<Tuple<long, long, decimal>> existingHalfHourForecasts;
        private Dictionary<long, Dictionary<long, decimal>> existingHalfHourForecastDictionary;
        private Dictionary<long, Dictionary<long, decimal>> forecastDictionary;

        public CreateHalfHourForecastController(ILogger<CreateHalfHourForecastController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CreateHalfHourForecastAPI, _systemAPIPasswordEnums.CreateHalfHourForecastAPI);
            createHalfHourForecastAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateHalfHourForecastAPI);
        }

        [HttpPost]
        [Route("CreateHalfHourForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(createHalfHourForecastAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateHalfHourForecast/Create")]
        public void Create([FromBody] object data)
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
                    createHalfHourForecastAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CreateHalfHourForecastAPI, createHalfHourForecastAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createHalfHourForecastAPIId);

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

                var newHalfHourForecastTuples = new List<Tuple<long, long, decimal>>();
                var oldHalfHourForecastTuples = new List<Tuple<long, long, decimal>>();

                foreach (var forecastDate in forecastDictionary)
                {
                    foreach (var forecastTimePeriod in forecastDate.Value)
                    {
                        var isNewPeriod = !existingHalfHourForecastDictionary.ContainsKey(forecastDate.Key)
                            || !existingHalfHourForecastDictionary[forecastDate.Key].ContainsKey(forecastTimePeriod.Key);
                        var addUsageToDataTable = isNewPeriod
                            || existingHalfHourForecastDictionary[forecastDate.Key][forecastTimePeriod.Key] != forecastTimePeriod.Value;

                        if (addUsageToDataTable)
                        {
                            if (!isNewPeriod)
                            {
                                oldHalfHourForecastTuples.Add(new Tuple<long, long, decimal>(forecastDate.Key, forecastTimePeriod.Key, existingHalfHourForecastDictionary[forecastDate.Key][forecastTimePeriod.Key]));
                            }

                            var newHalfHourForecastTuple = new Tuple<long, long, decimal>(forecastDate.Key, forecastTimePeriod.Key, forecastTimePeriod.Value);
                            newHalfHourForecastTuples.Add(newHalfHourForecastTuple);
                        }
                    }
                }

                if (newHalfHourForecastTuples.Any())
                {
                    existingHalfHourForecasts = existingHalfHourForecasts.Except(oldHalfHourForecastTuples).ToList();
                    existingHalfHourForecasts.AddRange(newHalfHourForecastTuples);

                    //Insert into history and latest tables
                    _supplyMethods.CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "DateId", "TimePeriodId" }, newHalfHourForecastTuples, existingHalfHourForecasts);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createHalfHourForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createHalfHourForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing five minute forecast
            existingHalfHourForecasts = _supplyMethods.ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "DateId", "TimePeriodId");
            existingHalfHourForecastDictionary = existingHalfHourForecasts.Select(f => f.Item1).Distinct()
                .ToDictionary(
                    d => d,
                    d => existingHalfHourForecasts.Where(f => f.Item1 == d).ToDictionary(
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