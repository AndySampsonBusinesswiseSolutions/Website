using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreCustomerData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreCustomerDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreCustomerDataController> _logger;
        private readonly Int64 storeCustomerDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreCustomerDataController(ILogger<StoreCustomerDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreCustomerDataAPI, password);
            storeCustomerDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreCustomerDataAPI);
        }

        [HttpPost]
        [Route("StoreCustomerData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeCustomerDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreCustomerData/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\StoreCustomerDataApp\bin\Debug\netcoreapp3.1\StoreCustomerDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreCustomerDataAPI, 
                storeCustomerDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}