using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace CreateYearForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateYearForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateYearForecastController> _logger;
        private readonly Int64 createYearForecastAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CreateYearForecastController(ILogger<CreateYearForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateYearForecastAPI, password);
            createYearForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateYearForecastAPI);
        }

        [HttpPost]
        [Route("CreateYearForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(createYearForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateYearForecast/Create")]
        public void Create([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());
            if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateYearForecastAPI, createYearForecastAPIId, hostEnvironment, jsonObject))
            {
                return;
            }

            var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateYearForecastApp\bin\Debug\netcoreapp3.1\CreateYearForecastApp.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
            startInfo.Arguments = JsonConvert.SerializeObject(data.ToString());
            System.Diagnostics.Process.Start(startInfo);
        }
    }
}