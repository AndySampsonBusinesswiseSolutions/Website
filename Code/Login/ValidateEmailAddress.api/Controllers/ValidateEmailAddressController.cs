using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateEmailAddress.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateEmailAddressController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateEmailAddressController> _logger;
        private readonly Int64 validateEmailAddressAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateEmailAddressController(ILogger<ValidateEmailAddressController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateEmailAddressAPI, password);
            validateEmailAddressAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateEmailAddressAPI);
        }

        [HttpPost]
        [Route("ValidateEmailAddress/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateEmailAddressAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateEmailAddress/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\Login\ValidateEmailAddressApp\bin\Debug\netcoreapp3.1\ValidateEmailAddressApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateEmailAddressAPI, 
                validateEmailAddressAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}