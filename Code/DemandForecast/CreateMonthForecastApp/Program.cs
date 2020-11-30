using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CreateMonthForecastApp
{
    class Program
    {
        private static string granularityCode = "Month";
        private static List<Tuple<long, long, decimal>> existingMonthForecasts;
        private static Dictionary<long, Dictionary<long, decimal>> existingMonthForecastDictionary;
        private static Dictionary<long, decimal> forecastDictionary;
        private static List<long> forecastYearIds;
        private static Dictionary<long, List<long>> monthToDateDictionary;
        private static Dictionary<long, List<long>> yearToDateDictionary;

        static void Main(string[] args)
        {
            try
            {
                var hostEnvironment = "Development";
                var password = "9JQtHgkA2CcnaE67";

                var systemMethods = new Methods.SystemSchema();

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateMonthForecastAPI, password);
                var createMonthForecastAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateMonthForecastAPI);

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
                    createMonthForecastAPIId);

                // if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateMonthForecastAPI, createMonthForecastAPIId, hostEnvironment, jsonObject))
                // {
                //     return;
                // }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createMonthForecastAPIId);

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

                var newMonthForecastTuples = new ConcurrentBag<Tuple<long, long, decimal>>();
                var oldMonthForecastTuples = new ConcurrentBag<Tuple<long, long, decimal>>();

                Parallel.ForEach(forecastYearIds, new ParallelOptions{MaxDegreeOfParallelism = 5}, forecastYearId => {
                    var dateIdsForYearId = yearToDateDictionary[forecastYearId];

                    //Get Forecast by Month
                    var forecastMonthIds = monthToDateDictionary.Where(m => m.Value.Any(mv => dateIdsForYearId.Contains(mv)))
                        .Select(m => m.Key).Distinct().ToList();

                    foreach (var forecastMonthId in forecastMonthIds)
                    {
                        var dateIdsForYearIdMonthId = monthToDateDictionary[forecastMonthId].Intersect(dateIdsForYearId);

                        var forecast = forecastDictionary.Where(f => dateIdsForYearIdMonthId.Contains(f.Key))
                            .Sum(f => f.Value);

                        var isNewPeriod = !existingMonthForecastDictionary.ContainsKey(forecastYearId)
                            || !existingMonthForecastDictionary[forecastYearId].ContainsKey(forecastMonthId);
                        var addUsageToDataTable = isNewPeriod
                            || existingMonthForecastDictionary[forecastYearId][forecastMonthId] != Math.Round(forecast, 10);

                        if (addUsageToDataTable)
                        {
                            if (!isNewPeriod)
                            {
                                oldMonthForecastTuples.Add(new Tuple<long, long, decimal>(forecastYearId, forecastMonthId, existingMonthForecastDictionary[forecastYearId][forecastMonthId]));
                            }

                            newMonthForecastTuples.Add(new Tuple<long, long, decimal>(forecastYearId, forecastMonthId, forecast));
                        }
                    }
                });

                if (newMonthForecastTuples.Any())
                {
                    existingMonthForecasts = existingMonthForecasts.Except(oldMonthForecastTuples).ToList();
                    existingMonthForecasts.AddRange(newMonthForecastTuples);

                    //Insert into history and latest tables
                    new Methods.SupplySchema().CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "YearId", "MonthId" }, newMonthForecastTuples.ToList(), existingMonthForecasts);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createMonthForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createMonthForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private static void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing month forecast
            existingMonthForecasts = new Methods.SupplySchema().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "YearId", "MonthId");
            existingMonthForecastDictionary = existingMonthForecasts.Select(f => f.Item1).Distinct()
                .ToDictionary(
                    d => d,
                    d => existingMonthForecasts.Where(f => f.Item1 == d).ToDictionary(
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

            //Get Date to Month mappings
            var dateToMonthMappings = mappingMethods.DateToMonth_GetList();
            monthToDateDictionary = dateToMonthMappings.Select(d => d.MonthId).Distinct()
                .ToDictionary(
                    m => m,
                    m => dateToMonthMappings.Where(d => d.MonthId == m).Select(d => d.DateId).ToList()
                );

            //Get Date to Year mappings
            var dateToYearMappings = mappingMethods.DateToYear_GetList();
            yearToDateDictionary = dateToYearMappings.Select(d => d.YearId).Distinct()
                .ToDictionary(
                    y => y,
                    y => dateToYearMappings.Where(d => d.YearId == y).Select(d => d.DateId).ToList()
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
