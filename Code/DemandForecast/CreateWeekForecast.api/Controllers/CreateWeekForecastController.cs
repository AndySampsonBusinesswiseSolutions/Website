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
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CreateWeekForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateWeekForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateWeekForecastController> _logger;
        private readonly Int64 createWeekForecastAPIId;
        private string granularityCode = "Week";
        private List<Tuple<long, long, decimal>> existingWeekForecasts;
        private Dictionary<long, Dictionary<long, decimal>> existingWeekForecastDictionary;
        private Dictionary<long, decimal> forecastDictionary;
        private List<long> forecastYearIds;
        private Dictionary<long, List<long>> weekToDateDictionary;
        private Dictionary<long, List<long>> yearToDateDictionary;
        private readonly string hostEnvironment;
        #endregion

        public CreateWeekForecastController(ILogger<CreateWeekForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateWeekForecastAPI, password);
            createWeekForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateWeekForecastAPI);
        }

        [HttpPost]
        [Route("CreateWeekForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(createWeekForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateWeekForecast/Create")]
        public void Create([FromBody] object data)
        {
            if(new Enums.SystemSchema.API.GUID().RunConsoleApps)
            {
                var jsonObject = JObject.Parse(data.ToString());
                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateWeekForecastAPI, createWeekForecastAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateWeekForecastApp\bin\Debug\netcoreapp3.1\CreateWeekForecastApp.exe";
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
                        createWeekForecastAPIId);

                    if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateWeekForecastAPI, createWeekForecastAPIId, hostEnvironment, jsonObject))
                    {
                        return;
                    }

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createWeekForecastAPIId);

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
                        new Methods.Supply().CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "YearId", "WeekId" }, newWeekForecastTuples.ToList(), existingWeekForecasts);
                    }

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createWeekForecastAPIId, false, null);
                }
                catch (Exception error)
                {
                    var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createWeekForecastAPIId, true, $"System Error Id {errorId}");
                }
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing Week forecast
            existingWeekForecasts = new Methods.Supply().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "YearId", "WeekId");
            existingWeekForecastDictionary = existingWeekForecasts.Select(f => f.Item1).Distinct()
                .ToDictionary(
                    d => d,
                    d => existingWeekForecasts.Where(f => f.Item1 == d).ToDictionary(
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