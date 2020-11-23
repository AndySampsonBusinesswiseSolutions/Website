using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitSubMeterDataController> _logger;
        private readonly Int64 commitSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitSubMeterDataController(ILogger<CommitSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitSubMeterDataAPI, password);
            commitSubMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitSubMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitSubMeterDataApp\bin\Debug\netcoreapp3.1\CommitSubMeterDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitSubMeterDataAPI, 
                commitSubMeterDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}