using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitLocalDistributionZoneToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitLocalDistributionZoneToMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitLocalDistributionZoneToMeterDataController> _logger;
        private readonly Int64 commitLocalDistributionZoneToMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitLocalDistributionZoneToMeterDataController(ILogger<CommitLocalDistributionZoneToMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitLocalDistributionZoneToMeterDataAPI, password);
            commitLocalDistributionZoneToMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitLocalDistributionZoneToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitLocalDistributionZoneToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitLocalDistributionZoneToMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitLocalDistributionZoneToMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitLocalDistributionZoneToMeterDataApp\bin\Debug\netcoreapp3.1\CommitLocalDistributionZoneToMeterDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitLocalDistributionZoneToMeterDataAPI, 
                commitLocalDistributionZoneToMeterDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}