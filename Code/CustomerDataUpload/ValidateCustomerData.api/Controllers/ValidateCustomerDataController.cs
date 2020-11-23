using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateCustomerData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateCustomerDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateCustomerDataController> _logger;
        private readonly Int64 validateCustomerDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateCustomerDataController(ILogger<ValidateCustomerDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateCustomerDataAPI, password);
            validateCustomerDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateCustomerDataAPI);
        }

        [HttpPost]
        [Route("ValidateCustomerData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateCustomerDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateCustomerData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateCustomerDataApp\bin\Debug\netcoreapp3.1\ValidateCustomerDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateCustomerDataAPI, 
                validateCustomerDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}