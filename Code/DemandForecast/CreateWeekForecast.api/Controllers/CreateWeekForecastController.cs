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

namespace CreateWeekForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateWeekForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateWeekForecastController> _logger;
        private readonly Int64 createWeekForecastAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CreateWeekForecastController(ILogger<CreateWeekForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateWeekForecastAPI, password);
            createWeekForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateWeekForecastAPI);
        }

        [HttpPost]
        [Route("CreateWeekForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(createWeekForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateWeekForecast/Create")]
        public void Create([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());
            if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateWeekForecastAPI, createWeekForecastAPIId, hostEnvironment, jsonObject))
            {
                return;
            }

            var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateWeekForecastApp\bin\Debug\netcoreapp3.1\CreateWeekForecastApp.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
            startInfo.Arguments = JsonConvert.SerializeObject(data.ToString());
            System.Diagnostics.Process.Start(startInfo);
        }
    }
}