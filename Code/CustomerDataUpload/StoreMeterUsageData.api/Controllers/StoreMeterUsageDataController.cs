using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreMeterUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreMeterUsageDataController> _logger;
        private readonly Int64 storeMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreMeterUsageDataController(ILogger<StoreMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreMeterUsageDataAPI, password);
            storeMeterUsageDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("StoreMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreMeterUsageData/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\StoreMeterUsageDataApp\bin\Debug\netcoreapp3.1\StoreMeterUsageDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreMeterUsageDataAPI, 
                storeMeterUsageDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}