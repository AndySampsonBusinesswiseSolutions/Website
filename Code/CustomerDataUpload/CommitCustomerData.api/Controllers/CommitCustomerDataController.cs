using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitCustomerData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitCustomerDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitCustomerDataController> _logger;
        private readonly Int64 commitCustomerDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitCustomerDataController(ILogger<CommitCustomerDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitCustomerDataAPI, password);
            commitCustomerDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitCustomerDataAPI);
        }

        [HttpPost]
        [Route("CommitCustomerData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitCustomerDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitCustomerData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitCustomerDataApp\bin\Debug\netcoreapp3.1\CommitCustomerDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitCustomerDataAPI, 
                commitCustomerDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}