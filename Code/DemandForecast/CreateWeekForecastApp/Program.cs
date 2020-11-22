using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CreateWeekForecastApp
{
    class Program
    {
        private static string granularityCode = "Week";
        private static List<Tuple<long, long, decimal>> existingWeekForecasts;
        private static Dictionary<long, Dictionary<long, decimal>> existingWeekForecastDictionary;
        private static Dictionary<long, decimal> forecastDictionary;
        private static List<long> forecastYearIds;
        private static Dictionary<long, List<long>> weekToDateDictionary;
        private static Dictionary<long, List<long>> yearToDateDictionary;

        static void Main(string[] args)
        {
            try
            {
                var hostEnvironment = "Development";
                var password = "95hU29J4PpaeQmFV";

                var systemMethods = new Methods.SystemSchema();

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateWeekForecastAPI, password);
                var createWeekForecastAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateWeekForecastAPI);

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    createWeekForecastAPIId);

                // if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateWeekForecastAPI, createWeekForecastAPIId, hostEnvironment, jsonObject))
                // {
                //     return;
                // }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createWeekForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[new Enums.SystemSchema.API.RequiredDataKey().MeterType].ToString();

                //Get MeterId
                var meterId = new Methods.CustomerSchema().GetMeterIdByMeterType(meterType, jsonObject);

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

                var newWeekForecastTuples = new ConcurrentBag<Tuple<long, long, decimal>>();
                var oldWeekForecastTuples = new ConcurrentBag<Tuple<long, long, decimal>>();

                Parallel.ForEach(forecastYearIds, new ParallelOptions{MaxDegreeOfParallelism = 5}, forecastYearId => {
                    var dateIdsForYearId = yearToDateDictionary[forecastYearId];

                    //Get Forecast by Week
                    var forecastWeekIds = weekToDateDictionary.Where(w => w.Value.Any(wv => dateIdsForYearId.Contains(wv)))
                        .Select(w => w.Key).Distinct().ToList();

                    foreach (var forecastWeekId in forecastWeekIds)
                    {
                        var dateIdsForYearIdWeekId = weekToDateDictionary[forecastWeekId].Intersect(dateIdsForYearId);

                        var forecast = forecastDictionary.Where(f => dateIdsForYearIdWeekId.Contains(f.Key))
                            .Sum(f => f.Value);

                        var isNewPeriod = !existingWeekForecastDictionary.ContainsKey(forecastYearId)
                            || !existingWeekForecastDictionary[forecastYearId].ContainsKey(forecastWeekId);
                        var addUsageToDataTable = isNewPeriod
                            || existingWeekForecastDictionary[forecastYearId][forecastWeekId] != Math.Round(forecast, 10);

                        if (addUsageToDataTable)
                        {
                            if (!isNewPeriod)
                            {
                                oldWeekForecastTuples.Add(new Tuple<long, long, decimal>(forecastYearId, forecastWeekId, existingWeekForecastDictionary[forecastYearId][forecastWeekId]));
                            }

                            newWeekForecastTuples.Add(new Tuple<long, long, decimal>(forecastYearId, forecastWeekId, forecast));
                        }
                    }
                });

                if (newWeekForecastTuples.Any())
                {
                    existingWeekForecasts = existingWeekForecasts.Except(oldWeekForecastTuples).ToList();
                    existingWeekForecasts.AddRange(newWeekForecastTuples);

                    //Insert into history and latest tables
                    new Methods.SupplySchema().CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "YearId", "WeekId" }, newWeekForecastTuples.ToList(), existingWeekForecasts);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createWeekForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createWeekForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private static void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing Week forecast
            existingWeekForecasts = new Methods.SupplySchema().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "YearId", "WeekId");
            existingWeekForecastDictionary = existingWeekForecasts.Select(f => f.Item1).Distinct()
                .ToDictionary(
                    d => d,
                    d => existingWeekForecasts.Where(f => f.Item1 == d).ToDictionary(
                        t => t.Item2,
                        t => t.Item3
                    )
            );
        }

        private static void GetForecastDictionary(string meterType, long meterId)
        {
            var supplyMethods = new Methods.SupplySchema();
            var mappingMethods = new Methods.MappingSchema();

            //Get latest loaded usage
            var latestLoadedUsage = supplyMethods.LoadedUsageLatest_GetList(meterType, meterId);

            //Get Date to Week mappings
            var dateToWeekMappings = mappingMethods.DateToWeek_GetList();
            weekToDateDictionary = dateToWeekMappings.Select(d => d.WeekId).Distinct()
                .ToDictionary(
                    w => w,
                    w => dateToWeekMappings.Where(d => d.WeekId == w).Select(d => d.DateId).ToList()
                );

            //Get Date to Year mappings
            var dateToYearMappings = mappingMethods.DateToYear_GetList();
            yearToDateDictionary = dateToYearMappings.Select(d => d.YearId).Distinct()
                .ToDictionary(
                    w => w,
                    w => dateToYearMappings.Where(d => d.YearId == w).Select(d => d.DateId).ToList()
                );

            //Set up forecast dictionary
            var futureDateToUsageDateDictionary = supplyMethods.DateMapping_GetLatestDictionary(meterType, meterId);
            forecastDictionary = new Dictionary<long, decimal>(futureDateToUsageDateDictionary.ToDictionary(f => f.Key, f => new decimal()));

            //Loop through future date ids
            var forecastDictionaryKeys = forecastDictionary.Keys.ToList();
            foreach (var futureDateId in forecastDictionaryKeys)
            {
                forecastDictionary[futureDateId] = latestLoadedUsage
                    .Where(u => u.DateId == futureDateToUsageDateDictionary[futureDateId])
                    .Sum(u => u.Usage);
            }

            //Get Forecast by Year
            forecastYearIds = yearToDateDictionary.Where(y => y.Value.Any(yv => futureDateToUsageDateDictionary.ContainsKey(yv)))
                .Select(y => y.Key).Distinct().ToList();
        }
    }
}