using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CreateFiveMinuteForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateFiveMinuteForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateFiveMinuteForecastController> _logger;
        private readonly Entity.System.API.CreateFiveMinuteForecast.Configuration _configuration;
        #endregion

        public CreateFiveMinuteForecastController(ILogger<CreateFiveMinuteForecastController> logger, Entity.System.API.CreateFiveMinuteForecast.Configuration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("CreateFiveMinuteForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(_configuration.APIId, _configuration.HostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateFiveMinuteForecast/Create")]
        public void Create([FromBody] object data)
        {
            var systemMethods = new Methods.System();
            var supplyMethods = new Methods.Supply();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    _configuration.APIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(_configuration.APIGUID, _configuration.APIId, _configuration.HostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, _configuration.APIId);

                //Get MeterType
                var meterType = jsonObject[new Enums.SystemSchema.API.RequiredDataKey().MeterType].ToString();

                //Get MeterId
                var meterId = new Methods.Customer().GetMeterIdByMeterType(meterType, jsonObject);

                //Get latest loaded usage
                var latestLoadedUsage = supplyMethods.LoadedUsageLatest_GetList(meterType, meterId);

                //Set up forecast dictionary
                var futureDateToUsageDateDictionary = supplyMethods.DateMapping_GetLatestDictionary(meterType, meterId);

                var forecastDictionary = futureDateToUsageDateDictionary.ToDictionary(
                    f => f.Key,
                    f => (_configuration.NonStandardGranularityDateDictionary.ContainsKey(f.Key)
                            ? _configuration.NonStandardGranularityDateDictionary[f.Key]
                            : _configuration.StandardGranularityTimePeriodList).ToDictionary(t => t, t => new decimal?())
                );

                //Loop through future date ids
                foreach(var forecast in forecastDictionary)
                {
                    supplyMethods.CreateTimePeriodForecast(forecast, latestLoadedUsage, futureDateToUsageDateDictionary, forecastDictionary, _configuration.TimePeriodToMappedTimePeriodDictionary);
                }
                
                //Get existing five minute forecast
                var existingFiveMinuteForecasts = new Methods.Supply().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, _configuration.GranularityCode, "DateId", "TimePeriodId");
                var existingFiveMinuteForecastDictionary = existingFiveMinuteForecasts.Select(f => f.Item1).Distinct()
                    .ToDictionary(
                        d => d,
                        d => existingFiveMinuteForecasts.Where(f => f.Item1 == d).ToDictionary(
                            t => t.Item2,
                            t => t.Item3
                        )
                );

                var newFiveMinuteForecastTuples = new List<Tuple<long, long, decimal>>();
                var oldFiveMinuteForecastTuples = new List<Tuple<long, long, decimal>>();

                foreach(var forecastDate in forecastDictionary)
                {
                    foreach (var forecastTimePeriod in forecastDate.Value)
                    {
                        var isNewPeriod = !existingFiveMinuteForecastDictionary.ContainsKey(forecastDate.Key)
                            || !existingFiveMinuteForecastDictionary[forecastDate.Key].ContainsKey(forecastTimePeriod.Key);
                        var addUsageToDataTable = isNewPeriod
                            || existingFiveMinuteForecastDictionary[forecastDate.Key][forecastTimePeriod.Key] != forecastTimePeriod.Value;

                        if (addUsageToDataTable)
                        {
                            if (!isNewPeriod)
                            {
                                oldFiveMinuteForecastTuples.Add(new Tuple<long, long, decimal>(forecastDate.Key, forecastTimePeriod.Key, existingFiveMinuteForecastDictionary[forecastDate.Key][forecastTimePeriod.Key]));
                            }

                            newFiveMinuteForecastTuples.Add(new Tuple<long, long, decimal>(forecastDate.Key, forecastTimePeriod.Key, forecastTimePeriod.Value.Value));
                        }
                    }
                }

                if (newFiveMinuteForecastTuples.Any())
                {
                    existingFiveMinuteForecasts = existingFiveMinuteForecasts.Except(oldFiveMinuteForecastTuples).ToList();
                    existingFiveMinuteForecasts.AddRange(newFiveMinuteForecastTuples);

                    //Insert into history and latest tables
                    supplyMethods.CreateGranularSupplyForecastDataTables(meterType, meterId, _configuration.GranularityCode, createdByUserId, sourceId, new List<string> { "DateId", "TimePeriodId" }, newFiveMinuteForecastTuples, existingFiveMinuteForecasts);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, _configuration.APIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, _configuration.APIId, true, $"System Error Id {errorId}");
            }
        }
    }
}