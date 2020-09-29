using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

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
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Information.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();
        private readonly Int64 createForecastUsageAPIId;

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
                var meterId = _customerMethods.GetMeterIdByMeterType(meterType, jsonObject);

                //Call GetMappedUsageDateId API and wait for response
                var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.GetMappedUsageDateIdAPI);
                var API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.GetProfileAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync().Result.Replace("\"", string.Empty).Replace("\\", string.Empty);

                var dateMappings = _supplyMethods.DateMapping_GetLatest(meterType, meterId);
                var futureDateToUsageDateDictionary = dateMappings.ToDictionary(
                    d => d.Field<long>("DateId"),
                    d => d.Field<long>("MappedDateId")
                );

                if(!futureDateToUsageDateDictionary.Any() || futureDateToUsageDateDictionary.Any(f => f.Value == 0))
                {
                    //throw error as mapping has failed
                    var errorMessage = futureDateToUsageDateDictionary.Any() 
                        ? $"Forecast date ids without mapped usage date id: {string.Join(',', futureDateToUsageDateDictionary.Where(f => f.Value == 0))}"
                        : $"No forecast date ids mapped to usage date ids";
                    var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, errorMessage, "Forecast Date Mapping", Environment.StackTrace);

                    //Update Process Queue
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createForecastUsageAPIId, true, $"System Error Id {errorId}");
                    return;
                }

                //Launch Forecast API for each granularity
                var forecastAPIGUIDGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.ForecastAPIGUID);
                var forecastAPIGUIDList = _informationMethods.GranularityDetail_GetGranularityDetailDescriptionListByGranularityAttributeId(forecastAPIGUIDGranularityAttributeId);

                var parallelOptions = new ParallelOptions{MaxDegreeOfParallelism = 5};
                Parallel.ForEach(forecastAPIGUIDList, parallelOptions, forecastAPIGUID => {
                    var APIId = _systemMethods.API_GetAPIIdByAPIGUID(forecastAPIGUID);
                    var API = _systemMethods.PostAsJsonAsync(APIId, _systemAPIGUIDEnums.CreateForecastUsageAPI, jsonObject);
                    var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync().Result;
                });

                //TODO: check if forecasts have worked
                //TODO: if all ok, email customer

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
    }
}