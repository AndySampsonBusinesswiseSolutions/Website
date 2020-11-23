using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateSiteDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateSiteDataController> _logger;
        private readonly Int64 validateSiteDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateSiteDataController(ILogger<ValidateSiteDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateSiteDataAPI, password);
            validateSiteDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateSiteDataAPI);
        }

        [HttpPost]
        [Route("ValidateSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateSiteDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateSiteData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateSiteDataApp\bin\Debug\netcoreapp3.1\ValidateSiteDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateSiteDataAPI, 
                validateSiteDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}