using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace LockUser.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class LockUserController : ControllerBase
    {
        #region Variables
        private readonly ILogger<LockUserController> _logger;
        private readonly Int64 lockUserAPIId;
        private readonly string hostEnvironment;
        #endregion

        public LockUserController(ILogger<LockUserController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().LockUserAPI, password);
            lockUserAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().LockUserAPI);
        }

        [HttpPost]
        [Route("LockUser/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(lockUserAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("LockUser/Lock")]
        public void Lock([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\Login\LockUserApp\bin\Debug\netcoreapp3.1\LockUserApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().LockUserAPI, 
                lockUserAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}