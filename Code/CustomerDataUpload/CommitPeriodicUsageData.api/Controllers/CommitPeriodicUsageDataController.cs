﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CommitPeriodicUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitPeriodicUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitPeriodicUsageDataController> _logger;
        private readonly Int64 commitPeriodicUsageDataAPIId;
        private long createdByUserId;
        private long sourceId;
        private string processQueueGUID;
        private string meterType;
        private long meterId;
        private string usageType;
        private long granularityId;
        private Dictionary<string, long> dateDictionary;
        private readonly string hostEnvironment;
        #endregion

        public CommitPeriodicUsageDataController(ILogger<CommitPeriodicUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitPeriodicUsageDataAPI, password);
            commitPeriodicUsageDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitPeriodicUsageDataAPI);
        }

        [HttpPost]
        [Route("CommitPeriodicUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(commitPeriodicUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitPeriodicUsageData/Commit")]
        public void Commit([FromBody] object data)
        {
            var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
            var informationMethods = new Methods.Information();
            var systemAPIMethods = new Methods.System.API();
            var systemMethods = new Methods.System();

            //Get base variables
            createdByUserId = new Methods.Administration.User().GetSystemUserId();
            sourceId = informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    commitPeriodicUsageDataAPIId);

                if (!systemAPIMethods.PrerequisiteAPIsAreSuccessful(systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, commitPeriodicUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitPeriodicUsageDataAPIId);

                var systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
                var customerMethods = new Methods.Customer();

                //Get MeterType
                meterType = jsonObject[systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get meterId/subMeterId
                meterId = meterType == "Meter"
                    ? customerMethods.GetMeterId(jsonObject[systemAPIRequiredDataKeyEnums.MPXN].ToString())
                    : customerMethods.GetSubMeterId(jsonObject[systemAPIRequiredDataKeyEnums.SubMeterIdentifier].ToString());

                //Get UsageTypeId
                usageType = jsonObject[systemAPIRequiredDataKeyEnums.UsageType].ToString();

                //Get GranularityId
                var granularity = jsonObject[systemAPIRequiredDataKeyEnums.Granularity].ToString();
                var granularityDescriptionGranularityAttributeId = informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(new Enums.InformationSchema.Granularity.Attribute().GranularityDescription);
                granularityId = informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(granularityDescriptionGranularityAttributeId, granularity);

                //Get Date dictionary
                dateDictionary = informationMethods.Date_GetDateDescriptionIdDictionary();

                //Get Periodic Usage
                var periodicUsageJson = jsonObject[systemAPIRequiredDataKeyEnums.PeriodicUsage].ToString();
                var periodicUsageDictionary = new Methods().DeserializePeriodicUsageToStringDictionary(periodicUsageJson);

                //Insert periodic usage
                var latestPeriodicUsageEntities = InsertPeriodicUsage(periodicUsageDictionary);

                //Check to see if full 365days are available
                var dateIdList = latestPeriodicUsageEntities.Select(lpul => lpul.DateId).Distinct().ToList();
                var latestPeriodicUsageDictionary = CreateDictionary(latestPeriodicUsageEntities, dateIdList, "DateId", "TimePeriodId");

                var latestPeriodicUsageDate = dateIdList.Max(dateId => Convert.ToDateTime(dateDictionary.First(d => d.Value == dateId).Key));
                var earliestRequiredPeriodicUsageDate = latestPeriodicUsageDate.AddYears(-1).AddDays(1);

                var latestPeriodicUsageRequiresProfiling = DoesLatestPeriodicUsageRequiresProfiling(latestPeriodicUsageDictionary, latestPeriodicUsageDate, earliestRequiredPeriodicUsageDate);

                //If not 365 days, get generic profile unless it's a submeter in which case just stop
                if (latestPeriodicUsageRequiresProfiling)
                {
                    if(meterType == "SubMeter")
                    {
                        //Update Process Queue
                        systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitPeriodicUsageDataAPIId, false, null);

                        return;
                    }

                    //Call CommitProfiledUsage API and wait for response
                    var commitProfiledUsageAPIId = systemAPIMethods.API_GetAPIIdByAPIGUID(systemAPIGUIDEnums.CommitProfiledUsageAPI);
                    var commitProfiledUsageResult = systemAPIMethods.PostAsJsonAsyncAndAwaitResult(commitProfiledUsageAPIId, systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, hostEnvironment, jsonObject);

                    latestPeriodicUsageEntities = new Methods.Supply().LoadedUsageLatest_GetList(meterType, meterId);
                }

                var supplyMethods = new Methods.Supply();

                //Create new ProcessQueueGUID
                var newProcessQueueGUID = Guid.NewGuid().ToString();

                //Map current ProcessQueueGUID to new ProcessQueueGUID
                systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                systemMethods.SetProcessQueueGUIDInJObject(jsonObject, newProcessQueueGUID);

                //Update Process GUID to Create Forecast Usage Process GUID
                systemMethods.SetProcessGUIDInJObject(jsonObject, new Enums.SystemSchema.Process.GUID().CreateForecastUsage);

                //Get Routing.API URL
                var routingAPIId = systemAPIMethods.GetRoutingAPIId();

                //Connect to Routing API and POST data
                systemAPIMethods.PostAsJsonAsync(routingAPIId, systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, hostEnvironment, jsonObject);

                if(meterType == "SubMeter")
                {
                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitPeriodicUsageDataAPIId, false, null);

                    return;
                }

                //Get Existing Estimated Annual Usage
                var existingEstimatedAnnualUsage = supplyMethods.EstimatedAnnualUsage_GetLatestEstimatedAnnualUsage(meterType, meterId);

                //Create Estimated Annual Usage
                var estimatedAnnualUsage = latestPeriodicUsageEntities
                    .Where(r => Convert.ToDateTime(dateDictionary.First(d => d.Value == r.DateId).Key) >= earliestRequiredPeriodicUsageDate)
                    .Sum(r => r.Usage);

                if(estimatedAnnualUsage != existingEstimatedAnnualUsage)
                {
                    //End date existing Estimated Annual Usage
                    supplyMethods.EstimatedAnnualUsage_Delete(meterType, meterId);

                    //Insert new Estimated Annual Usage
                    supplyMethods.EstimatedAnnualUsage_Insert(createdByUserId, sourceId, meterType, meterId, estimatedAnnualUsage);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitPeriodicUsageDataAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitPeriodicUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private bool DoesLatestPeriodicUsageRequiresProfiling(Dictionary<long, List<long>> latestPeriodicUsageDictionary, DateTime latestPeriodicUsageDate, DateTime earliestRequiredPeriodicUsageDate)
        {
            var mappingMethods = new Methods.Mapping();

            //Get standard number of time periods for granularity
            var granularityToTimePeriodStandardDateEntities = mappingMethods.GranularityToTimePeriod_StandardDate_GetList();
            var granularityToTimePeriodStandardDateTimePeriodIdListByGranularityId = GetIdListFromEntitiesByGranularityId(granularityToTimePeriodStandardDateEntities, granularityId, "TimePeriodId");

            //Get dates that have additional number of time periods for granularity
            var granularityToTimePeriodNonStandardDateEntities = mappingMethods.GranularityToTimePeriod_NonStandardDate_GetList()
                .Where(r => r.GranularityId == granularityId).ToList();
            var dateIdList = GetIdListFromEntitiesByGranularityId(granularityToTimePeriodNonStandardDateEntities, granularityId, "DateId");
            var granularityToTimePeriodNonStandardDateDictionaryByGranularityId = CreateDictionary(granularityToTimePeriodNonStandardDateEntities, dateIdList, "DateId", "TimePeriodId");

            //is the earliest date in the usage earlier than 1 year ago
            var latestPeriodicUsageRequiresProfiling = latestPeriodicUsageDictionary
                .Min(ud => Convert.ToDateTime(dateDictionary.First(d => d.Value == ud.Key).Key))
                > earliestRequiredPeriodicUsageDate;

            if (!latestPeriodicUsageRequiresProfiling)
            {
                var methods = new Methods();
                //does each date between earliestRequiredPeriodicUsageDate and latestPeriodicUsageDate exist in latestPeriodicUsageDictionary
                var periodicUsageDateIds = Enumerable.Range(0, latestPeriodicUsageDate.Subtract(earliestRequiredPeriodicUsageDate).Days + 1)
                    .Select(offset => earliestRequiredPeriodicUsageDate.AddDays(offset))
                    .Select(d => dateDictionary[methods.ConvertDateTimeToSqlParameter(d).Substring(0, 10)]);

                latestPeriodicUsageRequiresProfiling = periodicUsageDateIds.Any(d => !latestPeriodicUsageDictionary.ContainsKey(d));

                if (!latestPeriodicUsageRequiresProfiling)
                {
                    //for each date, does latestPeriodicUsageDictionary have enough time periods in list
                    foreach (var periodicUsageDateId in periodicUsageDateIds)
                    {
                        var latestPeriodicUsageTimePeriodIds = latestPeriodicUsageDictionary[periodicUsageDateId];

                        var timePeriodIdsToCheckAgainst = granularityToTimePeriodNonStandardDateDictionaryByGranularityId.ContainsKey(periodicUsageDateId)
                            ? granularityToTimePeriodNonStandardDateDictionaryByGranularityId[periodicUsageDateId]
                            : granularityToTimePeriodStandardDateTimePeriodIdListByGranularityId;

                        if (timePeriodIdsToCheckAgainst.Any(t => !latestPeriodicUsageTimePeriodIds.Contains(t)))
                        {
                            latestPeriodicUsageRequiresProfiling = true;
                            break;
                        }
                    }
                }
            }

            return latestPeriodicUsageRequiresProfiling;
        }

        private Dictionary<string, List<long>> GetTimePeriodListByEndTime()
        {
            var timePeriodEntities = new Methods.Information().TimePeriod_GetList();
            var endTimeList = timePeriodEntities.Select(tpe => tpe.EndTime.ToString().Substring(0, 5)).Distinct();
            var timePeriodIdListByEndTime = endTimeList.ToDictionary(etl => etl, etl => new List<long>());

            foreach (var timePeriodEntity in timePeriodEntities)
            {
                timePeriodIdListByEndTime[timePeriodEntity.EndTime.ToString().Substring(0, 5)].Add(timePeriodEntity.TimePeriodId);
            }

            return timePeriodIdListByEndTime;
        }

        private static Dictionary<long, List<long>> CreateDictionary<T>(List<T> entities, List<long> idList, string keyColumnName, string valueColumnName)
        {
            var dictionary = idList.ToDictionary(i => i, i => new List<long>());

            foreach (var entity in entities)
            {
                dictionary[Convert.ToInt64(entity.GetType().GetProperty(keyColumnName).GetValue(entity))].Add(Convert.ToInt64(entity.GetType().GetProperty(valueColumnName).GetValue(entity)));
            }

            return dictionary;
        }

        private List<long> GetIdListFromEntitiesByGranularityId<T>(List<T> entities, long granularityId, string idColumnName)
        {
            return entities.Where(e => Convert.ToInt64(e.GetType().GetProperty("GranularityId").GetValue(e)) == granularityId)
                .Select(e => Convert.ToInt64(e.GetType().GetProperty(idColumnName).GetValue(e)))
                .Distinct().ToList();
        }

        private List<Entity.Supply.LoadedUsageLatest> InsertPeriodicUsage(Dictionary<string, Dictionary<string, string>> periodicUsageStringDictionary)
        {
            var methods = new Methods();
            var dates = periodicUsageStringDictionary.Select(u => u.Key)
                .Distinct()
                .Select(d => methods.GetDateTimeSqlParameterFromDateTimeString(d).Substring(0, 10))
                .ToDictionary(d => d, d => dateDictionary[d]);  

            //Get GranularityToTimePeriod Lists by GranularityId
            var granularityToTimePeriodEntities = new Methods.Mapping().GranularityToTimePeriod_GetList();
            var granularityToTimePeriodTimePeriodIdListByGranularityId = GetIdListFromEntitiesByGranularityId(granularityToTimePeriodEntities, granularityId, "TimePeriodId");

            //Get TimePeriod List
            var timePeriodIdListByEndTime = GetTimePeriodListByEndTime();

            var timePeriods = periodicUsageStringDictionary.SelectMany(u => u.Value)
                    .Select(d => d.Key)
                    .Distinct()
                    .ToDictionary(t => t, t => timePeriodIdListByEndTime[t].Intersect(granularityToTimePeriodTimePeriodIdListByGranularityId).FirstOrDefault());

            var periodicUsageDictionary = periodicUsageStringDictionary.ToDictionary(
                d => dates[d.Key],
                d => d.Value.Where(t => !string.IsNullOrWhiteSpace(t.Value)).ToDictionary(t => timePeriods[t.Key], t => Convert.ToDecimal(t.Value))
            );

            //Insert new Periodic Usage into LoadedUsage tables
            var supplyMethods = new Methods.Supply();
            var usageTypeId = new Methods.Information().UsageType_GetUsageTypeIdByUsageTypeDescription(usageType);
            supplyMethods.InsertLoadedUsage(createdByUserId, sourceId, meterId, meterType, usageTypeId, periodicUsageDictionary);

            //Return latest periodic usage
            return supplyMethods.LoadedUsageLatest_GetList(meterType, meterId);
        }
    }
}