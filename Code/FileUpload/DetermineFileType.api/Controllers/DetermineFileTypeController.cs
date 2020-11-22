using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace DetermineFileType.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class DetermineFileTypeController : ControllerBase
    {
        #region Variables
        private readonly ILogger<DetermineFileTypeController> _logger;
        private readonly Int64 determineFileTypeAPIId;
        private readonly string hostEnvironment;
        private Int64 fileId;
        #endregion

        public DetermineFileTypeController(ILogger<DetermineFileTypeController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().DetermineFileTypeAPI, password);
            determineFileTypeAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().DetermineFileTypeAPI);
        }

        [HttpPost]
        [Route("DetermineFileType/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(determineFileTypeAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("DetermineFileType/Determine")]
        public void Determine([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\Login\DetermineFileTypeApp\bin\Debug\netcoreapp3.1\DetermineFileTypeApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().DetermineFileTypeAPI, 
                determineFileTypeAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}