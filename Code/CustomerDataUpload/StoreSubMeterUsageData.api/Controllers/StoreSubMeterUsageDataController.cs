using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreSubMeterUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreSubMeterUsageDataController> _logger;
        private readonly Int64 storeSubMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreSubMeterUsageDataController(ILogger<StoreSubMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreSubMeterUsageDataAPI, password);
            storeSubMeterUsageDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("StoreSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeSubMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreSubMeterUsageData/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\StoreSubMeterUsageDataApp\bin\Debug\netcoreapp3.1\StoreSubMeterUsageDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreSubMeterUsageDataAPI, 
                storeSubMeterUsageDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}