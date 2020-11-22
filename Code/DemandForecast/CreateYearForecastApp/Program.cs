using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CreateYearForecastApp
{
    class Program
    {
        private static string granularityCode = "Year";
        private static List<Tuple<long, decimal>> existingYearForecasts;
        private static Dictionary<long, decimal> existingYearForecastDictionary;
        private static Dictionary<long, decimal> forecastDictionary;
        private static List<long> forecastYearIds;
        private static Dictionary<long, List<long>> yearToDateDictionary;

        static void Main(string[] args)
        {
            try
            {
                var hostEnvironment = "Development";
                var password = "YgK7auZuW5LnxKXB";

                var systemMethods = new Methods.SystemSchema();
                
                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateYearForecastAPI, password);
                var createYearForecastAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateYearForecastAPI);

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
                    createYearForecastAPIId);

                // if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateYearForecastAPI, createYearForecastAPIId, hostEnvironment, jsonObject))
                // {
                //     return;
                // }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createYearForecastAPIId);

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

                var newYearForecastTuples = new ConcurrentBag<Tuple<long, decimal>>();
                var oldYearForecastTuples = new ConcurrentBag<Tuple<long, decimal>>();

                Parallel.ForEach(forecastYearIds, new ParallelOptions{MaxDegreeOfParallelism = 5}, forecastYearId => {
                    var dateIdsForYearId = yearToDateDictionary[forecastYearId];

                    var forecast = forecastDictionary.Where(f => dateIdsForYearId.Contains(f.Key))
                        .Sum(f => f.Value);

                    var addUsageToDataTable = !existingYearForecastDictionary.ContainsKey(forecastYearId)
                        || existingYearForecastDictionary[forecastYearId] != Math.Round(forecast, 10);

                    if(addUsageToDataTable)
                    {
                        if(existingYearForecastDictionary.ContainsKey(forecastYearId))
                        {
                            oldYearForecastTuples.Add(new Tuple<long, decimal>(forecastYearId, existingYearForecastDictionary[forecastYearId]));
                        }

                        newYearForecastTuples.Add(new Tuple<long, decimal>(forecastYearId, forecast));
                    }
                });

                if (newYearForecastTuples.Any())
                {
                    existingYearForecasts = existingYearForecasts.Except(oldYearForecastTuples).ToList();
                    existingYearForecasts.AddRange(newYearForecastTuples);

                    //Insert into history and latest tables
                    new Methods.SupplySchema().CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "YearId" }, newYearForecastTuples.ToList(), existingYearForecasts);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createYearForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createYearForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private static void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing Year forecast
            existingYearForecasts = new Methods.SupplySchema().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "YearId");
            existingYearForecastDictionary = existingYearForecasts.ToDictionary(
                d => d.Item1,
                d => d.Item2
            );
        }

        private static void GetForecastDictionary(string meterType, long meterId)
        {
            var supplyMethods = new Methods.SupplySchema();

            //Get latest loaded usage
            var latestLoadedUsage = supplyMethods.LoadedUsageLatest_GetList(meterType, meterId);

            //Get Date to Year mappings
            var dateToYearMappings = new Methods.MappingSchema().DateToYear_GetList();
            yearToDateDictionary = dateToYearMappings.Select(d => d.YearId).Distinct()
                .ToDictionary(
                    y => y,
                    y => dateToYearMappings.Where(d => d.YearId == y).Select(d => d.DateId).ToList()
                );

            //Set up forecast dictionary
            var futureDateToUsageDateDictionary = supplyMethods.DateMapping_GetLatestDictionary(meterType, meterId);
            forecastDictionary = new Dictionary<long, decimal>(
                futureDateToUsageDateDictionary.ToDictionary(
                    f => f.Key, 
                    f => new decimal())
                );

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