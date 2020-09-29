using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GetMappedUsageDateId.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class GetMappedUsageDateIdController : ControllerBase
    {
        private readonly ILogger<GetMappedUsageDateIdController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Supply _supplyMethods = new Methods.Supply();
        private readonly Methods.DemandForecast _demandForecastMethods = new Methods.DemandForecast();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Int64 getMappedUsageDateIdAPIId;
        private Dictionary<long, Dictionary<long, int>> dateToForecastGroupDictionary;
        private Dictionary<string, long> dateDictionary;
        private Dictionary<DateTime, long> loadedUsageDateDictionary;
        private DateTime earliestUsageDate;
        private DateTime latestUsageDate;

        public GetMappedUsageDateIdController(ILogger<GetMappedUsageDateIdController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.GetMappedUsageDateIdAPI, _systemAPIPasswordEnums.GetMappedUsageDateIdAPI);
            getMappedUsageDateIdAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetMappedUsageDateIdAPI);
        }

        [HttpPost]
        [Route("GetMappedUsageDateId/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(getMappedUsageDateIdAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("GetMappedUsageDateId/Get")]
        public void Get([FromBody] object data)
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
                    getMappedUsageDateIdAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, getMappedUsageDateIdAPIId);

                //Get MeterType
                var meterType = jsonObject[_systemAPIRequiredDataKeyEnums.MeterType].ToString();

                //Get MeterId
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject); 

                //Get latest loaded usage
                var latestDateMapping = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);

                //Get Date dictionary
                dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary();

                //Get Loaded Usage Date Ids
                var latestDateMappingDateIds = latestDateMapping.Select(d => d.Field<long>("DateId")).ToList();

                loadedUsageDateDictionary = dateDictionary.Where(d => latestDateMappingDateIds.Contains(d.Value))
                    .ToDictionary(d => Convert.ToDateTime(d.Key), d => d.Value);
                earliestUsageDate = loadedUsageDateDictionary.Min(d => d.Key);
                latestUsageDate = loadedUsageDateDictionary.Max(d => d.Key);

                var futureDateIds = dateDictionary.Where(d => Convert.ToDateTime(d.Key) > latestUsageDate)
                    .Select(d => d.Value).ToList();

                //Get DateToForecastGroup
                dateToForecastGroupDictionary = _mappingMethods.DateToForecastGroup_GetDateForecastGroupDictionary();
                var futureDateToForecastGroupDictionary = dateToForecastGroupDictionary.Where(d => futureDateIds.Contains(d.Key))
                    .ToDictionary(d => d.Key, d => d.Value);

                //Get Future Date mappings
                var futureDateToUsageDateDictionary = new ConcurrentDictionary<long, long>(futureDateToForecastGroupDictionary.ToDictionary(f => f.Key, f => new long()));

                //Get DateToForecastAgent
                var dateToForecastAgentDictionary = _mappingMethods.DateToForecastAgent_GetDateForecastAgentDictionary();

                //Get ForecastAgents
                var forecastAgentDictionary = _demandForecastMethods.GetForecastAgentDictionary();
                var parallelOptions = new ParallelOptions{MaxDegreeOfParallelism = 5};

                Parallel.ForEach(futureDateToForecastGroupDictionary, parallelOptions, futureDateToForecastGroup => {
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

                //Bulk Insert new Date Mapping into DateMapping_Temp table
                var supplyMethods = new Methods.Supply();
                supplyMethods.DateMappingTemp_Insert(meterType, meterId, dataTable);

                //End date existing Date Mapping
                supplyMethods.DateMapping_Delete(meterType, meterId);

                //Insert new Date Mapping into DateMapping table
                supplyMethods.DateMapping_Insert(meterType, meterId, processQueueGUID);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getMappedUsageDateIdAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getMappedUsageDateIdAPIId, true, $"System Error Id {errorId}");
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