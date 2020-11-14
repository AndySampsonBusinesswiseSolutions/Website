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

namespace CreateQuarterForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateQuarterForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateQuarterForecastController> _logger;
        private readonly Int64 createQuarterForecastAPIId;
        private string granularityCode = "Quarter";
        private List<Tuple<long, long, decimal>> existingQuarterForecasts;
        private Dictionary<long, Dictionary<long, decimal>> existingQuarterForecastDictionary;
        private Dictionary<long, decimal> forecastDictionary;
        private List<long> forecastYearIds;
        private Dictionary<long, List<long>> quarterToDateDictionary;
        private Dictionary<long, List<long>> yearToDateDictionary;
        private readonly string hostEnvironment;
        #endregion

        public CreateQuarterForecastController(ILogger<CreateQuarterForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateQuarterForecastAPI, password);
            createQuarterForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateQuarterForecastAPI);
        }

        [HttpPost]
        [Route("CreateQuarterForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(createQuarterForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateQuarterForecast/Create")]
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
                    createQuarterForecastAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateQuarterForecastAPI, createQuarterForecastAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createQuarterForecastAPIId);

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

                var newQuarterForecastTuples = new ConcurrentBag<Tuple<long, long, decimal>>();
                var oldQuarterForecastTuples = new ConcurrentBag<Tuple<long, long, decimal>>();

                Parallel.ForEach(forecastYearIds, new ParallelOptions{MaxDegreeOfParallelism = 5}, forecastYearId => {
                    var dateIdsForYearId = yearToDateDictionary[forecastYearId];

                    //Get Forecast by Quarter
                    var forecastQuarterIds = quarterToDateDictionary.Where(q => q.Value.Any(qv => dateIdsForYearId.Contains(qv)))
                        .Select(q => q.Key).Distinct().ToList();

                    foreach (var forecastQuarterId in forecastQuarterIds)
                    {
                        var dateIdsForYearIdQuarterId = quarterToDateDictionary[forecastQuarterId].Intersect(dateIdsForYearId);

                        var forecast = forecastDictionary.Where(f => dateIdsForYearIdQuarterId.Contains(f.Key))
                            .Sum(f => f.Value);

                        var isNewPeriod = !existingQuarterForecastDictionary.ContainsKey(forecastYearId)
                            || !existingQuarterForecastDictionary[forecastYearId].ContainsKey(forecastQuarterId);
                        var addUsageToDataTable = isNewPeriod
                            || existingQuarterForecastDictionary[forecastYearId][forecastQuarterId] != Math.Round(forecast, 10);

                        if (addUsageToDataTable)
                        {
                            if (!isNewPeriod)
                            {
                                oldQuarterForecastTuples.Add(new Tuple<long, long, decimal>(forecastYearId, forecastQuarterId, existingQuarterForecastDictionary[forecastYearId][forecastQuarterId]));
                            }

                            newQuarterForecastTuples.Add(new Tuple<long, long, decimal>(forecastYearId, forecastQuarterId, forecast));
                        }
                    }
                });

                if (newQuarterForecastTuples.Any())
                {
                    existingQuarterForecasts = existingQuarterForecasts.Except(oldQuarterForecastTuples).ToList();
                    existingQuarterForecasts.AddRange(newQuarterForecastTuples);

                    //Insert into history and latest tables
                    new Methods.Supply().CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "YearId", "QuarterId" }, newQuarterForecastTuples.ToList(), existingQuarterForecasts);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createQuarterForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createQuarterForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing Quarter forecast
            existingQuarterForecasts = new Methods.Supply().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "YearId", "QuarterId");
            existingQuarterForecastDictionary = existingQuarterForecasts.Select(f => f.Item1).Distinct()
                .ToDictionary(
                    d => d,
                    d => existingQuarterForecasts.Where(f => f.Item1 == d).ToDictionary(
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

            //Get Date to Quarter mappings
            var dateToQuarterMappings = mappingMethods.DateToQuarter_GetList();
            quarterToDateDictionary = dateToQuarterMappings.Select(d => d.QuarterId).Distinct()
                .ToDictionary(
                    q => q,
                    q => dateToQuarterMappings.Where(d => d.QuarterId == q).Select(d => d.DateId).ToList()
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