using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateFixedContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFixedContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateFixedContractDataController> _logger;
        private readonly Int64 validateFixedContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateFixedContractDataController(ILogger<ValidateFixedContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateFixedContractDataAPI, password);
            validateFixedContractDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateFixedContractDataAPI);
        }

        [HttpPost]
        [Route("ValidateFixedContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateFixedContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFixedContractData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateFlexTradeDataApp\bin\Debug\netcoreapp3.1\ValidateFlexTradeDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateFlexTradeDataAPI, 
                validateFixedContractDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}