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

namespace CreateQuarterForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateQuarterForecastController : ControllerBase
    {
        private readonly ILogger<CreateQuarterForecastController> _logger;
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
        private readonly Int64 createQuarterForecastAPIId;

        public CreateQuarterForecastController(ILogger<CreateQuarterForecastController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CreateQuarterForecastAPI, _systemAPIPasswordEnums.CreateQuarterForecastAPI);
            createQuarterForecastAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateQuarterForecastAPI);
        }

        [HttpPost]
        [Route("CreateQuarterForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(createQuarterForecastAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateQuarterForecast/Create")]
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
                    createQuarterForecastAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createQuarterForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                //Get latest loaded usage
                var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);

                //Get Date to Quarter mappings
                var dateToQuarterMappings = _mappingMethods.DateToQuarter_GetList();
                
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

                var granularityCode = "Quarter";

                foreach(var forecastYearId in forecastYearIds)
                {
                    var dateIdsForYearId = dateToYearMappings.Where(d => d.Field<long>("YearId") == forecastYearId)
                        .Select(d => d.Field<long>("DateId"));

                    //Get Forecast by Quarter
                    var forecastQuarterIds = dateToQuarterMappings.Where(d => dateIdsForYearId.Contains(d.Field<long>("DateId")))
                        .Select(d => d.Field<long>("QuarterId"))
                        .Distinct();

                    foreach(var forecastQuarterId in forecastQuarterIds)
                    {
                        var dateIdsForYearIdQuarterId = dateToQuarterMappings.Where(d => d.Field<long>("QuarterId") == forecastQuarterId)
                            .Select(d => d.Field<long>("DateId")).Intersect(dateIdsForYearId);

                        var forecastYearQuarterKeyValuePair = new KeyValuePair<long, long>(forecastYearId, forecastQuarterId);

                        var forecast = forecastDictionary.Where(f => dateIdsForYearIdQuarterId.Contains(f.Key))
                            .Sum(f => f.Value);

                        //End date existing quarter forecast
                        _supplyMethods.ForecastUsageGranularityHistory_Delete(meterType, meterId, granularityCode, forecastYearQuarterKeyValuePair);
                        _supplyMethods.ForecastUsageGranularityLatest_Delete(meterType, meterId, granularityCode, forecastYearQuarterKeyValuePair);

                        //Insert new quarter forecast
                        _supplyMethods.ForecastUsageGranularityHistory_Insert(meterType, meterId, granularityCode, createdByUserId, sourceId, forecastYearQuarterKeyValuePair, forecast);
                        _supplyMethods.ForecastUsageGranularityLatest_Insert(meterType, meterId, granularityCode, forecastYearQuarterKeyValuePair, forecast);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createQuarterForecastAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createQuarterForecastAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

