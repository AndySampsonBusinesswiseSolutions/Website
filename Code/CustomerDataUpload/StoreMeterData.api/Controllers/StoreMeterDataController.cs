using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreMeterDataController> _logger;
        private readonly Int64 storeMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreMeterDataController(ILogger<StoreMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreMeterDataAPI, password);
            storeMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreMeterDataAPI);
        }

        [HttpPost]
        [Route("StoreMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreMeterData/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\StoreMeterDataApp\bin\Debug\netcoreapp3.1\StoreMeterDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreMeterDataAPI, 
                storeMeterDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}