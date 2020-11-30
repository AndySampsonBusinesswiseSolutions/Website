using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateFlexReferenceVolumeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateFlexReferenceVolumeDataController> _logger;
        private readonly Int64 validateFlexReferenceVolumeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateFlexReferenceVolumeDataController(ILogger<ValidateFlexReferenceVolumeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateFlexReferenceVolumeDataAPI, password);
            validateFlexReferenceVolumeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("ValidateFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateFlexReferenceVolumeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateFlexReferenceVolumeData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateFlexReferenceVolumeDataApp\bin\Debug\netcoreapp3.1\ValidateFlexReferenceVolumeDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateFlexReferenceVolumeDataAPI, 
                validateFlexReferenceVolumeDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}