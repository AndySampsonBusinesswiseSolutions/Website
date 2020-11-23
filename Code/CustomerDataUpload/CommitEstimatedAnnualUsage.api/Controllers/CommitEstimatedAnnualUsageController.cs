using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitEstimatedAnnualUsage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitEstimatedAnnualUsageController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitEstimatedAnnualUsageController> _logger;
        private readonly Int64 commitEstimatedAnnualUsageAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitEstimatedAnnualUsageController(ILogger<CommitEstimatedAnnualUsageController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitEstimatedAnnualUsageAPI, password);
            commitEstimatedAnnualUsageAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitEstimatedAnnualUsageAPI);
        }

        [HttpPost]
        [Route("CommitEstimatedAnnualUsage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitEstimatedAnnualUsageAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitEstimatedAnnualUsage/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitEstimatedAnnualUsageApp\bin\Debug\netcoreapp3.1\CommitEstimatedAnnualUsageApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitEstimatedAnnualUsageAPI, 
                commitEstimatedAnnualUsageAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}