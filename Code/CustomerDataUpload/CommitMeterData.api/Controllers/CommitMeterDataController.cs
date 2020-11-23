using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterDataController> _logger;
        private readonly Int64 commitMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterDataController(ILogger<CommitMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterDataAPI, password);
            commitMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitMeterDataApp\bin\Debug\netcoreapp3.1\CommitMeterDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitMeterDataAPI, 
                commitMeterDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}