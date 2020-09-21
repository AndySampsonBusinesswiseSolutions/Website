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
using System.Collections.Concurrent;

namespace CreateForecastUsage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateForecastUsageController : ControllerBase
    {
        private readonly ILogger<CreateForecastUsageController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.DemandForecast _demandForecastMethods = new Methods.DemandForecast();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Information.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();
        private readonly Int64 createForecastUsageAPIId;
        private Dictionary<string, long> dateDictionary;
        private Dictionary<long, Dictionary<long, int>> dateToForecastGroupDictionary;
        private Dictionary<string, long> loadedUsageDateDictionary;
        private DateTime earliestUsageDate;
        private DateTime latestUsageDate;

        public CreateForecastUsageController(ILogger<CreateForecastUsageController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CreateForecastUsageAPI, _systemAPIPasswordEnums.CreateForecastUsageAPI);
            createForecastUsageAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CreateForecastUsageAPI);
        }

        [HttpPost]
        [Route("CreateForecastUsage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(createForecastUsageAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateForecastUsage/Create")]
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
                    createForecastUsageAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createForecastUsageAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = GetMeterIdByMeterType(meterType, jsonObject);                

                //Get latest loaded usage
                var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);

                //Get Loaded Usage Date Ids
                var latestLoadedUsageDateIds = latestLoadedUsage.Select(d => d.Field<long>("DateId")).ToList();

                //Get Date dictionary
                dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary();
                var futureDateIds = dateDictionary.Where(d => Convert.ToDateTime(d.Key) > DateTime.Today)
                    .Select(d => d.Value).ToList();

                //Get DateToForecastGroup
                dateToForecastGroupDictionary = _mappingMethods.DateToForecastGroup_GetDateForecastGroupDictionary();
                var futureDateToForecastGroupDictionary = dateToForecastGroupDictionary.Where(d => futureDateIds.Contains(d.Key))
                    .ToDictionary(d => d.Key, d => d.Value);

                //Get DateToForecastAgent
                var dateToForecastAgentDictionary = _mappingMethods.DateToForecastAgent_GetDateForecastAgentDictionary();

                //Get ForecastAgents
                var forecastAgentDataRows = _demandForecastMethods.ForecastAgent_GetList();
                var forecastAgentDictionary = forecastAgentDataRows.ToDictionary(d => d.Field<long>("ForecastAgentId"), d => d.Field<string>("ForecastAgent"));

                loadedUsageDateDictionary = dateDictionary.Where(d => latestLoadedUsageDateIds.Contains(d.Value))
                    .ToDictionary(d => d.Key, d => d.Value);
                earliestUsageDate = loadedUsageDateDictionary.Min(d => Convert.ToDateTime(d.Key));
                latestUsageDate = loadedUsageDateDictionary.Max(d => Convert.ToDateTime(d.Key));

                //Get Future Date mappings
                var futureDateToUsageDateDictionary = new ConcurrentDictionary<long, long>(futureDateToForecastGroupDictionary.ToDictionary(f => f.Key, f => new long()));
                foreach(var futureDateToForecastGroup in futureDateToForecastGroupDictionary.Take(40))
                {
                    var forecastAgentList = dateToForecastAgentDictionary[futureDateToForecastGroup.Key]
                        .OrderBy(a => a.Value)
                        .Select(a => forecastAgentDictionary[a.Key]);

                    var usageDateId = 0L;
                    foreach(var forecastAgent in forecastAgentList)
                    {
                        usageDateId = GetMappedUsageDateId(forecastAgent, futureDateToForecastGroup);

                        if(usageDateId > 0)
                        {
                            break;
                        }
                    }

                    futureDateToUsageDateDictionary[futureDateToForecastGroup.Key] = usageDateId;
                }

                if(futureDateToUsageDateDictionary.Any(f => f.Value == 0))
                {
                    //throw error as mapping has failed
                    var errorMessage = $"Forecast date ids without mapped usage date id: {string.Join(',', futureDateToUsageDateDictionary.Where(f => f.Value == 0))}";
                    var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, errorMessage, "Forecast Date Mapping", Environment.StackTrace);

                    //Update Process Queue
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createForecastUsageAPIId, true, $"System Error Id {errorId}");

                    return;
                }

                //Get GranularityId
                var granularity = "Five Minute";
                var granularityDescriptionGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityDescription);
                var granularityId = _informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeIdAndGranularityDetailDescription(granularityDescriptionGranularityAttributeId, granularity);

                //Get required time periods
                var nonStandardGranularityToTimePeriodDataRows = _mappingMethods.GranularityToTimePeriod_NonStandardDate_GetListByGranularityId(granularityId);
                var standardGranularityToTimePeriodDataRows = _mappingMethods.GranularityToTimePeriod_StandardDate_GetListByGranularityId(granularityId);
                var standardGranularityToTimePeriods = standardGranularityToTimePeriodDataRows.Select(d => d.Field<long>("TimePeriodId")).ToList();

                //Set up forecast dictionary
                var forecastDictionary = new ConcurrentDictionary<long, Dictionary<long, decimal>>();

                //Loop through future date ids
                foreach(var futureDateId in futureDateToForecastGroupDictionary.Keys)
                {
                    //Add date id to forecast dictionary
                    forecastDictionary.TryAdd(futureDateId, new Dictionary<long, decimal>());

                    //Get time periods required for date
                    var timePeriods = nonStandardGranularityToTimePeriodDataRows.Any(d => d.Field<long>("DateId") == futureDateId)
                        ? nonStandardGranularityToTimePeriodDataRows.Where(d => d.Field<long>("DateId") == futureDateId)
                            .Select(d => d.Field<long>("TimePeriodId"))
                        : standardGranularityToTimePeriods;

                    //Get usage date id
                    var usageDateId = futureDateToUsageDateDictionary[futureDateId];

                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createForecastUsageAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createForecastUsageAPIId, true, $"System Error Id {errorId}");
            }
        }

        private long GetMappedUsageDateId(string forecastAgent, KeyValuePair<long, Dictionary<long, int>> futureDateToForecastGroup)
        {
            var futureDate = Convert.ToDateTime(dateDictionary.First(d => d.Value == futureDateToForecastGroup.Key).Key);

            switch(forecastAgent)
            {
                case "Date":
                    return GetMappedUsageDateIdByDate(futureDate);
                case "ByForecastGroupByYear":
                    return GetMappedUsageDateIdByForecastGroupByYear(futureDateToForecastGroup.Value);
                case "ByYearByForecastGroup":
                    return GetMappedUsageDateIdByYearByForecastGroup(futureDateToForecastGroup.Value);
                default:
                    return 0L;
            }
        }

        private long GetMappedUsageDateIdByDate(DateTime futureDate)
        {
            //Maps directly against date from previous years
            var usageDateToFind = futureDate.AddYears(-1);

            while(usageDateToFind >= earliestUsageDate)
            {
                var usageDateId = loadedUsageDateDictionary.FirstOrDefault(d => Convert.ToDateTime(d.Key) == usageDateToFind).Value;

                if(usageDateId > 0)
                {
                    return usageDateId;
                }

                usageDateToFind = usageDateToFind.AddYears(-1);
            }

            return 0;
        }

        private long GetMappedUsageDateIdByForecastGroupByYear(Dictionary<long, int> forecastGroups)
        {
            //Tries to map against any ForecastGroup on a historical year before moving to next historical year
            var latestHistoricalDate = new DateTime(latestUsageDate.Year, latestUsageDate.Month, latestUsageDate.Day);
            
            while(latestHistoricalDate >= earliestUsageDate)
            {
                var earliestHistoricalDate = latestHistoricalDate.AddYears(-1).AddDays(1);
                var historicalUsageDateIds = loadedUsageDateDictionary
                    .Where(d => Convert.ToDateTime(d.Key) >= earliestHistoricalDate && Convert.ToDateTime(d.Key) <= latestHistoricalDate)
                    .Select(d => d.Value);
                var historicalDateToForecastGroupDictionary = dateToForecastGroupDictionary.Where(d => historicalUsageDateIds.Contains(d.Key))
                    .ToDictionary(d => d.Key, d => d.Value);

                foreach(var forecastGroupId in forecastGroups.OrderBy(fg => fg.Value).Select(fg => fg.Key))
                {
                    var mappedDateIds = historicalDateToForecastGroupDictionary
                        .Where(d => d.Value.ContainsKey(forecastGroupId))
                        .Select(d => d.Key).ToList();

                    if(mappedDateIds.Any())
                    {
                        var latestMappedDate = loadedUsageDateDictionary.Where(d => mappedDateIds.Contains(d.Value))
                            .Max(d => Convert.ToDateTime(d.Key));
                        return loadedUsageDateDictionary.First(d => Convert.ToDateTime(d.Key) == latestMappedDate).Value;
                    }
                }

                latestHistoricalDate = earliestHistoricalDate.AddDays(-1);
            }

            return 0L;
        }

        private long GetMappedUsageDateIdByYearByForecastGroup(Dictionary<long, int> forecastGroups)
        {
            //Tries to map against any historical year on a ForecastGroup before moving to next ForecastGroup
            foreach(var forecastGroupId in forecastGroups.OrderBy(fg => fg.Value).Select(fg => fg.Key))
            {
                var latestHistoricalDate = new DateTime(latestUsageDate.Year, latestUsageDate.Month, latestUsageDate.Day);
            
                while(latestHistoricalDate >= earliestUsageDate)
                {
                    var earliestHistoricalDate = latestHistoricalDate.AddYears(-1).AddDays(1);
                    var historicalUsageDateIds = loadedUsageDateDictionary
                        .Where(d => Convert.ToDateTime(d.Key) >= earliestHistoricalDate && Convert.ToDateTime(d.Key) <= latestHistoricalDate)
                        .Select(d => d.Value);
                    
                    var mappedDateIds = dateToForecastGroupDictionary.Where(d => historicalUsageDateIds.Contains(d.Key))
                        .Where(d => d.Value.ContainsKey(forecastGroupId))
                        .Select(d => d.Key);

                    if(mappedDateIds.Any())
                    {
                        var latestMappedDate = dateDictionary.Where(d => mappedDateIds.Contains(d.Value))
                            .Max(d => Convert.ToDateTime(d.Key));
                        return dateDictionary.First(d => Convert.ToDateTime(d.Key) == latestMappedDate).Value;
                    }

                    latestHistoricalDate = earliestHistoricalDate.AddDays(-1);
                }
            }

            return 0L;
        }

        private long GetMeterIdByMeterType(string meterType, JObject jsonObject)
        {
            if(meterType == "Meter")
            {
                //Get mpxn
                var mpxn = jsonObject[_systemAPIRequiredDataKeyEnums.MPXN].ToString();

                //Get MeterId
                return GetMeterId(mpxn);
            }
            else 
            {
                //Get mpxn
                var subMeterIdentifier = jsonObject[_systemAPIRequiredDataKeyEnums.SubMeterIdentifier].ToString();

                //Get MeterId
                return GetSubMeterId(subMeterIdentifier);
            }
        }

        private long GetMeterId(string mpxn)
        {
            //Get MeterIdentifierMeterAttributeId
            var customerMethods = new Methods.Customer();
            var customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
            var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(customerMeterAttributeEnums.MeterIdentifier);

            //Get MeterId
            return customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn).FirstOrDefault();
        }

        private long GetSubMeterId(string subMeterIdentifier)
        {
            //Get SubMeterIdentifierSubMeterAttributeId
            var customerMethods = new Methods.Customer();
            var customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
            var subMeterIdentifierSubMeterAttributeId = customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(customerSubMeterAttributeEnums.SubMeterIdentifier);

            //Get SubMeterId
            return customerMethods.SubMeterDetail_GetSubMeterIdListBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, subMeterIdentifier).FirstOrDefault();
        }
    }
}