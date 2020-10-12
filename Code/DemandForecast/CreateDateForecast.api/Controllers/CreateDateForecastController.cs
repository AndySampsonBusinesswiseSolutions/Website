﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

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
        private readonly string granularityCode = "Date";
        private List<Tuple<long, decimal>> existingDateForecasts;
        private Dictionary<long, decimal> existingDateForecastDictionary;
        private Dictionary<long, decimal> forecastDictionary;

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

                if (!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CreateDateForecastAPI, createDateForecastAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createDateForecastAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                GetForecastDictionary(meterType, meterId);
                GetExistingForecast(meterType, meterId);

                var newDateForecastTuples = new List<Tuple<long, decimal>>();
                var oldDateForecastTuples = new List<Tuple<long, decimal>>();                

                foreach (var forecastDate in forecastDictionary)
                {
                    var addUsageToDataTable = !existingDateForecastDictionary.ContainsKey(forecastDate.Key)
                        || existingDateForecastDictionary[forecastDate.Key] != forecastDate.Value;

                    if (addUsageToDataTable)
                    {
                        if (existingDateForecastDictionary.ContainsKey(forecastDate.Key))
                        {
                            var existingDateForecastTuple = existingDateForecasts.First(t => t.Item1 == forecastDate.Key);
                            oldDateForecastTuples.Add(existingDateForecastTuple);
                        }

                        var newDateForecastTuple = new Tuple<long, decimal>(forecastDate.Key, forecastDate.Value);
                        newDateForecastTuples.Add(newDateForecastTuple);
                    }
                }

                if (newDateForecastTuples.Any())
                {
                    existingDateForecasts = existingDateForecasts.Except(oldDateForecastTuples).ToList();
                    existingDateForecasts.AddRange(newDateForecastTuples);

                    //Insert into history and latest tables
                    _supplyMethods.CreateGranularSupplyForecastDataTables(meterType, meterId, granularityCode, createdByUserId, sourceId, new List<string> { "DateId" }, newDateForecastTuples, existingDateForecasts);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDateForecastAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createDateForecastAPIId, true, $"System Error Id {errorId}");
            }
        }

        private void GetExistingForecast(string meterType, long meterId)
        {
            //Get existing date forecast
            existingDateForecasts = _supplyMethods.ForecastUsageGranularityLatest_GetLatestTuple(meterType, meterId, granularityCode, "DateId");
            existingDateForecastDictionary = existingDateForecasts.ToDictionary(
                    d => d.Item1,
                    d => d.Item2
            );
        }

        private void GetForecastDictionary(string meterType, long meterId)
        {
            //Get latest loaded usage
            var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatestTuple(meterType, meterId);

            //Set up forecast dictionary
            var futureDateToUsageDateDictionary = _supplyMethods.DateMapping_GetLatestDictionary(meterType, meterId);

            forecastDictionary = new Dictionary<long, decimal>(futureDateToUsageDateDictionary.ToDictionary(f => f.Key, f => new decimal()));

            //Loop through future date ids
            var forecastDictionaryKeys = forecastDictionary.Keys.ToList();
            foreach (var futureDateId in forecastDictionaryKeys)
            {
                forecastDictionary[futureDateId] = latestLoadedUsage
                    .Where(u => u.Item1 == futureDateToUsageDateDictionary[futureDateId])
                    .Sum(u => u.Item4);
            }
        }
    }
}