using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace CreateHalfHourForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateHalfHourForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateHalfHourForecastController> _logger;
        private readonly Int64 createHalfHourForecastAPIId;
        private readonly string granularityCode = "HalfHour";
        private List<Tuple<long, long, decimal>> existingHalfHourForecasts;
        private Dictionary<long, Dictionary<long, decimal>> existingHalfHourForecastDictionary;
        private ConcurrentDictionary<long, Dictionary<long, decimal>> forecastDictionary;
        private readonly string hostEnvironment;
        #endregion

        public CreateHalfHourForecastController(ILogger<CreateHalfHourForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateHalfHourForecastAPI, password);
            createHalfHourForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateHalfHourForecastAPI);
        }

        [HttpPost]
        [Route("CreateHalfHourForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(createHalfHourForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateHalfHourForecast/Create")]
        public void Create([FromBody] object data)
        {
            if(new Enums.SystemSchema.API.GUID().RunConsoleApps)
            {
                var jsonObject = JObject.Parse(data.ToString());
                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateHalfHourForecastAPI, createHalfHourForecastAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateHalfHourForecastApp\bin\Debug\netcoreapp3.1\CreateHalfHourForecastApp.exe";
                ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
                startInfo.Arguments = JsonConvert.SerializeObject(data.ToString());
                System.Diagnostics.Process.Start(startInfo);
            }
            else
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
                        createHalfHourForecastAPIId);

                    if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateHalfHourForecastAPI, createHalfHourForecastAPIId, hostEnvironment, jsonObject))
                    {
                        return;
                    }

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createHalfHourForecastAPIId);

                    //Get MeterType
                    var meterType = jsonObject[new Enums.SystemSchema.API.RequiredDataKey().MeterType].ToString();

                    //Get MeterId
                    var meterId = new Methods.Customer().GetMeterIdByMeterType(meterType, jsonObject);

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

                    var newHalfHourForecastTuples = new ConcurrentBag<Tuple<long, long, decimal>>();
                    var oldHalfHourForecastTuples = new ConcurrentBag<Tuple<long, long, decimal>>();

                    Parallel.ForEach(forecastDictionary, new ParallelOptions{MaxDegreeOfParallelism = 5}, forecastDate => {
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
                    });

                    if (newHalfHourForecastTuples.Any())
                    {
                        existingHalfHourForecasts = existingHalfHourForecasts.Except(oldHalfHourForecastTuples).ToList();
                        existingHalfHourForecasts.AddRange(newHalfHourForecastTuples);

                        //Insert into history and latest tables
                        new Methods.Supply().CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "DateId", "TimePeriodId" }, newHalfHourForecastTuples.ToList(), existingHalfHourForecasts);
                    }

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createHalfHourForecastAPIId, false, null);
                }
                catch (Exception error)
                {
                    var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createHalfHourForecastAPIId, true, $"System Error Id {errorId}");
                }
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing five minute forecast
            existingHalfHourForecasts = new Methods.Supply().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "DateId", "TimePeriodId");
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
            var supplyMethods = new Methods.Supply();
            var mappingMethods = new Methods.Mapping();
            var informationMethods = new Methods.Information();

            //Get latest loaded usage
            var latestLoadedUsage = supplyMethods.LoadedUsageLatest_GetList(meterType, meterId);

            //Get GranularityId
            var granularityCodeGranularityAttributeId = informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(new Enums.InformationSchema.Granularity.Attribute().GranularityCode);
            var granularityId = informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(granularityCodeGranularityAttributeId, granularityCode);

            //Get required time periods
            var nonStandardGranularityToTimePeriodDataRows = mappingMethods.GranularityToTimePeriod_NonStandardDate_GetListByGranularityId(granularityId);
            var standardGranularityToTimePeriodDataRows = mappingMethods.GranularityToTimePeriod_StandardDate_GetListByGranularityId(granularityId);
            var standardGranularityToTimePeriods = standardGranularityToTimePeriodDataRows.Select(d => d.TimePeriodId).ToList();

            //Set up forecast dictionary
            var futureDateToUsageDateDictionary = supplyMethods.DateMapping_GetLatestDictionary(meterType, meterId);

            var timePeriodToTimePeriodDictionary = mappingMethods.TimePeriodToTimePeriod_GetDictionary();
            var nonStandardGranularityDates = nonStandardGranularityToTimePeriodDataRows.Select(d => d.DateId).Distinct()
                .ToDictionary(
                    d => d,
                    d => nonStandardGranularityToTimePeriodDataRows.Where(n => n.DateId == d).Select(d => d.TimePeriodId).ToList()
                );

            forecastDictionary = new ConcurrentDictionary<long, Dictionary<long, decimal>>(
                futureDateToUsageDateDictionary.ToDictionary(
                    f => f.Key,
                    f => (nonStandardGranularityDates.ContainsKey(f.Key)
                            ? nonStandardGranularityDates[f.Key]
                            : standardGranularityToTimePeriods).ToDictionary(t => t, t => new decimal())
                )
            );

            var forecastFoundDictionary = new ConcurrentDictionary<long, Dictionary<long, bool>>(
                futureDateToUsageDateDictionary.ToDictionary(
                    f => f.Key,
                    f => (nonStandardGranularityDates.ContainsKey(f.Key)
                            ? nonStandardGranularityDates[f.Key]
                            : standardGranularityToTimePeriods).ToDictionary(t => t, t => new bool())
                )
            );

            var timePeriodToMappedTimePeriodDictionary = new ConcurrentDictionary<long, Dictionary<long, List<long>>>(
                timePeriodToTimePeriodDictionary.ToDictionary(
                    t => t.Key,
                    t => t.Value.Select(m => m).Distinct()
                        .ToDictionary(m => m, m => timePeriodToTimePeriodDictionary.Where(t => t.Value.Contains(m)).Select(t => t.Key).ToList())
                        .OrderBy(m => m.Value.Count())
                        .ToDictionary(o => o.Key, o => o.Value)
                )
            );

            //Loop through future date ids
            Parallel.ForEach(forecastDictionary, new ParallelOptions{MaxDegreeOfParallelism = 5}, forecast => {
                //Get usage date id
                var usageDateId = futureDateToUsageDateDictionary[forecast.Key];

                //Get usage for date
                var usageForDateList = latestLoadedUsage.Where(u => u.DateId == usageDateId).ToList();
                var timePeriodIds = forecast.Value.Keys.ToList();
                var forecastFound = forecastFoundDictionary[forecast.Key];

                foreach (var timePeriodId in timePeriodIds)
                {
                    if(forecastFound[timePeriodId])
                    {
                        continue;
                    }
                        
                    //Get usage for time period
                    var usageForTimePeriodList = usageForDateList.Where(u => u.TimePeriodId == timePeriodId).ToList();

                    if (usageForTimePeriodList.Any())
                    {
                        supplyMethods.SetForecastValue(forecast.Value, forecastFound, timePeriodId, usageForTimePeriodList.First().Usage);
                    }
                    else
                    {
                        var mappedTimePeriodDictionary = timePeriodToMappedTimePeriodDictionary[timePeriodId]
                            .First(t => usageForDateList.Any(u => u.TimePeriodId == t.Key));

                        usageForTimePeriodList = usageForDateList.Where(u => u.TimePeriodId == mappedTimePeriodDictionary.Key).ToList();

                        //Get usage based on usage type priority
                        var mappedUsage = usageForTimePeriodList.First().Usage;
                        var mappedTimePeriodIdsWithUsageList = mappedTimePeriodDictionary.Value.Where(v => usageForDateList.Any(u => u.TimePeriodId == v)).ToList();
                        var missingTimePeriodIds = mappedTimePeriodDictionary.Value.Except(mappedTimePeriodIdsWithUsageList);

                        foreach (var mappedtimePeriodId in mappedTimePeriodIdsWithUsageList)
                        {
                            //Get usage for time period
                            usageForTimePeriodList = usageForDateList.Where(u => u.TimePeriodId == mappedtimePeriodId).ToList();

                            supplyMethods.SetForecastValue(forecast.Value, forecastFound, timePeriodId, usageForTimePeriodList.First().Usage);
                        }

                        if (missingTimePeriodIds.Any())
                        {
                            var timePeriodUsage = mappedTimePeriodDictionary.Value
                                .Where(t => forecastDictionary[forecast.Key].ContainsKey(t))
                                .Sum(t => forecastDictionary[forecast.Key][t]);
                            var missingTimePeriodUsage = (mappedUsage - timePeriodUsage) / missingTimePeriodIds.Count();

                            foreach (var missingTimePeriodId in missingTimePeriodIds)
                            {
                                supplyMethods.SetForecastValue(forecast.Value, forecastFound, missingTimePeriodId, missingTimePeriodUsage);
                            }
                        }
                    }
                }
            });
        }
    }
}