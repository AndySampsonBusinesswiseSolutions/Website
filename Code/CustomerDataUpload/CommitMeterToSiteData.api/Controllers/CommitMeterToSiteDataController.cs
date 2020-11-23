using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitMeterToSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToSiteDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterToSiteDataController> _logger;
        private readonly Int64 commitMeterToSiteDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterToSiteDataController(ILogger<CommitMeterToSiteDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToSiteDataAPI, password);
            commitMeterToSiteDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterToSiteDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitMeterToSiteDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToSiteData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitMeterToSiteDataApp\bin\Debug\netcoreapp3.1\CommitMeterToSiteDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitMeterToSiteDataAPI, 
                commitMeterToSiteDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}