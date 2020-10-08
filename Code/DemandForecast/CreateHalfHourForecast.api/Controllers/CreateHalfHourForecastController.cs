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

namespace CreateHalfHourForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateHalfHourForecastController : ControllerBase
    {
        private readonly ILogger<CreateHalfHourForecastController> _logger;
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
        private readonly Int64 createHalfHourForecastAPIId;

        public CreateHalfHourForecastController(ILogger<CreateHalfHourForecastController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CreateHalfHourForecastAPI, _systemAPIPasswordEnums.CreateHalfHourForecastAPI);
            createHalfHourForecastAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateHalfHourForecastAPI);
        }

        [HttpPost]
        [Route("CreateHalfHourForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(createHalfHourForecastAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateHalfHourForecast/Create")]
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
                    createHalfHourForecastAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createHalfHourForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                //Get latest loaded usage
                var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);

                //Get GranularityId
                var granularityCode = "HalfHour";
                var granularityCodeGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityCode);
                var granularityId = _informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(granularityCodeGranularityAttributeId, granularityCode);

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

                var usageTypePriorityDictionary = new Dictionary<long, long>{{1, 3}, {2, 2}, {3, 4}, {4, 1}}; //TODO: Resolve                
                var timePeriodToTimePeriodDictionary = _mappingMethods.TimePeriodToTimePeriod_GetDictionary();
                var nonStandardGranularityDates = nonStandardGranularityToTimePeriodDataRows.Select(d => d.Field<long>("DateId")).Distinct()
                    .ToDictionary(
                        d => d, 
                        d => nonStandardGranularityToTimePeriodDataRows.Where(n => n.Field<long>("DateId") == d).Select(d => d.Field<long>("TimePeriodId")).ToList()
                    );

                var forecastDictionary = new ConcurrentDictionary<long, Dictionary<long, decimal>>(
                    futureDateToUsageDateDictionary.ToDictionary(
                        f => f.Key, 
                        f => (nonStandardGranularityDates.ContainsKey(f.Key)
                                ? nonStandardGranularityDates[f.Key]
                                : standardGranularityToTimePeriods).ToDictionary(t => t, t => new decimal())
                    )
                );

                var forecastFoundDictionary = new ConcurrentDictionary<long, Dictionary<long, bool>>(
                    futureDateToUsageDateDictionary.ToDictionary(
                        f => f.Key, 
                        f => (nonStandardGranularityDates.ContainsKey(f.Key)
                                ? nonStandardGranularityDates[f.Key]
                                : standardGranularityToTimePeriods).ToDictionary(t => t, t => new bool())
                    )
                );

                var timePeriodToMappedTimePeriodDictionary = new ConcurrentDictionary<long, KeyValuePair<long, List<long>>>(
                    timePeriodToTimePeriodDictionary.ToDictionary(
                        t => t.Key,
                        t => t.Value.Select(m => m).Distinct()
                            .ToDictionary(m => m, m => timePeriodToTimePeriodDictionary.Where(t => t.Value.Contains(m)).Select(t => t.Key).ToList())
                            .OrderBy(m => m.Value.Count()).First()
                    )
                );

                //Loop through future date ids
                foreach(var forecast in forecastDictionary)
                {
                    //Get usage date id
                    var usageDateId = futureDateToUsageDateDictionary[forecast.Key];

                    //Get usage for date
                    var usageForDateList = latestLoadedUsage.Where(u => u.Field<long>("DateId") == usageDateId).ToList();
                    var timePeriodIds = forecast.Value.Keys.ToList();
                    var forecastFound = forecastFoundDictionary[forecast.Key];

                    foreach(var timePeriodId in timePeriodIds)
                    {
                        if(forecastFound[timePeriodId])
                        {
                            continue;
                        }

                        //Get usage for time period
                        var usageForTimePeriodList = usageForDateList.Where(u => u.Field<long>("TimePeriodId") == timePeriodId).ToList();

                        if(usageForTimePeriodList.Any())
                        {
                            SetForecastValue(forecast.Value, forecastFound, timePeriodId, GetUsageByUsageType(usageForTimePeriodList, usageTypePriorityDictionary));
                        }
                        else
                        {
                            var mappedTimePeriodDictionary = timePeriodToMappedTimePeriodDictionary[timePeriodId];

                            usageForTimePeriodList = usageForDateList.Where(u => u.Field<long>("TimePeriodId") == mappedTimePeriodDictionary.Key).ToList();

                            //Get usage based on usage type priority
                            var mappedUsage = usageForTimePeriodList.ToDictionary(u => usageTypePriorityDictionary.First(ut => ut.Value == u.Field<long>("UsageTypeId")).Key, u => u.Field<decimal>("Usage"))
                                .OrderBy(u => u.Key).First().Value;

                            var mappedTimePeriodIdsWithUsageList = mappedTimePeriodDictionary.Value.Where(v => usageForDateList.Any(u => u.Field<long>("TimePeriodId") == v)).ToList();
                            var missingTimePeriodIds = mappedTimePeriodDictionary.Value.Except(mappedTimePeriodIdsWithUsageList);

                            foreach(var mappedtimePeriodId in mappedTimePeriodIdsWithUsageList)
                            {
                                //Get usage for time period
                                usageForTimePeriodList = usageForDateList.Where(u => u.Field<long>("TimePeriodId") == mappedtimePeriodId).ToList();

                                SetForecastValue(forecast.Value, forecastFound, timePeriodId, GetUsageByUsageType(usageForTimePeriodList, usageTypePriorityDictionary));
                            }

                            if(missingTimePeriodIds.Any())
                            {
                                var timePeriodUsage = mappedTimePeriodDictionary.Value
                                    .Where(t => forecastDictionary[forecast.Key].ContainsKey(t))
                                    .Sum(t => forecastDictionary[forecast.Key][t]);
                                var missingTimePeriodUsage = (mappedUsage - timePeriodUsage)/missingTimePeriodIds.Count();

                                foreach(var missingTimePeriodId in missingTimePeriodIds)
                                {
                                    SetForecastValue(forecast.Value, forecastFound, missingTimePeriodId, missingTimePeriodUsage);
                                }
                            }
                        }
                    }
                }

                //Get existing half hour forecast
                var existingHalfHourForecasts = _supplyMethods.ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode);
                var existingHalfHourForecastDictionary = existingHalfHourForecasts.Select(f => f.Item1).Distinct()
                    .ToDictionary(
                        d => d,
                        d => existingHalfHourForecasts.Where(f => f.Item1 == d).ToDictionary(
                            t => t.Item2,
                            t => t.Item3
                        )
                );

                //Create DataTable
                var dataTable = new DataTable();
                dataTable.Columns.Add("ProcessQueueGUID", typeof(string));
                dataTable.Columns.Add("CreatedByUserId", typeof(long));
                dataTable.Columns.Add("SourceId", typeof(long));
                dataTable.Columns.Add("DateId", typeof(long));
                dataTable.Columns.Add("TimePeriodId", typeof(long));
                dataTable.Columns.Add("Usage", typeof(decimal));

                //Set default values
                dataTable.Columns["ProcessQueueGUID"].DefaultValue = processQueueGUID;
                dataTable.Columns["CreatedByUserId"].DefaultValue = createdByUserId;
                dataTable.Columns["SourceId"].DefaultValue = sourceId;

                var dataRowAdded = false;

                foreach(var forecastDate in forecastDictionary)
                {
                    foreach(var forecastTimePeriod in forecastDate.Value)
                    {
                        var addUsageToDataTable = !existingHalfHourForecastDictionary.ContainsKey(forecastDate.Key)
                            || !existingHalfHourForecastDictionary[forecastDate.Key].ContainsKey(forecastTimePeriod.Key)
                            || existingHalfHourForecastDictionary[forecastDate.Key][forecastTimePeriod.Key] != forecastTimePeriod.Value;

                        if(addUsageToDataTable)
                        {
                            AddToDataTable(dataTable, forecastDate.Key, forecastTimePeriod.Key, forecastTimePeriod.Value);
                        dataRowAdded = true;
                        }
                    }
                }

                if(dataRowAdded)
                {
                    _supplyMethods.InsertGranularSupplyForecast(dataTable, meterType, meterId, granularityCode, processQueueGUID);
                }  
                
                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createHalfHourForecastAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createHalfHourForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void AddToDataTable(DataTable dataTable, long forecastDateId, long forecastTimePeriodId, decimal usage)
        {
            var dataRow = dataTable.NewRow();
            dataRow["DateId"] = forecastDateId;
            dataRow["TimePeriodId"] = forecastTimePeriodId;
            dataRow["Usage"] = usage;
            dataTable.Rows.Add(dataRow);
        }

        private decimal GetUsageByUsageType(List<DataRow> usageForTimePeriodList, Dictionary<long, long> usageTypePriorityDictionary)
        {
            return usageForTimePeriodList.ToDictionary(u => usageTypePriorityDictionary.First(ut => ut.Value == u.Field<long>("UsageTypeId")).Key, u => u.Field<decimal>("Usage"))
                .OrderBy(u => u.Key).First().Value;
        }

        private void SetForecastValue(Dictionary<long, decimal> forecast, Dictionary<long, bool> forecastFound, long timePeriodId, decimal usage)
        {
            //Add usage to forecast
            forecast[timePeriodId] = Math.Round(usage, 10);
            forecastFound[timePeriodId] = true;
        }
    }
}