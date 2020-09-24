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
using System.Net.Http;

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
        private Dictionary<string, long> loadedUsageDateDictionary;
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
        public string Get([FromBody] object data)
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
                var latestLoadedUsage = _supplyMethods.LoadedUsage_GetLatest(meterType, meterId);

                //Get Date dictionary
                dateDictionary = _informationMethods.Date_GetDateDescriptionIdDictionary();
                var futureDateIds = dateDictionary.Where(d => Convert.ToDateTime(d.Key) > DateTime.Today)
                    .Select(d => d.Value).ToList();

                //Get Loaded Usage Date Ids
                var latestLoadedUsageDateIds = latestLoadedUsage.Select(d => d.Field<long>("DateId")).ToList();

                loadedUsageDateDictionary = dateDictionary.Where(d => latestLoadedUsageDateIds.Contains(d.Value))
                    .ToDictionary(d => d.Key, d => d.Value);
                earliestUsageDate = loadedUsageDateDictionary.Min(d => Convert.ToDateTime(d.Key));
                latestUsageDate = loadedUsageDateDictionary.Max(d => Convert.ToDateTime(d.Key));

                //Get DateToForecastGroup
                dateToForecastGroupDictionary = _mappingMethods.DateToForecastGroup_GetDateForecastGroupDictionary();
                var futureDateToForecastGroupDictionary = dateToForecastGroupDictionary.Where(d => futureDateIds.Contains(d.Key))
                    .ToDictionary(d => d.Key, d => d.Value);

                //Get Future Date mappings
                var futureDateToUsageDateDictionary = new ConcurrentDictionary<long, long>(futureDateToForecastGroupDictionary.ToDictionary(f => f.Key, f => new long()));

                //Clone jsonObject
                var newJsonObject = (JObject)jsonObject.DeepClone();

                //Add Future Date Id to newJsonObject
                newJsonObject.Add(_systemAPIRequiredDataKeyEnums.FutureDateId, 0);

                //Add Earliest and Latest Usage Date to newJsonObject
                newJsonObject.Add(_systemAPIRequiredDataKeyEnums.EarliestUsageDate, earliestUsageDate);
                newJsonObject.Add(_systemAPIRequiredDataKeyEnums.LatestUsageDate, latestUsageDate);

                var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.MapFutureDateIdToUsageDateIdAPI);
                var parallelOptions = new ParallelOptions{MaxDegreeOfParallelism = 5};

                Parallel.ForEach(futureDateToForecastGroupDictionary, parallelOptions, futureDateToForecastGroup => {
                    //Clone jsonObject
                    var APIJsonObject = (JObject)newJsonObject.DeepClone();

                    APIJsonObject[_systemAPIRequiredDataKeyEnums.FutureDateId] = futureDateToForecastGroup.Key;

                    //Call MapFutureDateIdToUsageDateId.api
                    var API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.GetMappedUsageDateIdAPI, APIJsonObject);
                    var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync().Result.ToString().Replace("\"", string.Empty);

                    if(!string.IsNullOrWhiteSpace(result))
                    {
                        var futureDateId = Convert.ToInt64(result.Split(',')[0]);
                        var usageDateId = Convert.ToInt64(result.Split(',')[1]);

                        futureDateToUsageDateDictionary[futureDateId] = usageDateId;
                    }
                });

                

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getMappedUsageDateIdAPIId, false, null);

                return JsonConvert.SerializeObject(futureDateToUsageDateDictionary);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, getMappedUsageDateIdAPIId, true, $"System Error Id {errorId}");

                return string.Empty;
            }
        }
    }
}