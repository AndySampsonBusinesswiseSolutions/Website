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
using Microsoft.Extensions.Configuration;

namespace CreateMonthForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateMonthForecastController : ControllerBase
    {
        private readonly ILogger<CreateMonthForecastController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Int64 createMonthForecastAPIId;
        private string granularityCode = "Month";
        private List<Tuple<long, long, decimal>> existingMonthForecasts;
        private Dictionary<long, Dictionary<long, decimal>> existingMonthForecastDictionary;
        private Dictionary<long, decimal> forecastDictionary;
        private List<long> forecastYearIds;
        private Dictionary<long, List<long>> monthToDateDictionary;
        private Dictionary<long, List<long>> yearToDateDictionary;
        private readonly string hostEnvironment;

        public CreateMonthForecastController(ILogger<CreateMonthForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.CreateMonthForecastAPI, password);
            createMonthForecastAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateMonthForecastAPI);
        }

        [HttpPost]
        [Route("CreateMonthForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(createMonthForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateMonthForecast/Create")]
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
                    createMonthForecastAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CreateMonthForecastAPI, createMonthForecastAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createMonthForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                GetForecastDictionary(meterType, meterId);
                GetExistingForecast(meterType, meterId);

                var newMonthForecastTuples = new List<Tuple<long, long, decimal>>();
                var oldMonthForecastTuples = new List<Tuple<long, long, decimal>>();

                foreach (var forecastYearId in forecastYearIds)
                {
                    var dateIdsForYearId = yearToDateDictionary[forecastYearId];

                    //Get Forecast by Month
                    var forecastMonthIds = monthToDateDictionary.Where(w => w.Value.Any(wv => dateIdsForYearId.Contains(wv)))
                        .Select(w => w.Key).Distinct().ToList();

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

                            var newMonthForecastTuple = new Tuple<long, long, decimal>(forecastYearId, forecastMonthId, forecast);
                            newMonthForecastTuples.Add(newMonthForecastTuple);
                        }
                    }
                }

                if (newMonthForecastTuples.Any())
                {
                    existingMonthForecasts = existingMonthForecasts.Except(oldMonthForecastTuples).ToList();
                    existingMonthForecasts.AddRange(newMonthForecastTuples);

                    //Insert into history and latest tables
                    _supplyMethods.CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "YearId", "MonthId" }, newMonthForecastTuples, existingMonthForecasts);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createMonthForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createMonthForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing month forecast
            existingMonthForecasts = _supplyMethods.ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "YearId", "MonthId");
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
            //Get latest loaded usage
            var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);

            //Get Date to Month mappings
            var dateToMonthMappings = _mappingMethods.DateToMonth_GetList();
            monthToDateDictionary = dateToMonthMappings.Select(d => d.Field<long>("MonthId")).Distinct()
                .ToDictionary(
                    w => w,
                    w => dateToMonthMappings.Where(d => d.Field<long>("MonthId") == w).Select(d => d.Field<long>("DateId")).ToList()
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