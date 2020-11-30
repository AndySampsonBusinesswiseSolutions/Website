using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace UploadFile.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        #region Variables
        private readonly ILogger<UploadFileController> _logger;
        private readonly Int64 uploadFileAPIId;
        private readonly string hostEnvironment;
        #endregion

        public UploadFileController(ILogger<UploadFileController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().UploadFileAPI, password);
            uploadFileAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().UploadFileAPI);
        }

        [HttpPost]
        [Route("UploadFile/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(uploadFileAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("UploadFile/Upload")]
        public void Upload([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\ConsoleApplication\ConsoleApplication\FileUpload\UploadFileApp\bin\Debug\netcoreapp3.1\UploadFileApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().UploadFileAPI, 
                uploadFileAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}