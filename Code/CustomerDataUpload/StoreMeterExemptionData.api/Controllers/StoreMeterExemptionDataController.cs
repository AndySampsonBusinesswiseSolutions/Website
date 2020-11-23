using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreMeterExemptionData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreMeterExemptionDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreMeterExemptionDataController> _logger;
        private readonly Int64 storeMeterExemptionDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreMeterExemptionDataController(ILogger<StoreMeterExemptionDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreMeterExemptionDataAPI, password);
            storeMeterExemptionDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("StoreMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeMeterExemptionDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreMeterExemptionData/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\StoreMeterExemptionDataApp\bin\Debug\netcoreapp3.1\StoreMeterExemptionDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreMeterExemptionDataAPI, 
                storeMeterExemptionDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}