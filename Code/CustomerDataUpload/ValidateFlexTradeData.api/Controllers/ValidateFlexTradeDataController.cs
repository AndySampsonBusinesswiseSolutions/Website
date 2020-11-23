using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateFlexTradeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFlexTradeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateFlexTradeDataController> _logger;
        private readonly Int64 validateFlexTradeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateFlexTradeDataController(ILogger<ValidateFlexTradeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateFlexTradeDataAPI, password);
            validateFlexTradeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateFlexTradeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFlexTradeData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateFlexTradeDataApp\bin\Debug\netcoreapp3.1\ValidateFlexTradeDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateFlexTradeDataAPI, 
                validateFlexTradeDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}