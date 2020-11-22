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

namespace CreateMonthForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateMonthForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateMonthForecastController> _logger;
        private readonly Int64 createMonthForecastAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CreateMonthForecastController(ILogger<CreateMonthForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateMonthForecastAPI, password);
            createMonthForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateMonthForecastAPI);
        }

        [HttpPost]
        [Route("CreateMonthForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(createMonthForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateMonthForecast/Create")]
        public void Create([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());
            if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateMonthForecastAPI, createMonthForecastAPIId, hostEnvironment, jsonObject))
            {
                return;
            }

            var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateMonthForecastApp\bin\Debug\netcoreapp3.1\CreateMonthForecastApp.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
            startInfo.Arguments = JsonConvert.SerializeObject(data.ToString());
            System.Diagnostics.Process.Start(startInfo);
        }
    }
}