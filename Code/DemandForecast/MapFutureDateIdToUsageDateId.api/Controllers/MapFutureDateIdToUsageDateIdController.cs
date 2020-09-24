using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MapFutureDateIdToUsageDateId.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class MapFutureDateIdToUsageDateIdController : ControllerBase
    {
        private readonly ILogger<MapFutureDateIdToUsageDateIdController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.DemandForecast _demandForecastMethods = new Methods.DemandForecast();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Int64 mapFutureDateIdToUsageDateIdAPIId;
        private Dictionary<long, Dictionary<long, int>> dateToForecastGroupDictionary;
        private Dictionary<string, long> dateDictionary;
        private DateTime earliestUsageDate;
        private DateTime latestUsageDate;

        public MapFutureDateIdToUsageDateIdController(ILogger<MapFutureDateIdToUsageDateIdController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.MapFutureDateIdToUsageDateIdAPI, _systemAPIPasswordEnums.MapFutureDateIdToUsageDateIdAPI);
            mapFutureDateIdToUsageDateIdAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.MapFutureDateIdToUsageDateIdAPI);
        }

        [HttpPost]
        [Route("MapFutureDateIdToUsageDateId/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(mapFutureDateIdToUsageDateIdAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("MapFutureDateIdToUsageDateId/Map")]
        public string Map([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Get FutureDateId
                var futureDateId = Convert.ToInt64(jsonObject[_systemAPIRequiredDataKeyEnums.FutureDateId].ToString());
                earliestUsageDate = Convert.ToDateTime(jsonObject[_systemAPIRequiredDataKeyEnums.EarliestUsageDate].ToString());
                latestUsageDate = Convert.ToDateTime(jsonObject[_systemAPIRequiredDataKeyEnums.LatestUsageDate].ToString());

                //Get DateToForecastAgent
                var dateToForecastAgentDictionary = _mappingMethods.DateToForecastAgent_GetDateForecastAgentDictionaryByDateId(futureDateId);
                dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary();

                //Get DateToForecastGroup
                dateToForecastGroupDictionary = _mappingMethods.DateToForecastGroup_GetDateForecastGroupDictionary();
                var futureDateToForecastGroup = dateToForecastGroupDictionary.First(d => d.Key == futureDateId);

                //Get ForecastAgents
                var forecastAgentDictionary = _demandForecastMethods.GetForecastAgentDictionary();

                var forecastAgentList = dateToForecastAgentDictionary
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

                return $"{futureDateId},{usageDateId}";
            }
            catch(Exception error)
            {
                _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                return string.Empty;
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
                var usageDateId = dateDictionary.FirstOrDefault(d => Convert.ToDateTime(d.Key) == usageDateToFind).Value;

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
                var historicalDates = Enumerable.Range(0, latestHistoricalDate.Subtract(earliestHistoricalDate).Days + 1)
                    .Select(offset => earliestHistoricalDate.AddDays(offset)).ToList();
                var historicalUsageDateIds = dateDictionary
                    .Where(d => historicalDates.Contains(Convert.ToDateTime(d.Key)))
                    .Select(d => d.Value).ToList();
                var historicalDateToForecastGroupDictionary = dateToForecastGroupDictionary.Where(d => historicalUsageDateIds.Contains(d.Key))
                    .ToDictionary(d => d.Key, d => d.Value);

                foreach(var forecastGroupId in forecastGroups.OrderBy(fg => fg.Value).Select(fg => fg.Key))
                {
                    var mappedDateIds = historicalDateToForecastGroupDictionary
                        .Where(d => d.Value.ContainsKey(forecastGroupId))
                        .Select(d => d.Key).ToList();

                    if(mappedDateIds.Any())
                    {
                        var latestMappedDate = dateDictionary.Where(d => mappedDateIds.Contains(d.Value))
                            .Max(d => Convert.ToDateTime(d.Key));
                        return dateDictionary.First(d => Convert.ToDateTime(d.Key) == latestMappedDate).Value;
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
                    var historicalDates = Enumerable.Range(0, latestHistoricalDate.Subtract(earliestHistoricalDate).Days + 1)
                        .Select(offset => earliestHistoricalDate.AddDays(offset)).ToList();
                    var historicalUsageDateIds = dateDictionary
                        .Where(d => historicalDates.Contains(Convert.ToDateTime(d.Key)))
                        .Select(d => d.Value).ToList();
                    
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
    }
}