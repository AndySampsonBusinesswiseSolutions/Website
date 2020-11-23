using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreSubMeterDataController> _logger;
        private readonly Int64 storeSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreSubMeterDataController(ILogger<StoreSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreSubMeterDataAPI, password);
            storeSubMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreSubMeterDataAPI);
        }

        [HttpPost]
        [Route("StoreSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreSubMeterData/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\StoreSubMeterDataApp\bin\Debug\netcoreapp3.1\StoreSubMeterDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreSubMeterDataAPI, 
                storeSubMeterDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}