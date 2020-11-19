using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GetMappedUsageDateId.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class GetMappedUsageDateIdController : ControllerBase
    {
        #region Variables
        private readonly ILogger<GetMappedUsageDateIdController> _logger;
        private readonly Int64 getMappedUsageDateIdAPIId;
        private Dictionary<long, Dictionary<long, int>> dateToForecastGroupDictionary;
        private Dictionary<string, long> dateDictionary;
        private Dictionary<DateTime, long> loadedUsageDateDictionary;
        private DateTime earliestUsageDate;
        private DateTime latestUsageDate;
        private readonly string hostEnvironment;
        #endregion

        public GetMappedUsageDateIdController(ILogger<GetMappedUsageDateIdController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().GetMappedUsageDateIdAPI, password);
            getMappedUsageDateIdAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().GetMappedUsageDateIdAPI);
        }

        [HttpPost]
        [Route("GetMappedUsageDateId/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(getMappedUsageDateIdAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetMappedUsageDateId/Get")]
        public void Get([FromBody] object data)
        {
            var systemMethods = new Methods.System();
            var informationMethods = new Methods.Information();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

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
                    getMappedUsageDateIdAPIId);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getMappedUsageDateIdAPIId);

                var mappingMethods = new Methods.Mapping();

                //Get MeterType
                var meterType = jsonObject[new Enums.SystemSchema.API.RequiredDataKey().MeterType].ToString();

                //Get MeterId
                var meterId = new Methods.Customer().GetMeterIdByMeterType(meterType, jsonObject); 

                //Get latest loaded usage
                var latestDateMapping = new Methods.Supply().LoadedUsageLatest_GetList(meterType, meterId);

                //Get Date dictionary
                dateDictionary = informationMethods.Date_GetDateDescriptionIdDictionary();

                //Get Loaded Usage Date Ids
                var latestDateMappingDateIds = latestDateMapping.Select(d => d.DateId).ToList();

                loadedUsageDateDictionary = dateDictionary.Where(d => latestDateMappingDateIds.Contains(d.Value))
                    .ToDictionary(d => Convert.ToDateTime(d.Key), d => d.Value);
                earliestUsageDate = loadedUsageDateDictionary.Min(d => d.Key);
                latestUsageDate = loadedUsageDateDictionary.Max(d => d.Key);

                var futureDateIds = dateDictionary.Where(d => Convert.ToDateTime(d.Key) > latestUsageDate)
                    .Select(d => d.Value).ToList();

                //Get DateToForecastGroup
                dateToForecastGroupDictionary = mappingMethods.DateToForecastGroup_GetDateForecastGroupDictionary();
                var futureDateToForecastGroupDictionary = dateToForecastGroupDictionary.Where(d => futureDateIds.Contains(d.Key))
                    .ToDictionary(d => d.Key, d => d.Value);

                //Get Future Date mappings
                var futureDateToUsageDateDictionary = new ConcurrentDictionary<long, long>(futureDateToForecastGroupDictionary.ToDictionary(f => f.Key, f => new long()));

                //Get DateToForecastAgent
                var dateToForecastAgentDictionary = mappingMethods.DateToForecastAgent_GetDateForecastAgentDictionary();

                //Get ForecastAgents
                var forecastAgentDictionary = new Methods.DemandForecast().GetForecastAgentDictionary();

                Parallel.ForEach(futureDateToForecastGroupDictionary, new ParallelOptions{MaxDegreeOfParallelism = 5}, futureDateToForecastGroup => {
                    //Order by priority
                    var forecastAgents = dateToForecastAgentDictionary[futureDateToForecastGroup.Key];
                    var forecastAgentList = forecastAgents
                        .OrderBy(a => a.Value)
                        .Select(a => forecastAgentDictionary[a.Key])
                        .ToList();

                    //Get mapped usage date
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
                });

                //Create DataTable
                var dataTable = new DataTable();
                dataTable.Columns.Add("ProcessQueueGUID", typeof(string));
                dataTable.Columns.Add("CreatedByUserId", typeof(long));
                dataTable.Columns.Add("SourceId", typeof(long));
                dataTable.Columns.Add("DateId", typeof(long));
                dataTable.Columns.Add("MappedDateId", typeof(long));

                //Set default values
                dataTable.Columns["ProcessQueueGUID"].DefaultValue = processQueueGUID;
                dataTable.Columns["CreatedByUserId"].DefaultValue = createdByUserId;
                dataTable.Columns["SourceId"].DefaultValue = sourceId;

                foreach(var futureDateToUsageDate in futureDateToUsageDateDictionary)
                {
                    var dataRow = dataTable.NewRow();
                    dataRow["DateId"] = futureDateToUsageDate.Key;
                    dataRow["MappedDateId"] = futureDateToUsageDate.Value;
                    dataTable.Rows.Add(dataRow);
                }

                //Insert new Date Mappings
                new Methods.Supply().InsertDateMapping(meterType, meterId, dataTable, processQueueGUID);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getMappedUsageDateIdAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getMappedUsageDateIdAPIId, true, $"System Error Id {errorId}");
            }
        }

        private long GetMappedUsageDateId(string forecastAgent, KeyValuePair<long, Dictionary<long, int>> futureDateToForecastGroup)
        {
            switch(forecastAgent)
            {
                case "Date":
                    return GetMappedUsageDateIdByDate(futureDateToForecastGroup.Key);
                case "ByForecastGroupByYear":
                    return GetMappedUsageDateIdByForecastGroupByYear(futureDateToForecastGroup.Value);
                case "ByYearByForecastGroup":
                    return GetMappedUsageDateIdByYearByForecastGroup(futureDateToForecastGroup.Value);
                default:
                    return 0L;
            }
        }

        private long GetMappedUsageDateIdByDate(long futureDateId)
        {
            //Maps directly against date from previous years
            var usageDateToFind = Convert.ToDateTime(dateDictionary.First(d => d.Value == futureDateId).Key).AddYears(-1);

            while(usageDateToFind >= earliestUsageDate)
            {
                var usageDateId = loadedUsageDateDictionary.FirstOrDefault(d => d.Key == usageDateToFind).Value;

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
            var orderedForecastGroupIds = forecastGroups.OrderBy(fg => fg.Value).Select(fg => fg.Key).ToList();
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

                foreach(var forecastGroupId in orderedForecastGroupIds)
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