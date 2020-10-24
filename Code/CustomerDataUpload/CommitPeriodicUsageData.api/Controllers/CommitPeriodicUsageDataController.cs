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

namespace CommitPeriodicUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitPeriodicUsageDataController : ControllerBase
    {
        private readonly ILogger<CommitPeriodicUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Information.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();
        private readonly Enums.System.Process.GUID _systemProcessGUIDEnums = new Enums.System.Process.GUID();
        private readonly Int64 commitPeriodicUsageDataAPIId;
        private long createdByUserId;
        private long sourceId;
        private string processQueueGUID;
        private string meterType;
        private long meterId;
        private string usageType;
        private long granularityId;
        private Dictionary<string, long> dateDictionary;

        public CommitPeriodicUsageDataController(ILogger<CommitPeriodicUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitPeriodicUsageDataAPI, _systemAPIPasswordEnums.CommitPeriodicUsageDataAPI);
            commitPeriodicUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitPeriodicUsageDataAPI);
        }

        [HttpPost]
        [Route("CommitPeriodicUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitPeriodicUsageDataAPIId, JObject.Parse(data.ToString()));

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

                if (!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, commitPeriodicUsageDataAPIId, jsonObject))
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
                var periodicUsageTempDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(periodicUsageJson.Replace(":{", ":\'{").Replace("},", "}\',").Replace("}}", "}\'}"));
                var periodicUsageDictionary = periodicUsageTempDictionary.ToDictionary(x => x.Key.Substring(0, 10), x => JsonConvert.DeserializeObject<Dictionary<string, string>>(x.Value));

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
                    var commitProfiledUsageAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitProfiledUsageAPI);
                    var commitProfiledUsageAPI = _systemMethods.PostAsJsonAsync(commitProfiledUsageAPIId, _systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, jsonObject);
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
                var routingAPIId = _systemMethods.GetRoutingAPIId();

                //Connect to Routing API and POST data
                _systemMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.CommitPeriodicUsageDataAPI, jsonObject);

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

        private List<DataRow> InsertPeriodicUsage(Dictionary<string, Dictionary<string, string>> periodicUsageDictionary)
        {
            var dates = periodicUsageDictionary.Select(u => u.Key)
                .Distinct()
                .Select(d => _methods.GetDateTimeSqlParameterFromDateTimeString(d).Substring(0, 10))
                .ToDictionary(d => d, d => dateDictionary[d]);  

            //Get GranularityToTimePeriod Lists by GranularityId
            var granularityToTimePeriodTimePeriodIdListByGranularityId = GetIdListFromDataRowsByGranularityId(_mappingMethods.GranularityToTimePeriod_GetList(), granularityId, "TimePeriodId");

            //Get TimePeriod List
            var timePeriodIdListByEndTime = GetTimePeriodListByEndTime();

            var timePeriods = periodicUsageDictionary.SelectMany(u => u.Value)
                    .Select(d => d.Key)
                    .Distinct()
                    .ToDictionary(t => t, t => timePeriodIdListByEndTime[t].Intersect(granularityToTimePeriodTimePeriodIdListByGranularityId).FirstOrDefault());

            //Create DataTable
            var dataTable = new DataTable();
            dataTable.Columns.Add("LoadedUsageId", typeof(long));
            dataTable.Columns.Add("CreatedDateTime", typeof(DateTime));
            dataTable.Columns.Add("CreatedByUserId", typeof(long));
            dataTable.Columns.Add("SourceId", typeof(long));
            dataTable.Columns.Add("DateId", typeof(long));
            dataTable.Columns.Add("TimePeriodId", typeof(long));
            dataTable.Columns.Add("UsageTypeId", typeof(long));
            dataTable.Columns.Add("Usage", typeof(decimal));

            //Set default values
            dataTable.Columns["CreatedDateTime"].DefaultValue = DateTime.UtcNow;
            dataTable.Columns["CreatedByUserId"].DefaultValue = createdByUserId;
            dataTable.Columns["SourceId"].DefaultValue = sourceId;
            dataTable.Columns["UsageTypeId"].DefaultValue = _informationMethods.UsageType_GetUsageTypeIdByUsageTypeDescription(usageType);

            //Create datarows
            var dataRows = AddDataRows(periodicUsageDictionary, dates, timePeriods, dataTable);

            foreach (var dataRow in dataRows)
            {
                dataTable.Rows.Add(dataRow);
            }

            //Bulk Insert new Periodic Usage into LoadedUsage table
            _supplyMethods.LoadedUsage_Insert(meterType, meterId, dataTable);

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
            var _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
            var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

            //Get MeterId
            return _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn).FirstOrDefault();
        }

        private long GetSubMeterId(string mpxn)
        {
            //Get SubMeterIdentifierSubMeterAttributeId
            var _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
            var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);

            //Get SubMeterId
            return _customerMethods.SubMeterDetail_GetSubMeterIdListBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, mpxn).FirstOrDefault();
        }
    }
}