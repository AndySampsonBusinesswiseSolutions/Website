using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CreateHalfHourForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateHalfHourForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateHalfHourForecastController> _logger;
        private readonly Entity.System.API.CreateHalfHourForecast.Configuration _configuration;
        #endregion

        public CreateHalfHourForecastController(ILogger<CreateHalfHourForecastController>  logger, Entity.System.API.CreateHalfHourForecast.Configuration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("CreateHalfHourForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(_configuration.APIId, _configuration.HostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateHalfHourForecast/Create")]
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
                var existingHalfHourForecasts = new Methods.Supply().ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, _configuration.GranularityCode, "DateId", "TimePeriodId");
                var existingHalfHourForecastDictionary = existingHalfHourForecasts.Select(f => f.Item1).Distinct()
                    .ToDictionary(
                        d => d,
                        d => existingHalfHourForecasts.Where(f => f.Item1 == d).ToDictionary(
                            t => t.Item2,
                            t => t.Item3
                        )
                );

                var newHalfHourForecastTuples = new List<Tuple<long, long, decimal>>();
                var oldHalfHourForecastTuples = new List<Tuple<long, long, decimal>>();

                foreach(var forecastDate in forecastDictionary)
                {
                    foreach (var forecastTimePeriod in forecastDate.Value)
                    {
                        var isNewPeriod = !existingHalfHourForecastDictionary.ContainsKey(forecastDate.Key)
                            || !existingHalfHourForecastDictionary[forecastDate.Key].ContainsKey(forecastTimePeriod.Key);
                        var addUsageToDataTable = isNewPeriod
                            || existingHalfHourForecastDictionary[forecastDate.Key][forecastTimePeriod.Key] != forecastTimePeriod.Value;

                        if (addUsageToDataTable)
                        {
                            if (!isNewPeriod)
                            {
                                oldHalfHourForecastTuples.Add(new Tuple<long, long, decimal>(forecastDate.Key, forecastTimePeriod.Key, existingHalfHourForecastDictionary[forecastDate.Key][forecastTimePeriod.Key]));
                            }

                            newHalfHourForecastTuples.Add(new Tuple<long, long, decimal>(forecastDate.Key, forecastTimePeriod.Key, forecastTimePeriod.Value.Value));
                        }
                    }
                };

                if (newHalfHourForecastTuples.Any())
                {
                    existingHalfHourForecasts = existingHalfHourForecasts.Except(oldHalfHourForecastTuples).ToList();
                    existingHalfHourForecasts.AddRange(newHalfHourForecastTuples);

                    //Insert into history and latest tables
                    supplyMethods.CreateGranularSupplyForecastDataTables(meterType, meterId, _configuration.GranularityCode, createdByUserId, sourceId, new List<string> { "DateId", "TimePeriodId" }, newHalfHourForecastTuples, existingHalfHourForecasts);
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