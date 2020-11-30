using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

using Microsoft.Extensions.Configuration;

namespace ValidateSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateSubMeterUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateSubMeterUsageDataController> _logger;
        private readonly Int64 validateSubMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateSubMeterUsageDataController(ILogger<ValidateSubMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateSubMeterUsageDataAPI, password);
            validateSubMeterUsageDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("ValidateSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateSubMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateSubMeterUsageData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateSubMeterUsageDataApp\bin\Debug\netcoreapp3.1\ValidateSubMeterUsageDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateSubMeterUsageDataAPI, 
                validateSubMeterUsageDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}