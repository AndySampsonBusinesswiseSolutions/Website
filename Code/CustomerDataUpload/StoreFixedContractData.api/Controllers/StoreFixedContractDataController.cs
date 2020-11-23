using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreFixedContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreFixedContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreFixedContractDataController> _logger;
        private readonly Int64 storeFixedContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreFixedContractDataController(ILogger<StoreFixedContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFixedContractDataAPI, password);
            storeFixedContractDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreFixedContractDataAPI);
        }

        [HttpPost]
        [Route("StoreFixedContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeFixedContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFixedContractData/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\StoreFixedContractDataApp\bin\Debug\netcoreapp3.1\StoreFixedContractDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreFixedContractDataAPI, 
                storeFixedContractDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}