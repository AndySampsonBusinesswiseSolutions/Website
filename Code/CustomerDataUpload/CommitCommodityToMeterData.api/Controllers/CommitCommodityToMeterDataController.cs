using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitCommodityToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitCommodityToMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitCommodityToMeterDataController> _logger;
        private readonly Int64 commitCommodityToMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitCommodityToMeterDataController(ILogger<CommitCommodityToMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitCommodityToMeterDataAPI, password);
            commitCommodityToMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitCommodityToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitCommodityToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitCommodityToMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitCommodityToMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitCommodityToMeterDataApp\bin\Debug\netcoreapp3.1\CommitCommodityToMeterDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitCommodityToMeterDataAPI, 
                commitCommodityToMeterDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}