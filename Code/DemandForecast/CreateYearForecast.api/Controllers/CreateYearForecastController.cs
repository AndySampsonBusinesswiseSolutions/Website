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

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createYearForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                //Get latest loaded usage
                var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);
                
                //Get Date to Year mappings
                var dateToYearMappings = _mappingMethods.DateToYear_GetList();
                var yearToDateDictionary = dateToYearMappings.Select(d => d.Field<long>("YearId")).Distinct()
                    .ToDictionary(
                        w => w,
                        w => dateToYearMappings.Where(d => d.Field<long>("YearId") == w).Select(d => d.Field<long>("DateId")).ToList()
                    );

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
                        .Sum(u => u.Field<decimal>("Usage"));
                }

                //Get Forecast by Year
                var forecastYearIds = yearToDateDictionary.Where(y => y.Value.Any(yv => futureDateToUsageDateDictionary.ContainsKey(yv)))
                    .Select(y => y.Key).Distinct().ToList();

                var granularityCode = "Year";

                //Get existing year forecast
                var existingYearForecasts = _supplyMethods.ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "YearId");
                var existingYearForecastDictionary = existingYearForecasts.ToDictionary(
                        d => d.Item1,
                        d => d.Item2
                );

                //Create DataTable
                var dataTable = _supplyMethods.CreateHistoryForecastDataTable(granularityCode, new List<string>{"YearId"}, createdByUserId, sourceId);
                var dataRowAdded = false;

                foreach(var forecastYearId in forecastYearIds)
                {
                    var dateIdsForYearId = yearToDateDictionary[forecastYearId];

                    var forecast = forecastDictionary.Where(f => dateIdsForYearId.Contains(f.Key))
                        .Sum(f => f.Value);

                    var addUsageToDataTable = !existingYearForecastDictionary.ContainsKey(forecastYearId)
                        || existingYearForecastDictionary[forecastYearId] != Math.Round(forecast, 10);

                    if(addUsageToDataTable)
                    {
                        AddToDataTable(dataTable, forecastYearId, forecast);
                        dataRowAdded = true;

                        if(existingYearForecastDictionary.ContainsKey(forecastYearId))
                        {
                            var existingYearForecastTuple = existingYearForecasts.First(t => t.Item1 == forecastYearId);
                            existingYearForecasts.Remove(existingYearForecastTuple);
                        }

                        var newDateForecastTuple = new Tuple<long, decimal>(forecastYearId, forecast);
                        existingYearForecasts.Add(newDateForecastTuple);
                    }
                }

                if(dataRowAdded)
                {
                    //Setup latest forecast
                    var latestForecastDataTable = _supplyMethods.CreateLatestForecastDataTable(dataTable, granularityCode);

                    foreach(var existingYearForecast in existingYearForecasts)
                    {
                        AddToDataTable(latestForecastDataTable, existingYearForecast.Item1, existingYearForecast.Item2);
                    }

                    //Insert into history and latest tables
                    _supplyMethods.InsertGranularSupplyForecast(dataTable, latestForecastDataTable, meterType, meterId, granularityCode);
                }  

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createYearForecastAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createYearForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void AddToDataTable(DataTable dataTable, long forecastYearId, decimal usage)
        {
            var dataRow = dataTable.NewRow();
            dataRow["YearId"] = forecastYearId;
            dataRow["Usage"] = usage;
            dataTable.Rows.Add(dataRow);
        }
    }
}