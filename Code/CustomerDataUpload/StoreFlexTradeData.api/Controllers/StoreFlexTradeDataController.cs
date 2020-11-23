using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreFlexTradeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreFlexTradeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreFlexTradeDataController> _logger;
        private readonly Int64 storeFlexTradeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreFlexTradeDataController(ILogger<StoreFlexTradeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFlexTradeDataAPI, password);
            storeFlexTradeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreFlexTradeDataAPI);
        }

        [HttpPost]
        [Route("StoreFlexTradeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeFlexTradeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFlexTradeData/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\StoreFlexTradeDataApp\bin\Debug\netcoreapp3.1\StoreFlexTradeDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreFlexTradeDataAPI, 
                storeFlexTradeDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}