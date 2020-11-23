using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateSubMeterDataController> _logger;
        private readonly Int64 validateSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateSubMeterDataController(ILogger<ValidateSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateSubMeterDataAPI, password);
            validateSubMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateSubMeterDataAPI);
        }

        [HttpPost]
        [Route("ValidateSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateSubMeterData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateSubMeterDataApp\bin\Debug\netcoreapp3.1\ValidateSubMeterDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateSubMeterDataAPI, 
                validateSubMeterDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}