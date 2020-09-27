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

namespace CreateFiveMinuteForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateFiveMinuteForecastController : ControllerBase
    {
        private readonly ILogger<CreateFiveMinuteForecastController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Information.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();
        private readonly Int64 createFiveMinuteForecastAPIId;

        public CreateFiveMinuteForecastController(ILogger<CreateFiveMinuteForecastController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CreateFiveMinuteForecastAPI, _systemAPIPasswordEnums.CreateFiveMinuteForecastAPI);
            createFiveMinuteForecastAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateFiveMinuteForecastAPI);
        }

        [HttpPost]
        [Route("CreateFiveMinuteForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(createFiveMinuteForecastAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateFiveMinuteForecast/Create")]
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
                    createFiveMinuteForecastAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createFiveMinuteForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                //Get latest loaded usage
                var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);

                //Get GranularityId
                var granularity = "Five Minute";
                var granularityDescriptionGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityDescription);
                var granularityId = _informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(granularityDescriptionGranularityAttributeId, granularity);

                //Get required time periods
                var nonStandardGranularityToTimePeriodDataRows = _mappingMethods.GranularityToTimePeriod_NonStandardDate_GetListByGranularityId(granularityId);
                var standardGranularityToTimePeriodDataRows = _mappingMethods.GranularityToTimePeriod_StandardDate_GetListByGranularityId(granularityId);
                var standardGranularityToTimePeriods = standardGranularityToTimePeriodDataRows.Select(d => d.Field<long>("TimePeriodId")).ToList();

                //Set up forecast dictionary
                var dateMappings = _supplyMethods.DateMapping_GetLatest(meterType, meterId);
                var futureDateToUsageDateDictionary = dateMappings.ToDictionary(
                    d => d.Field<long>("DateId"),
                    d => d.Field<long>("MappedDateId")
                );
                var forecastDictionary = new ConcurrentDictionary<long, Dictionary<long, decimal>>(futureDateToUsageDateDictionary.ToDictionary(f => f.Key, f => new Dictionary<long, decimal>()));
                var usageTypePriority = new Dictionary<long, long>{{1, 3}, {2, 2}, {3, 4}, {4, 1}}; //TODO: Resolve
                var timePeriodToTimePeriodIds = _mappingMethods.TimePeriodToTimePeriod_GetList();

                //Loop through future date ids
                foreach(var futureDateId in forecastDictionary.Keys)
                {
                    //Get time periods required for date
                    var timePeriodIds = nonStandardGranularityToTimePeriodDataRows.Any(d => d.Field<long>("DateId") == futureDateId)
                        ? nonStandardGranularityToTimePeriodDataRows.Where(d => d.Field<long>("DateId") == futureDateId)
                            .Select(d => d.Field<long>("TimePeriodId"))
                        : standardGranularityToTimePeriods;

                    //Get usage date id
                    var usageDateId = futureDateToUsageDateDictionary[futureDateId];

                    //Get usage for date
                    var usageForDateList = latestLoadedUsage.Where(u => u.Field<long>("DateId") == usageDateId);

                    foreach(var timePeriodId in timePeriodIds)
                    {
                        if(forecastDictionary[futureDateId].ContainsKey(timePeriodId))
                        {
                            continue;
                        }

                        //Get usage for time period
                        var usageForTimePeriodList = usageForDateList.Where(u => u.Field<long>("TimePeriodId") == timePeriodId);

                        if(usageForTimePeriodList.Any())
                        {
                            //Get usage based on usage type priority
                            var usage = usageForTimePeriodList.ToDictionary(u => usageTypePriority.First(ut => ut.Value == u.Field<long>("UsageTypeId")).Key, u => u.Field<decimal>("Usage"))
                                .OrderBy(u => u.Key).First().Value;

                            //Add usage to forecast
                            forecastDictionary[futureDateId].TryAdd(timePeriodId, usage);
                        }
                        else
                        {
                            var mappedTimePeriodIds = timePeriodToTimePeriodIds.Where(t => t.Field<long>("TimePeriodId") == timePeriodId);
                            var mappedtimePeriodDictionary = mappedTimePeriodIds.ToDictionary(
                                    m => m.Field<long>("MappedTimePeriodId"),
                                    m => timePeriodToTimePeriodIds.Where(t => t.Field<long>("MappedTimePeriodId") == m.Field<long>("MappedTimePeriodId"))
                                        .Select(t => t.Field<long>("TimePeriodId")))
                                .OrderBy(m => m.Value.Count()).First();

                            usageForTimePeriodList = usageForDateList.Where(u => u.Field<long>("TimePeriodId") == mappedtimePeriodDictionary.Key);

                            //Get usage based on usage type priority
                            var mappedUsage = usageForTimePeriodList.ToDictionary(u => usageTypePriority.First(ut => ut.Value == u.Field<long>("UsageTypeId")).Key, u => u.Field<decimal>("Usage"))
                                .OrderBy(u => u.Key).First().Value;

                            var missingTimePeriodIds = new List<long>();

                            foreach(var mappedtimePeriodId in mappedtimePeriodDictionary.Value)
                            {
                                //Get usage for time period
                                usageForTimePeriodList = usageForDateList.Where(u => u.Field<long>("TimePeriodId") == mappedtimePeriodId);

                                if(usageForTimePeriodList.Any())
                                {
                                    //Get usage based on usage type priority
                                    var usage = usageForTimePeriodList.ToDictionary(u => usageTypePriority.First(ut => ut.Value == u.Field<long>("UsageTypeId")).Key, u => u.Field<decimal>("Usage"))
                                        .OrderBy(u => u.Key).First().Value;

                                    //Add usage to forecast
                                    forecastDictionary[futureDateId].TryAdd(mappedtimePeriodId, usage);
                                }
                                else
                                {
                                    missingTimePeriodIds.Add(mappedtimePeriodId);
                                }
                            }

                            if(missingTimePeriodIds.Any())
                            {
                                var timePeriodUsage = mappedtimePeriodDictionary.Value
                                    .Where(t => forecastDictionary[futureDateId].ContainsKey(t))
                                    .Sum(t => forecastDictionary[futureDateId][t]);
                                var missingTimePeriodUsage = (mappedUsage - timePeriodUsage)/missingTimePeriodIds.Count();

                                foreach(var missingTimePeriodId in missingTimePeriodIds)
                                {
                                    //Add usage to forecast
                                    forecastDictionary[futureDateId].TryAdd(missingTimePeriodId, missingTimePeriodUsage);
                                }
                            }
                        }
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createFiveMinuteForecastAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createFiveMinuteForecastAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

