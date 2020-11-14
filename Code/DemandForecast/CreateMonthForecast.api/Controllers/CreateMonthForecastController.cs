using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CreateMonthForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateMonthForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateMonthForecastController> _logger;
        private readonly Int64 createMonthForecastAPIId;
        private string granularityCode = "Month";
        private List<Tuple<long, long, decimal>> existingMonthForecasts;
        private Dictionary<long, Dictionary<long, decimal>> existingMonthForecastDictionary;
        private Dictionary<long, decimal> forecastDictionary;
        private List<long> forecastYearIds;
        private Dictionary<long, List<long>> monthToDateDictionary;
        private Dictionary<long, List<long>> yearToDateDictionary;
        private readonly string hostEnvironment;
        #endregion

        public CreateMonthForecastController(ILogger<CreateMonthForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateMonthForecastAPI, password);
            createMonthForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateMonthForecastAPI);
        }

        [HttpPost]
        [Route("CreateMonthForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(createMonthForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateMonthForecast/Create")]
        public void Create([FromBody] object data)
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
                    createMonthForecastAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateMonthForecastAPI, createMonthForecastAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createMonthForecastAPIId);

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
                    new Methods.Supply().CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "YearId", "MonthId" }, newMonthForecastTuples.ToList(), existingMonthForecasts);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createMonthForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createMonthForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing month forecast
            existingMonthForecasts = new Methods.Supply().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "YearId", "MonthId");
            existingMonthForecastDictionary = existingMonthForecasts.Select(f => f.Item1).Distinct()
                .ToDictionary(
                    d => d,
                    d => existingMonthForecasts.Where(f => f.Item1 == d).ToDictionary(
                        t => t.Item2,
                        t => t.Item3
                    )
            );
        }

        private void GetForecastDictionary(string meterType, long meterId)
        {
            var supplyMethods = new Methods.Supply();
            var mappingMethods = new Methods.Mapping();

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