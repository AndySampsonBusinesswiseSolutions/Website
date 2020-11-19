using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CreateForecastUsage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateForecastUsageController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateForecastUsageController> _logger;
        private readonly Int64 createForecastUsageAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CreateForecastUsageController(ILogger<CreateForecastUsageController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateForecastUsageAPI, password);
            createForecastUsageAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateForecastUsageAPI);
        }

        [HttpPost]
        [Route("CreateForecastUsage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(createForecastUsageAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateForecastUsage/Create")]
        public void Create([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

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
                    createForecastUsageAPIId);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, createForecastUsageAPIId);

                var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
                var systemAPIMethods = new Methods.System.API();

                //Get MeterType
                var meterType = jsonObject[new Enums.SystemSchema.API.RequiredDataKey().MeterType].ToString();

                //Get MeterId
                var meterId = new Methods.Customer().GetMeterIdByMeterType(meterType, jsonObject);

                //Call GetMappedUsageDateId API and wait for response
                var getMappedUsageDateIdAPIId = systemAPIMethods.API_GetAPIIdByAPIGUID(systemAPIGUIDEnums.GetMappedUsageDateIdAPI);
                systemAPIMethods.PostAsJsonAsyncAndAwaitResult(getMappedUsageDateIdAPIId, systemAPIGUIDEnums.GetProfileAPI, hostEnvironment, jsonObject);

                var dateMappings = new Methods.Supply().DateMapping_GetLatestDictionary(meterType, meterId);

                if(!dateMappings.Any() || dateMappings.Any(d => d.Value == 0))
                {
                    //throw error as mapping has failed
                    var errorMessage = dateMappings.Any() 
                        ? $"Forecast date ids without mapped usage date id: {string.Join(',', dateMappings.Where(d => d.Value == 0))}"
                        : $"No forecast date ids mapped to usage date ids";
                    var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, errorMessage, "Forecast Date Mapping", Environment.StackTrace);

                    //Update Process Queue
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createForecastUsageAPIId, true, $"System Error Id {errorId}");
                    return;
                }

                //TODO: check if forecasts have worked
                //TODO: if all ok, email customer

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createForecastUsageAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, createForecastUsageAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}