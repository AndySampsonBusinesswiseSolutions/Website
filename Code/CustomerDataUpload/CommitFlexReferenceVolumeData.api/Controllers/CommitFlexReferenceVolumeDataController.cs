using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitFlexReferenceVolumeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitFlexReferenceVolumeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitFlexReferenceVolumeDataController> _logger;
        private readonly Int64 commitFlexReferenceVolumeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitFlexReferenceVolumeDataController(ILogger<CommitFlexReferenceVolumeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitFlexReferenceVolumeDataAPI, password);
            commitFlexReferenceVolumeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitFlexReferenceVolumeDataAPI);
        }

        [HttpPost]
        [Route("CommitFlexReferenceVolumeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitFlexReferenceVolumeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitFlexReferenceVolumeData/Commit")]
        public void Commit([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\CustomerDataUpload\CommitFlexReferenceVolumeDataApp\bin\Debug\netcoreapp3.1\CommitFlexReferenceVolumeDataApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CommitFlexReferenceVolumeDataAPI, 
                commitFlexReferenceVolumeDataAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}