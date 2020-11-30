using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitGridSupplyPointToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitGridSupplyPointToMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitGridSupplyPointToMeterDataController> _logger;
        private readonly Int64 commitGridSupplyPointToMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitGridSupplyPointToMeterDataController(ILogger<CommitGridSupplyPointToMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitGridSupplyPointToMeterDataAPI, password);
            commitGridSupplyPointToMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitGridSupplyPointToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitGridSupplyPointToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitGridSupplyPointToMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitGridSupplyPointToMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitGridSupplyPointToMeterDataApp\bin\Debug\netcoreapp3.1\CommitGridSupplyPointToMeterDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitGridSupplyPointToMeterDataAPI, 
                commitGridSupplyPointToMeterDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}