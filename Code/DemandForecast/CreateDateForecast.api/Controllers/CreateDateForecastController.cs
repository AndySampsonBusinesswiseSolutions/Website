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

namespace CreateDateForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateDateForecastController : ControllerBase
    {
        private readonly ILogger<CreateDateForecastController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Int64 createDateForecastAPIId;

        public CreateDateForecastController(ILogger<CreateDateForecastController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CreateDateForecastAPI, _systemAPIPasswordEnums.CreateDateForecastAPI);
            createDateForecastAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateDateForecastAPI);
        }

        [HttpPost]
        [Route("CreateDateForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(createDateForecastAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateDateForecast/Create")]
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
                    createDateForecastAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CreateDateForecastAPI, createDateForecastAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createDateForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                //Get latest loaded usage
                var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);

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

                var granularityCode = "Date";

                //Get existing date forecast
                var existingDateForecasts = _supplyMethods.ForecastUsageGranularityLatest_GetLatest(meterType, meterId, granularityCode);

                foreach(var forecastDate in forecastDictionary)
                {
                    var existingDateForecastDataRow = existingDateForecasts.FirstOrDefault(d => d.Field<long>("DateId") == forecastDate.Key);

                    if(existingDateForecastDataRow == null || existingDateForecastDataRow.Field<decimal>("Usage") != forecastDate.Value)
                    {
                        var forecastDateKeyValuePair = new KeyValuePair<long, long>(forecastDate.Key, new long());

                        //End date existing date forecast
                        _supplyMethods.ForecastUsageGranularityHistory_Delete(meterType, meterId, granularityCode, forecastDateKeyValuePair);
                        _supplyMethods.ForecastUsageGranularityLatest_Delete(meterType, meterId, granularityCode, forecastDateKeyValuePair);

                        //Insert new date forecast
                        _supplyMethods.ForecastUsageGranularityHistory_Insert(meterType, meterId, granularityCode, createdByUserId, sourceId, forecastDateKeyValuePair, forecastDate.Value);
                        _supplyMethods.ForecastUsageGranularityLatest_Insert(meterType, meterId, granularityCode, forecastDateKeyValuePair, forecastDate.Value);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDateForecastAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDateForecastAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

