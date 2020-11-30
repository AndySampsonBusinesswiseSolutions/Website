using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitCustomerToSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitCustomerToSiteDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitCustomerToSiteDataController> _logger;
        private readonly Int64 commitCustomerToSiteDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitCustomerToSiteDataController(ILogger<CommitCustomerToSiteDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitCustomerToSiteDataAPI, password);
            commitCustomerToSiteDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitCustomerToSiteDataAPI);
        }

        [HttpPost]
        [Route("CommitCustomerToSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitCustomerToSiteDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitCustomerToSiteData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitCustomerToSiteDataApp\bin\Debug\netcoreapp3.1\CommitCustomerToSiteDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitCustomerToSiteDataAPI, 
                commitCustomerToSiteDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}