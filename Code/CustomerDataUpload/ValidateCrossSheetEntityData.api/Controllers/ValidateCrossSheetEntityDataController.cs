using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ValidateCrossSheetEntityData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateCrossSheetEntityDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ValidateCrossSheetEntityDataController> _logger;
        private readonly Int64 validateCrossSheetEntityDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ValidateCrossSheetEntityDataController(ILogger<ValidateCrossSheetEntityDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateCrossSheetEntityDataAPI, password);
            validateCrossSheetEntityDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateCrossSheetEntityDataAPI);
        }

        [HttpPost]
        [Route("ValidateCrossSheetEntityData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(validateCrossSheetEntityDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ValidateCrossSheetEntityData/Validate")]
        public void Validate([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ValidateCrossSheetEntityDataApp\bin\Debug\netcoreapp3.1\ValidateCrossSheetEntityDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ValidateCrossSheetEntityDataAPI, 
                validateCrossSheetEntityDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}