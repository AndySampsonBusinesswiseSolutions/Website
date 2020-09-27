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
using System.Collections.Concurrent;

namespace CreateWeekForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateWeekForecastController : ControllerBase
    {
        private readonly ILogger<CreateWeekForecastController> _logger;
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
        private readonly Int64 createWeekForecastAPIId;

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
                    createWeekForecastAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createWeekForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                //Get latest loaded usage
                var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);

                //Get Date to Week mappings
                var dateToWeekMappings = _mappingMethods.DateToWeek_GetList();
                
                //Get Date to Year mappings
                var dateToYearMappings = _mappingMethods.DateToYear_GetList();

                //Set up forecast dictionary
                var dateMappings = _supplyMethods.DateMapping_GetLatest(meterType, meterId);
                var futureDateToUsageDateDictionary = dateMappings.ToDictionary(
                    d => d.Field<long>("DateId"),
                    d => d.Field<long>("MappedDateId")
                );
                var forecastDictionary = new ConcurrentDictionary<long, decimal>(futureDateToUsageDateDictionary.ToDictionary(f => f.Key, f => new decimal()));
                var usageTypePriority = new Dictionary<long, long>{{1, 3}, {2, 2}, {3, 4}, {4, 1}}; //TODO: Resolve

                //Loop through future date ids
                foreach(var futureDateId in forecastDictionary.Keys)
                {
                    forecastDictionary[futureDateId] = latestLoadedUsage
                        .Where(u => u.Field<long>("DateId") == futureDateToUsageDateDictionary[futureDateId])
                        .Sum(u => u.Field<long>("Usage"));
                }

                //Get Forecast by Year
                var forecastYearIds = dateToYearMappings.Where(d => futureDateToUsageDateDictionary.ContainsKey(d.Field<long>("DateId")))
                    .Select(d => d.Field<long>("YearId"))
                    .Distinct();

                var granularityCode = "Week";

                foreach(var forecastYearId in forecastYearIds)
                {
                    var dateIdsForYearId = dateToYearMappings.Where(d => d.Field<long>("YearId") == forecastYearId)
                        .Select(d => d.Field<long>("DateId"));

                    //Get Forecast by Week
                    var forecastWeekIds = dateToWeekMappings.Where(d => dateIdsForYearId.Contains(d.Field<long>("DateId")))
                        .Select(d => d.Field<long>("WeekId"))
                        .Distinct();

                    foreach(var forecastWeekId in forecastWeekIds)
                    {
                        var dateIdsForYearIdWeekId = dateToWeekMappings.Where(d => d.Field<long>("WeekId") == forecastWeekId)
                            .Select(d => d.Field<long>("DateId")).Intersect(dateIdsForYearId);

                        var forecastYearWeekKeyValuePair = new KeyValuePair<long, long>(forecastYearId, forecastWeekId);

                        var forecast = forecastDictionary.Where(f => dateIdsForYearIdWeekId.Contains(f.Key))
                            .Sum(f => f.Value);

                        //End date existing week forecast
                        _supplyMethods.ForecastUsageGranularityHistory_Delete(meterType, meterId, granularityCode, forecastYearWeekKeyValuePair);
                        _supplyMethods.ForecastUsageGranularityLatest_Delete(meterType, meterId, granularityCode, forecastYearWeekKeyValuePair);

                        //Insert new week forecast
                        _supplyMethods.ForecastUsageGranularityHistory_Insert(meterType, meterId, granularityCode, createdByUserId, sourceId, forecastYearWeekKeyValuePair, forecast);
                        _supplyMethods.ForecastUsageGranularityLatest_Insert(meterType, meterId, granularityCode, forecastYearWeekKeyValuePair, forecast);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createWeekForecastAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createWeekForecastAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

