using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitMeterToMeterTimeswitchCodeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToMeterTimeswitchCodeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterToMeterTimeswitchCodeDataController> _logger;
        private readonly Int64 commitMeterToMeterTimeswitchCodeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterToMeterTimeswitchCodeDataController(ILogger<CommitMeterToMeterTimeswitchCodeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToMeterTimeswitchCodeDataAPI, password);
            commitMeterToMeterTimeswitchCodeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterToMeterTimeswitchCodeDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToMeterTimeswitchCodeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitMeterToMeterTimeswitchCodeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToMeterTimeswitchCodeData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitMeterToMeterTimeswitchCodeDataApp\bin\Debug\netcoreapp3.1\CommitMeterToMeterTimeswitchCodeDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitMeterToMeterTimeswitchCodeDataAPI, 
                commitMeterToMeterTimeswitchCodeDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}