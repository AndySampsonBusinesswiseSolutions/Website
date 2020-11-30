using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitFlexContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFlexContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitFlexContractDataController> _logger;
        private readonly Int64 commitFlexContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitFlexContractDataController(ILogger<CommitFlexContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitFlexContractDataAPI, password);
            commitFlexContractDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitFlexContractDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitFlexContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexContractData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitFlexContractDataApp\bin\Debug\netcoreapp3.1\CommitFlexContractDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitFlexContractDataAPI, 
                commitFlexContractDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}