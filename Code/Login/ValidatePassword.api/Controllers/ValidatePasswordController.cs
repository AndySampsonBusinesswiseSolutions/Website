using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidatePassword.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidatePasswordController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidatePasswordController> _logger;
        private readonly Int64 validatePasswordAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidatePasswordController(ILogger<ValidatePasswordController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidatePasswordAPI, password);
            validatePasswordAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidatePasswordAPI);
        }

        [HttpPost]
        [Route("ValidatePassword/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validatePasswordAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidatePassword/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\Login\ValidatePasswordApp\bin\Debug\netcoreapp3.1\ValidatePasswordApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidatePasswordAPI, 
                validatePasswordAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}