using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreFlexContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreFlexContractDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreFlexContractDataController> _logger;
        private readonly Int64 storeFlexContractDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreFlexContractDataController(ILogger<StoreFlexContractDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFlexContractDataAPI, password);
            storeFlexContractDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreFlexContractDataAPI);
        }

        [HttpPost]
        [Route("StoreFlexContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeFlexContractDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFlexContractData/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\StoreFlexContractDataApp\bin\Debug\netcoreapp3.1\StoreFlexContractDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreFlexContractDataAPI, 
                storeFlexContractDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}