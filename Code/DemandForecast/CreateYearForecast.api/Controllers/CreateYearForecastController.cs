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

namespace CreateYearForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateYearForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateYearForecastController> _logger;
        private readonly Int64 createYearForecastAPIId;
        private string granularityCode = "Year";
        private List<Tuple<long, decimal>> existingYearForecasts;
        private Dictionary<long, decimal> existingYearForecastDictionary;
        private Dictionary<long, decimal> forecastDictionary;
        private List<long> forecastYearIds;
        private Dictionary<long, List<long>> yearToDateDictionary;
        private readonly string hostEnvironment;
        #endregion

        public CreateYearForecastController(ILogger<CreateYearForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateYearForecastAPI, password);
            createYearForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateYearForecastAPI);
        }

        [HttpPost]
        [Route("CreateYearForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(createYearForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateYearForecast/Create")]
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
                    createYearForecastAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateYearForecastAPI, createYearForecastAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createYearForecastAPIId);

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
                    new Methods.Supply().CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "YearId" }, newYearForecastTuples.ToList(), existingYearForecasts);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createYearForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createYearForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing Year forecast
            existingYearForecasts = new Methods.Supply().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "YearId");
            existingYearForecastDictionary = existingYearForecasts.ToDictionary(
                d => d.Item1,
                d => d.Item2
            );
        }

        private void GetForecastDictionary(string meterType, long meterId)
        {
            var supplyMethods = new Methods.Supply();

            //Get latest loaded usage
            var latestLoadedUsage = supplyMethods.LoadedUsageLatest_GetList(meterType, meterId);

            //Get Date to Year mappings
            var dateToYearMappings = new Methods.Mapping().DateToYear_GetList();
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