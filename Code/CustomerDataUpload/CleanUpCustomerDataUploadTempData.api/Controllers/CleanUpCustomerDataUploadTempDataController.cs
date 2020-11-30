using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CleanUpCustomerDataUploadTempData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CleanUpCustomerDataUploadTempDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CleanUpCustomerDataUploadTempDataController> _logger;
        private readonly Int64 cleanUpCustomerDataUploadTempDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CleanUpCustomerDataUploadTempDataController(ILogger<CleanUpCustomerDataUploadTempDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CleanUpCustomerDataUploadTempDataAPI, password);
            cleanUpCustomerDataUploadTempDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CleanUpCustomerDataUploadTempDataAPI);
        }

        [HttpPost]
        [Route("CleanUpCustomerDataUploadTempData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(cleanUpCustomerDataUploadTempDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CleanUpCustomerDataUploadTempData/Clean")]
        public void Clean([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CleanUpCustomerDataUploadTempDataApp\bin\Debug\netcoreapp3.1\CleanUpCustomerDataUploadTempDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CleanUpCustomerDataUploadTempDataAPI, 
                cleanUpCustomerDataUploadTempDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}