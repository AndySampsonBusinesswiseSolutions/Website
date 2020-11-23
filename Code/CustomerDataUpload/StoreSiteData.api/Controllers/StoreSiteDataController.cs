using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreSiteData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreSiteDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreSiteDataController> _logger;
        private readonly Int64 storeSiteDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreSiteDataController(ILogger<StoreSiteDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreSiteDataAPI, password);
            storeSiteDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreSiteDataAPI);
        }

        [HttpPost]
        [Route("StoreSiteData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeSiteDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreSiteData/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\StoreSiteDataApp\bin\Debug\netcoreapp3.1\StoreSiteDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreSiteDataAPI, 
                storeSiteDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}