using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace CreateWeekForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateWeekForecastController : ControllerBase
    {
        private readonly ILogger<CreateWeekForecastController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Int64 createWeekForecastAPIId;
        private string granularityCode = "Week";
        private List<Tuple<long, long, decimal>> existingWeekForecasts;
        private Dictionary<long, Dictionary<long, decimal>> existingWeekForecastDictionary;
        private Dictionary<long, decimal> forecastDictionary;
        private List<long> forecastYearIds;
        private Dictionary<long, List<long>> weekToDateDictionary;
        private Dictionary<long, List<long>> yearToDateDictionary;

        public CreateWeekForecastController(ILogger<CreateWeekForecastController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CreateWeekForecastAPI, _systemAPIPasswordEnums.CreateWeekForecastAPI);
            createWeekForecastAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateWeekForecastAPI);
        }

        [HttpPost]
        [Route("CreateWeekForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(createWeekForecastAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateWeekForecast/Create")]
        public void Create([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    createWeekForecastAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CreateWeekForecastAPI, createWeekForecastAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createWeekForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                GetForecastDictionary(meterType, meterId);
                GetExistingForecast(meterType, meterId);

                var newWeekForecastTuples = new List<Tuple<long, long, decimal>>();
                var oldWeekForecastTuples = new List<Tuple<long, long, decimal>>();

                //TODO: Work out how to speed this up
                foreach (var forecastYearId in forecastYearIds)
                {
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

                            var newWeekForecastTuple = new Tuple<long, long, decimal>(forecastYearId, forecastWeekId, forecast);
                            newWeekForecastTuples.Add(newWeekForecastTuple);
                        }
                    }
                }

                if (newWeekForecastTuples.Any())
                {
                    existingWeekForecasts = existingWeekForecasts.Except(oldWeekForecastTuples).ToList();
                    existingWeekForecasts.AddRange(newWeekForecastTuples);

                    //Insert into history and latest tables
                    _supplyMethods.CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "YearId", "WeekId" }, newWeekForecastTuples, existingWeekForecasts);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createWeekForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createWeekForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing Week forecast
            existingWeekForecasts = _supplyMethods.ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "YearId", "WeekId");
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
            //Get latest loaded usage
            var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);

            //Get Date to Week mappings
            var dateToWeekMappings = _mappingMethods.DateToWeek_GetList();
            weekToDateDictionary = dateToWeekMappings.Select(d => d.Field<long>("WeekId")).Distinct()
                .ToDictionary(
                    w => w,
                    w => dateToWeekMappings.Where(d => d.Field<long>("WeekId") == w).Select(d => d.Field<long>("DateId")).ToList()
                );

            //Get Date to Year mappings
            var dateToYearMappings = _mappingMethods.DateToYear_GetList();
            yearToDateDictionary = dateToYearMappings.Select(d => d.Field<long>("YearId")).Distinct()
                .ToDictionary(
                    w => w,
                    w => dateToYearMappings.Where(d => d.Field<long>("YearId") == w).Select(d => d.Field<long>("DateId")).ToList()
                );

            //Set up forecast dictionary
            var futureDateToUsageDateDictionary = _supplyMethods.DateMapping_GetLatestDictionary(meterType, meterId);
            forecastDictionary = new Dictionary<long, decimal>(futureDateToUsageDateDictionary.ToDictionary(f => f.Key, f => new decimal()));

            //Loop through future date ids
            var forecastDictionaryKeys = forecastDictionary.Keys.ToList();
            foreach (var futureDateId in forecastDictionaryKeys)
            {
                forecastDictionary[futureDateId] = latestLoadedUsage
                    .Where(u => u.Field<long>("DateId") == futureDateToUsageDateDictionary[futureDateId])
                    .Sum(u => u.Field<decimal>("Usage"));
            }

            //Get Forecast by Year
            forecastYearIds = yearToDateDictionary.Where(y => y.Value.Any(yv => futureDateToUsageDateDictionary.ContainsKey(yv)))
                .Select(y => y.Key).Distinct().ToList();
        }
    }
}