using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitSiteDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitSiteDataController> _logger;
        private readonly Int64 commitSiteDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitSiteDataController(ILogger<CommitSiteDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitSiteDataAPI, password);
            commitSiteDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitSiteDataAPI);
        }

        [HttpPost]
        [Route("CommitSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitSiteDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitSiteData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitSiteDataApp\bin\Debug\netcoreapp3.1\CommitSiteDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitSiteDataAPI, 
                commitSiteDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}