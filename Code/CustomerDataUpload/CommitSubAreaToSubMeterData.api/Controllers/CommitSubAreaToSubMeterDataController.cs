using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitSubAreaToSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitSubAreaToSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitSubAreaToSubMeterDataController> _logger;
        private readonly Int64 commitSubAreaToSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitSubAreaToSubMeterDataController(ILogger<CommitSubAreaToSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitSubAreaToSubMeterDataAPI, password);
            commitSubAreaToSubMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitSubAreaToSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitSubAreaToSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitSubAreaToSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitSubAreaToSubMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitSubAreaToSubMeterDataApp\bin\Debug\netcoreapp3.1\CommitSubAreaToSubMeterDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitSubAreaToSubMeterDataAPI, 
                commitSubAreaToSubMeterDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}