using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateMeterUsageDataController> _logger;
        private readonly Int64 validateMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateMeterUsageDataController(ILogger<ValidateMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateMeterUsageDataAPI, password);
            validateMeterUsageDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("ValidateMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateMeterUsageData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateMeterUsageDataApp\bin\Debug\netcoreapp3.1\ValidateMeterUsageDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateMeterUsageDataAPI, 
                validateMeterUsageDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}