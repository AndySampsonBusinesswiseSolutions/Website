using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitBasketData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitBasketDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitBasketDataController> _logger;
        private readonly Int64 commitBasketDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitBasketDataController(ILogger<CommitBasketDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitBasketDataAPI, password);
            commitBasketDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitBasketDataAPI);
        }

        [HttpPost]
        [Route("CommitBasketData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitBasketDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitBasketData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitBasketDataApp\bin\Debug\netcoreapp3.1\CommitBasketDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitBasketDataAPI, 
                commitBasketDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}