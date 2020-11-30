using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreLoginAttempt.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreLoginAttemptController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreLoginAttemptController> _logger;
        private readonly Int64 storeLoginAttemptAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreLoginAttemptController(ILogger<StoreLoginAttemptController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreLoginAttemptAPI, password);
            storeLoginAttemptAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreLoginAttemptAPI);
        }

        [HttpPost]
        [Route("StoreLoginAttempt/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeLoginAttemptAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreLoginAttempt/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\Login\StoreLoginAttemptApp\bin\Debug\netcoreapp3.1\StoreLoginAttemptApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreLoginAttemptAPI, 
                storeLoginAttemptAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}