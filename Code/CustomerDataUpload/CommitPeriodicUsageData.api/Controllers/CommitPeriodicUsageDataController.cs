using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace CommitPeriodicUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitPeriodicUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitPeriodicUsageDataController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.SystemSchema.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
        private readonly Enums.InformationSchema.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.InformationSchema.Granularity.Attribute();
        private readonly Enums.SystemSchema.Process.GUID _systemProcessGUIDEnums = new Enums.SystemSchema.Process.GUID();
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
            _methods.InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitPeriodicUsageDataAPI, password);
            commitPeriodicUsageDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitPeriodicUsageDataAPI);
        }

        [HttpPost]
        [Route("CommitPeriodicUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitPeriodicUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitPeriodicUsageData/Commit")]
        public void Commit([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            createdByUserId = administrationUserMethods.GetSystemUserId();
            sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    commitPeriodicUsageDataAPIId);

                if (!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, commitPeriodicUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitPeriodicUsageDataAPIId);

                //Get MeterType
                meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get meterId/subMeterId
                meterId = meterType == "Meter"
                    ? GetMeterId(jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString())
                    : GetSubMeterId(jsonObject[_systemAPIRequiredDataKeyEnums.SubMeterIdentifier].ToString());

                //Get UsageTypeId
                usageType = jsonObject[_systemAPIRequiredDataKeyEnums.UsageType].ToString();

                //Get GranularityId
                var granularity = jsonObject[_systemAPIRequiredDataKeyEnums.Granularity].ToString();
                var granularityDescriptionGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityDescription);
                granularityId = _informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(granularityDescriptionGranularityAttributeId, granularity);

                //Get Date dictionary
                dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary();

                //Get Periodic Usage
                var periodicUsageJson = jsonObject[_systemAPIRequiredDataKeyEnums.PeriodicUsage].ToString();
                var periodicUsageDictionary = new Methods().DeserializePeriodicUsageToStringDictionary(periodicUsageJson);

                //Insert periodic usage
                var latestPeriodicUsageList = InsertPeriodicUsage(periodicUsageDictionary);

                //Check to see if full 365days are available
                var dateIdList = latestPeriodicUsageList.Select(r => r.Field<long>("DateId")).Distinct().ToList();
                var latestPeriodicUsageDictionary = CreateDictionary(latestPeriodicUsageList, dateIdList, "DateId", "TimePeriodId");

                var latestPeriodicUsageDate = dateIdList.Max(dateId => Convert.ToDateTime(dateDictionary.First(d => d.Value == dateId).Key));
                var earliestRequiredPeriodicUsageDate = latestPeriodicUsageDate.AddYears(-1).AddDays(1);

                var latestPeriodicUsageRequiresProfiling = DoesLatestPeriodicUsageRequiresProfiling(latestPeriodicUsageDictionary, latestPeriodicUsageDate, earliestRequiredPeriodicUsageDate);

                //If not 365 days, get generic profile unless it's a submeter in which case just stop
                if (latestPeriodicUsageRequiresProfiling)
                {
                    if(meterType == "SubMeter")
                    {
                        //Update Process Queue
                        _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitPeriodicUsageDataAPIId, false, null);

                        return;
                    }

                    //Call CommitProfiledUsage API and wait for response
                    var commitProfiledUsageAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitProfiledUsageAPI);
                    var commitProfiledUsageAPI = _systemAPIMethods.PostAsJsonAsync(commitProfiledUsageAPIId, _systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, hostEnvironment, jsonObject);
                    var commitProfiledUsageResult = commitProfiledUsageAPI.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                    latestPeriodicUsageList = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);
                }

                //Create new ProcessQueueGUID
                var newProcessQueueGUID = Guid.NewGuid().ToString();

                //Map current ProcessQueueGUID to new ProcessQueueGUID
                _systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                _systemMethods.SetProcessQueueGUIDInJObject(jsonObject, newProcessQueueGUID);

                //Update Process GUID to Create Forecast Usage Process GUID
                _systemMethods.SetProcessGUIDInJObject(jsonObject, _systemProcessGUIDEnums.CreateForecastUsage);

                //Get Routing.API URL
                var routingAPIId = _systemAPIMethods.GetRoutingAPIId();

                //Connect to Routing API and POST data
                _systemAPIMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, hostEnvironment, jsonObject);

                if(meterType == "SubMeter")
                {
                    //Update Process Queue
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitPeriodicUsageDataAPIId, false, null);

                    return;
                }

                //Get Existing Estimated Annual Usage
                var existingEstimatedAnnualUsage = _supplyMethods.EstimatedAnnualUsage_GetLatestEstimatedAnnualUsage(meterType, meterId);

                //Create Estimated Annual Usage
                var estimatedAnnualUsage = latestPeriodicUsageList
                    .Where(r => Convert.ToDateTime(dateDictionary.First(d => d.Value == r.Field<long>("DateId")).Key) >= earliestRequiredPeriodicUsageDate)
                    .Sum(r => r.Field<decimal>("Usage"));

                if(estimatedAnnualUsage != existingEstimatedAnnualUsage)
                {
                    //End date existing Estimated Annual Usage
                    _supplyMethods.EstimatedAnnualUsage_Delete(meterType, meterId);

                    //Insert new Estimated Annual Usage
                    _supplyMethods.EstimatedAnnualUsage_Insert(createdByUserId, sourceId, meterType, meterId, estimatedAnnualUsage);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitPeriodicUsageDataAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitPeriodicUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private bool DoesLatestPeriodicUsageRequiresProfiling(Dictionary<long, List<long>> latestPeriodicUsageDictionary, DateTime latestPeriodicUsageDate, DateTime earliestRequiredPeriodicUsageDate)
        {
            //Get standard number of time periods for granularity
            var granularityToTimePeriodStandardDateTimePeriodIdListByGranularityId = GetIdListFromDataRowsByGranularityId(_mappingMethods.GranularityToTimePeriod_StandardDate_GetList(), granularityId, "TimePeriodId");

            //Get dates that have additional number of time periods for granularity
            var granularityToTimePeriodNonStandardDateList = _mappingMethods.GranularityToTimePeriod_NonStandardDate_GetList()
                .Where(r => r.Field<long>("GranularityId") == granularityId).ToList();
            var dateIdList = GetIdListFromDataRowsByGranularityId(granularityToTimePeriodNonStandardDateList, granularityId, "DateId");
            var granularityToTimePeriodNonStandardDateDictionaryByGranularityId = CreateDictionary(granularityToTimePeriodNonStandardDateList, dateIdList, "DateId", "TimePeriodId");

            //is the earliest date in the usage earlier than 1 year ago
            var latestPeriodicUsageRequiresProfiling = latestPeriodicUsageDictionary
                .Min(ud => Convert.ToDateTime(dateDictionary.First(d => d.Value == ud.Key).Key))
                > earliestRequiredPeriodicUsageDate;

            if (!latestPeriodicUsageRequiresProfiling)
            {
                //does each date between earliestRequiredPeriodicUsageDate and latestPeriodicUsageDate exist in latestPeriodicUsageDictionary
                var periodicUsageDateIds = Enumerable.Range(0, latestPeriodicUsageDate.Subtract(earliestRequiredPeriodicUsageDate).Days + 1)
                    .Select(offset => earliestRequiredPeriodicUsageDate.AddDays(offset))
                    .Select(d => dateDictionary[_methods.ConvertDateTimeToSqlParameter(d).Substring(0, 10)]);

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
            var timePeriodList = _informationMethods.TimePeriod_GetList();
            var endTimeList = timePeriodList.Select(r => r.Field<TimeSpan>("EndTime").ToString().Substring(0, 5)).Distinct();
            var timePeriodIdListByEndTime = endTimeList.ToDictionary(r => r, r => new List<long>());

            foreach (var dataRow in timePeriodList)
            {
                timePeriodIdListByEndTime[dataRow.Field<TimeSpan>("EndTime").ToString().Substring(0, 5)].Add(dataRow.Field<long>("TimePeriodId"));
            }

            return timePeriodIdListByEndTime;
        }

        private static Dictionary<long, List<long>> CreateDictionary(List<DataRow> dataRows, List<long> idList, string keyColumnName, string valueColumnName)
        {
            var dictionary = idList.ToDictionary(r => r, r => new List<long>());

            foreach (var dataRow in dataRows)
            {
                dictionary[dataRow.Field<long>(keyColumnName)].Add(dataRow.Field<long>(valueColumnName));
            }

            return dictionary;
        }

        private List<long> GetIdListFromDataRowsByGranularityId(List<DataRow> dataRows, long granularityId, string idColumnName)
        {
            return dataRows.Where(r => r.Field<long>("GranularityId") == granularityId)
                .Select(r => r.Field<long>(idColumnName))
                .Distinct().ToList();
        }

        private List<DataRow> InsertPeriodicUsage(Dictionary<string, Dictionary<string, string>> periodicUsageStringDictionary)
        {
            var dates = periodicUsageStringDictionary.Select(u => u.Key)
                .Distinct()
                .Select(d => _methods.GetDateTimeSqlParameterFromDateTimeString(d).Substring(0, 10))
                .ToDictionary(d => d, d => dateDictionary[d]);  

            //Get GranularityToTimePeriod Lists by GranularityId
            var granularityToTimePeriodTimePeriodIdListByGranularityId = GetIdListFromDataRowsByGranularityId(_mappingMethods.GranularityToTimePeriod_GetList(), granularityId, "TimePeriodId");

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
            var usageTypeId = _informationMethods.UsageType_GetUsageTypeIdByUsageTypeDescription(usageType);
            _supplyMethods.InsertLoadedUsage(createdByUserId, sourceId, meterId, meterType, usageTypeId, periodicUsageDictionary);

            //Return latest periodic usage
            return _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);
        }

        private List<DataRow> AddDataRows(Dictionary<string, Dictionary<string, string>> periodicUsageDictionary, Dictionary<string, long> dates, Dictionary<string, long> timePeriods, DataTable dataTable)
        {
            var dataRows = new List<DataRow>();

            foreach(var periodicUsage in periodicUsageDictionary)
            {
                var timePeriodUsage = periodicUsage.Value
                    .Where(v => !string.IsNullOrWhiteSpace(v.Value))
                    .ToDictionary(x => timePeriods[x.Key], x => Convert.ToDecimal(x.Value));

                foreach (var timePeriod in timePeriodUsage)
                {
                    var dataRow = dataTable.NewRow();
                    dataRow["DateId"] = dates[periodicUsage.Key];
                    dataRow["TimePeriodId"] = timePeriod.Key;
                    dataRow["Usage"] = timePeriod.Value;
                    dataRows.Add(dataRow);
                }
            }

            return dataRows;         
        }

        private long GetMeterId(string mpxn)
        {
            //Get MeterIdentifierMeterAttributeId
            var _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
            var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

            //Get MeterId
            return _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn).FirstOrDefault();
        }

        private long GetSubMeterId(string mpxn)
        {
            //Get SubMeterIdentifierSubMeterAttributeId
            var _customerSubMeterAttributeEnums = new Enums.CustomerSchema.SubMeter.Attribute();
            var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);

            //Get SubMeterId
            return _customerMethods.SubMeterDetail_GetSubMeterIdListBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, mpxn).FirstOrDefault();
        }
    }
}