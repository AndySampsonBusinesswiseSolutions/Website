using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CreateDateForecastApp
{
    class Program
    {
        private readonly static string granularityCode = "Date";
        private static List<Tuple<long, decimal>> existingDateForecasts;
        private static Dictionary<long, decimal> existingDateForecastDictionary;
        private static Dictionary<long, decimal> forecastDictionary;

        static void Main(string[] args)
        {
            try
            {
                var hostEnvironment = "Development";
                var password = "YLtRVcMGf7UUALXU";

                var systemMethods = new Methods.System();

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateDateForecastAPI, password);
                var createDateForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateDateForecastAPI);

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
                    createDateForecastAPIId);

                // if (!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateDateForecastAPI, createDateForecastAPIId, hostEnvironment, jsonObject))
                // {
                //     return;
                // }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createDateForecastAPIId);

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

                var newDateForecastTuples = new ConcurrentBag<Tuple<long, decimal>>();
                var oldDateForecastTuples = new ConcurrentBag<Tuple<long, decimal>>();

                Parallel.ForEach(forecastDictionary, new ParallelOptions{MaxDegreeOfParallelism = 5}, forecastDate => {
                    var addUsageToDataTable = !existingDateForecastDictionary.ContainsKey(forecastDate.Key)
                        || existingDateForecastDictionary[forecastDate.Key] != forecastDate.Value;

                    if (addUsageToDataTable)
                    {
                        if (existingDateForecastDictionary.ContainsKey(forecastDate.Key))
                        {
                            oldDateForecastTuples.Add(new Tuple<long, decimal>(forecastDate.Key, existingDateForecastDictionary[forecastDate.Key]));
                        }

                        newDateForecastTuples.Add(new Tuple<long, decimal>(forecastDate.Key, forecastDate.Value));
                    }
                });

                if (newDateForecastTuples.Any())
                {
                    existingDateForecasts = existingDateForecasts.Except(oldDateForecastTuples).ToList();
                    existingDateForecasts.AddRange(newDateForecastTuples);

                    //Insert into history and latest tables
                    new Methods.Supply().CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "DateId" }, newDateForecastTuples.ToList(), existingDateForecasts);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDateForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = new Methods.System().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDateForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private static void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing date forecast
            existingDateForecasts = new Methods.Supply().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "DateId");
            existingDateForecastDictionary = existingDateForecasts.ToDictionary(
                    d => d.Item1,
                    d => d.Item2
            );
        }

        private static void GetForecastDictionary(string meterType, long meterId)
        {
            var supplyMethods = new Methods.Supply();

            //Get latest loaded usage
            var latestLoadedUsage = supplyMethods.LoadedUsageLatest_GetList(meterType, meterId);

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
        }
    }
}
