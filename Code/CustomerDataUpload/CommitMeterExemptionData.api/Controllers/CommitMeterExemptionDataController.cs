using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitMeterExemptionData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterExemptionDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterExemptionDataController> _logger;
        private readonly Int64 commitMeterExemptionDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterExemptionDataController(ILogger<CommitMeterExemptionDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterExemptionDataAPI, password);
            commitMeterExemptionDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitMeterExemptionDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterExemptionData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitMeterExemptionDataApp\bin\Debug\netcoreapp3.1\CommitMeterExemptionDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitMeterExemptionDataAPI, 
                commitMeterExemptionDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}