using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitContractDataController> _logger;
        private readonly Methods.SystemSchema.API _systemAPIMethods = new Methods.SystemSchema.API();
        private readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Int64 commitContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitContractDataController(ILogger<CommitContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitContractDataAPI, password);
            commitContractDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitContractDataAPI);
        }

        [HttpPost]
        [Route("CommitContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitContractData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitContractDataApp\bin\Debug\netcoreapp3.1\CommitContractDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitContractDataAPI, 
                commitContractDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}