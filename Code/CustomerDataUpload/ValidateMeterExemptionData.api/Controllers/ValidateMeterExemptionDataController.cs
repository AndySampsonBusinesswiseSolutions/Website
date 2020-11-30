using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateMeterExemptionData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateMeterExemptionDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateMeterExemptionDataController> _logger;
        private readonly Int64 ValidateMeterExemptionDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateMeterExemptionDataController(ILogger<ValidateMeterExemptionDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateMeterExemptionDataAPI, password);
            ValidateMeterExemptionDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("ValidateMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(ValidateMeterExemptionDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateMeterExemptionData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateMeterExemptionDataApp\bin\Debug\netcoreapp3.1\ValidateMeterExemptionDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateMeterExemptionDataAPI, 
                ValidateMeterExemptionDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}