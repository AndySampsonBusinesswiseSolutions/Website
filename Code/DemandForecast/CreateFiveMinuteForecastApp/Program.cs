using enums;
using MethodLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateFiveMinuteForecastApp
{
    class Program
    {
        private static readonly string granularityCode = "FiveMinute";
        private static List<Tuple<long, long, decimal>> existingFiveMinuteForecasts;
        private static Dictionary<long, Dictionary<long, decimal>> existingFiveMinuteForecastDictionary;
        private static ConcurrentDictionary<long, Dictionary<long, decimal>> forecastDictionary;

        static void Main(string[] args)
        {
            try
            {
                var hostEnvironment = "Development";
                var password = "zTqVewH8Zrgye4Vd";

                var systemMethods = new Methods.System();

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateFiveMinuteForecastAPI, password);
                var createFiveMinuteForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateFiveMinuteForecastAPI);

                //Get base variables
                var createdByUserId = new Methods.Administration.User().GetSystemUserId();
                var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    createFiveMinuteForecastAPIId);

                // if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateFiveMinuteForecastAPI, createFiveMinuteForecastAPIId, hostEnvironment, jsonObject))
                // {
                //     return;
                // }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createFiveMinuteForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[new Enums.SystemSchema.API.RequiredDataKey().MeterType].ToString();

                //Get MeterId
                var meterId = new Methods.Customer().GetMeterIdByMeterType(meterType, jsonObject);

                Parallel.ForEach(new List<bool>{true, false}, getForecastDictionary => {
                    if(getForecastDictionary)
                    {
                        GetForecastDictionary(meterType, meterId);
                    }
                    else
                    {
                        GetExistingForecast(meterType, meterId);
                    }
                });

                var newFiveMinuteForecastTuples = new ConcurrentBag<Tuple<long, long, decimal>>();
                var oldFiveMinuteForecastTuples = new ConcurrentBag<Tuple<long, long, decimal>>();

                Parallel.ForEach(forecastDictionary, new ParallelOptions{MaxDegreeOfParallelism = 5}, forecastDate => {
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

                            newFiveMinuteForecastTuples.Add(new Tuple<long, long, decimal>(forecastDate.Key, forecastTimePeriod.Key, forecastTimePeriod.Value));
                        }
                    }
                });

                if (newFiveMinuteForecastTuples.Any())
                {
                    existingFiveMinuteForecasts = existingFiveMinuteForecasts.Except(oldFiveMinuteForecastTuples).ToList();
                    existingFiveMinuteForecasts.AddRange(newFiveMinuteForecastTuples);

                    //Insert into history and latest tables
                    new Methods.Supply().CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "DateId", "TimePeriodId" }, newFiveMinuteForecastTuples.ToList(), existingFiveMinuteForecasts);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createFiveMinuteForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = new Methods.System().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createFiveMinuteForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private static void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing five minute forecast
            existingFiveMinuteForecasts = new Methods.Supply().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "DateId", "TimePeriodId");
            existingFiveMinuteForecastDictionary = existingFiveMinuteForecasts.Select(f => f.Item1).Distinct()
                .ToDictionary(
                    d => d,
                    d => existingFiveMinuteForecasts.Where(f => f.Item1 == d).ToDictionary(
                        t => t.Item2,
                        t => t.Item3
                    )
            );
        }

        private static void GetForecastDictionary(string meterType, long meterId)
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
            var nonStandardGranularityToTimePeriodEntities = mappingMethods.GranularityToTimePeriod_NonStandardDate_GetListByGranularityId(granularityId);
            var standardGranularityToTimePeriodEntities = mappingMethods.GranularityToTimePeriod_StandardDate_GetListByGranularityId(granularityId);
            var standardGranularityToTimePeriods = standardGranularityToTimePeriodEntities.Select(d => d.TimePeriodId).ToList();

            //Set up forecast dictionary
            var futureDateToUsageDateDictionary = supplyMethods.DateMapping_GetLatestDictionary(meterType, meterId);

            var timePeriodToTimePeriodDictionary = mappingMethods.TimePeriodToTimePeriod_GetDictionary();
            var nonStandardGranularityDates = nonStandardGranularityToTimePeriodEntities.Select(d => d.DateId).Distinct()
                .ToDictionary(
                    d => d,
                    d => nonStandardGranularityToTimePeriodEntities.Where(n => n.DateId == d).Select(d => d.TimePeriodId).ToList()
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
                    var usageForTimePeriodList = usageForDateList.Where(u => u.TimePeriodId== timePeriodId).ToList();

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