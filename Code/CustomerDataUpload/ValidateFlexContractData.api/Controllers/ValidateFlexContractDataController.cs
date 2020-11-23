using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateFlexContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFlexContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateFlexContractDataController> _logger;
        private readonly Int64 validateFlexContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateFlexContractDataController(ILogger<ValidateFlexContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateFlexContractDataAPI, password);
            validateFlexContractDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateFlexContractDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateFlexContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFlexContractData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateFlexContractDataApp\bin\Debug\netcoreapp3.1\ValidateFlexContractDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateFlexContractDataAPI, 
                validateFlexContractDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}