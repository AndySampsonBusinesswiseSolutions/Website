using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CreateDataAnalysisWebpage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateDataAnalysisWebpageController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateDataAnalysisWebpageController> _logger;
        private readonly Int64 createDataAnalysisWebpageAPIId;
        private string hostEnvironment;
        #endregion

        public CreateDataAnalysisWebpageController(ILogger<CreateDataAnalysisWebpageController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateDataAnalysisWebpageAPI, password);
            createDataAnalysisWebpageAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateDataAnalysisWebpageAPI);
        }

        [HttpPost]
        [Route("CreateDataAnalysisWebpage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(createDataAnalysisWebpageAPIId, hostEnvironment, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateDataAnalysisWebpage/BuildLocationTree")]
        public void BuildLocationTree([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\Webpage\CreateDataAnalysisWebpageApp\bin\Debug\netcoreapp3.1\CreateDataAnalysisWebpageApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CreateDataAnalysisWebpageAPI, 
                createDataAnalysisWebpageAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}