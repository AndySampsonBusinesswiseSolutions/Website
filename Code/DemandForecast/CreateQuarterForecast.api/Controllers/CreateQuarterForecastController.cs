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
                var quarterToDateDictionary = dateToQuarterMappings.Select(d => d.Field<long>("QuarterId")).Distinct()
                    .ToDictionary(
                        w => w,
                        w => dateToQuarterMappings.Where(d => d.Field<long>("QuarterId") == w).Select(d => d.Field<long>("DateId")).ToList()
                    );
                
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

                var granularityCode = "Quarter";

                //Get existing quarter forecast
                var existingQuarterForecasts = _supplyMethods.ForecastUsageGranularityLatest_GetLatest(meterType, meterId, granularityCode);
                var existingQuarterForecastDictionary = existingQuarterForecasts.Select(d => d.Field<long>("YearId")).Distinct()
                    .ToDictionary(
                        d => d,
                        d => existingQuarterForecasts.Where(e => e.Field<long>("YearId") == d).ToDictionary(
                            t => t.Field<long>("QuarterId"),
                            t => t.Field<decimal>("Usage")
                        )
                );

                //Create DataTable
                var dataTable = new DataTable();
                dataTable.Columns.Add("ProcessQueueGUID", typeof(string));
                dataTable.Columns.Add("CreatedByUserId", typeof(long));
                dataTable.Columns.Add("SourceId", typeof(long));
                dataTable.Columns.Add("YearId", typeof(long));
                dataTable.Columns.Add("QuarterId", typeof(long));
                dataTable.Columns.Add("Usage", typeof(decimal));

                //Set default values
                dataTable.Columns["ProcessQueueGUID"].DefaultValue = processQueueGUID;
                dataTable.Columns["CreatedByUserId"].DefaultValue = createdByUserId;
                dataTable.Columns["SourceId"].DefaultValue = sourceId;

                var dataRowAdded = false;

                foreach(var forecastYearId in forecastYearIds)
                {
                    var dateIdsForYearId = yearToDateDictionary[forecastYearId];

                    //Get Forecast by Quarter
                    var forecastQuarterIds = quarterToDateDictionary.Where(w => w.Value.Any(wv => dateIdsForYearId.Contains(wv)))
                        .Select(w => w.Key).Distinct().ToList();

                    foreach(var forecastQuarterId in forecastQuarterIds)
                    {
                        var dateIdsForYearIdQuarterId = quarterToDateDictionary[forecastQuarterId].Intersect(dateIdsForYearId);

                        var forecast = forecastDictionary.Where(f => dateIdsForYearIdQuarterId.Contains(f.Key))
                            .Sum(f => f.Value);

                        var addUsageToDataTable = !existingQuarterForecastDictionary.ContainsKey(forecastYearId)
                            || !existingQuarterForecastDictionary[forecastYearId].ContainsKey(forecastQuarterId)
                            || existingQuarterForecastDictionary[forecastYearId][forecastQuarterId] != Math.Round(forecast, 10);

                        if(addUsageToDataTable)
                        {
                            AddToDataTable(dataTable, forecastYearId, forecastQuarterId, forecast);
                            dataRowAdded = true;
                        }
                    }
                }

                if(dataRowAdded)
                {
                    _supplyMethods.InsertGranularSupplyForecast(dataTable, meterType, meterId, granularityCode, processQueueGUID);
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

        private void AddToDataTable(DataTable dataTable, long forecastYearId, long forecastQuarterId, decimal usage)
        {
            var dataRow = dataTable.NewRow();
            dataRow["YearId"] = forecastYearId;
            dataRow["QuarterId"] = forecastQuarterId;
            dataRow["Usage"] = usage;
            dataTable.Rows.Add(dataRow);
        }
    }
}

