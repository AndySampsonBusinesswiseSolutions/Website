using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitMeterToSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterToSubMeterDataController> _logger;
        private readonly Int64 commitMeterToSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterToSubMeterDataController(ILogger<CommitMeterToSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToSubMeterDataAPI, password);
            commitMeterToSubMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterToSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitMeterToSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToSubMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitMeterToSubMeterDataApp\bin\Debug\netcoreapp3.1\CommitMeterToSubMeterDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitMeterToSubMeterDataAPI, 
                commitMeterToSubMeterDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}