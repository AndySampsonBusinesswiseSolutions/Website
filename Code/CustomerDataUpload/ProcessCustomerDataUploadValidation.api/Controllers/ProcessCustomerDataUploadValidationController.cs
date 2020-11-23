using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace ProcessCustomerDataUploadValidation.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ProcessCustomerDataUploadValidationController : ControllerBase
    {
        #region Variables
        private readonly ILogger<ProcessCustomerDataUploadValidationController> _logger;
        private readonly Int64 processCustomerDataUploadValidationAPIId;
        private readonly string hostEnvironment;
        #endregion

        public ProcessCustomerDataUploadValidationController(ILogger<ProcessCustomerDataUploadValidationController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ProcessCustomerDataUploadValidationAPI, password);
            processCustomerDataUploadValidationAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ProcessCustomerDataUploadValidationAPI);
        }

        [HttpPost]
        [Route("ProcessCustomerDataUploadValidation/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(processCustomerDataUploadValidationAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("ProcessCustomerDataUploadValidation/Process")]
        public void Process([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\ProcessCustomerDataUploadValidationApp\bin\Debug\netcoreapp3.1\ProcessCustomerDataUploadValidationApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().ProcessCustomerDataUploadValidationAPI, 
                processCustomerDataUploadValidationAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}