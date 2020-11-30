using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitMeterToProfileClassData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToProfileClassDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterToProfileClassDataController> _logger;
        private readonly Int64 commitMeterToProfileClassDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterToProfileClassDataController(ILogger<CommitMeterToProfileClassDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToProfileClassDataAPI, password);
            commitMeterToProfileClassDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterToProfileClassDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToProfileClassData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitMeterToProfileClassDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToProfileClassData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitMeterToProfileClassDataApp\bin\Debug\netcoreapp3.1\CommitMeterToProfileClassDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitMeterToProfileClassDataAPI, 
                commitMeterToProfileClassDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}