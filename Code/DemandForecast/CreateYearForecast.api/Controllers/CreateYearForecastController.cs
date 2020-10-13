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

namespace CreateYearForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateYearForecastController : ControllerBase
    {
        private readonly ILogger<CreateYearForecastController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Int64 createYearForecastAPIId;
        private string granularityCode = "Year";
        private List<Tuple<long, decimal>> existingYearForecasts;
        private Dictionary<long, decimal> existingYearForecastDictionary;
        private Dictionary<long, decimal> forecastDictionary;
        private List<long> forecastYearIds;
        private Dictionary<long, List<long>> yearToDateDictionary;

        public CreateYearForecastController(ILogger<CreateYearForecastController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CreateYearForecastAPI, _systemAPIPasswordEnums.CreateYearForecastAPI);
            createYearForecastAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateYearForecastAPI);
        }

        [HttpPost]
        [Route("CreateYearForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(createYearForecastAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateYearForecast/Create")]
        public void Create([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
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
                    createYearForecastAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CreateYearForecastAPI, createYearForecastAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createYearForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                GetForecastDictionary(meterType, meterId);
                GetExistingForecast(meterType, meterId);

                var newYearForecastTuples = new List<Tuple<long, decimal>>();
                var oldYearForecastTuples = new List<Tuple<long, decimal>>();

                foreach(var forecastYearId in forecastYearIds)
                {
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

                        var newYearForecastTuple = new Tuple<long, decimal>(forecastYearId, forecast);
                        newYearForecastTuples.Add(newYearForecastTuple);
                    }
                }

                if (newYearForecastTuples.Any())
                {
                    existingYearForecasts = existingYearForecasts.Except(oldYearForecastTuples).ToList();
                    existingYearForecasts.AddRange(newYearForecastTuples);

                    //Insert into history and latest tables
                    _supplyMethods.CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "YearId" }, newYearForecastTuples, existingYearForecasts);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createYearForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createYearForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing Year forecast
            existingYearForecasts = _supplyMethods.ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "YearId");
            existingYearForecastDictionary = existingYearForecasts.ToDictionary(
                d => d.Item1,
                d => d.Item2
            );
        }

        private void GetForecastDictionary(string meterType, long meterId)
        {
            //Get latest loaded usage
            var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);

            //Get Date to Year mappings
            var dateToYearMappings = _mappingMethods.DateToYear_GetList();
            yearToDateDictionary = dateToYearMappings.Select(d => d.Field<long>("YearId")).Distinct()
                .ToDictionary(
                    w => w,
                    w => dateToYearMappings.Where(d => d.Field<long>("YearId") == w).Select(d => d.Field<long>("DateId")).ToList()
                );

            //Set up forecast dictionary
            var futureDateToUsageDateDictionary = _supplyMethods.DateMapping_GetLatestDictionary(meterType, meterId);
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
                    .Where(u => u.Field<long>("DateId") == futureDateToUsageDateDictionary[futureDateId])
                    .Sum(u => u.Field<decimal>("Usage"));
            }

            //Get Forecast by Year
            forecastYearIds = yearToDateDictionary.Where(y => y.Value.Any(yv => futureDateToUsageDateDictionary.ContainsKey(yv)))
                .Select(y => y.Key).Distinct().ToList();
        }
    }
}