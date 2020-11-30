using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateMeterDataController> _logger;
        private readonly Int64 validateMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateMeterDataController(ILogger<ValidateMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateMeterDataAPI, password);
            validateMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateMeterDataAPI);
        }

        [HttpPost]
        [Route("ValidateMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateMeterData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateMeterDataApp\bin\Debug\netcoreapp3.1\ValidateMeterDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateMeterDataAPI, 
                validateMeterDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}