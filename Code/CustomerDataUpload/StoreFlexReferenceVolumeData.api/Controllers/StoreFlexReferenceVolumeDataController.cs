using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace StoreFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreFlexReferenceVolumeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<StoreFlexReferenceVolumeDataController> _logger;
        private readonly Int64 storeFlexReferenceVolumeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public StoreFlexReferenceVolumeDataController(ILogger<StoreFlexReferenceVolumeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFlexReferenceVolumeDataAPI, password);
            storeFlexReferenceVolumeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("StoreFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(storeFlexReferenceVolumeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("StoreFlexReferenceVolumeData/Store")]
        public void Store([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\StoreFlexReferenceVolumeDataApp\bin\Debug\netcoreapp3.1\StoreFlexReferenceVolumeDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().StoreFlexReferenceVolumeDataAPI, 
                storeFlexReferenceVolumeDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}