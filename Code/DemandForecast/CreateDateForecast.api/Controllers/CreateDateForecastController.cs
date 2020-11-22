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

namespace CreateDateForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateDateForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateDateForecastController> _logger;
        private readonly Int64 createDateForecastAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CreateDateForecastController(ILogger<CreateDateForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateDateForecastAPI, password);
            createDateForecastAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateDateForecastAPI);
        }

        [HttpPost]
        [Route("CreateDateForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(createDateForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateDateForecast/Create")]
        public void Create([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());
            if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CreateDateForecastAPI, createDateForecastAPIId, hostEnvironment, jsonObject))
            {
                return;
            }

            var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateDateForecastApp\bin\Debug\netcoreapp3.1\CreateDateForecastApp.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
            startInfo.Arguments = JsonConvert.SerializeObject(data.ToString());
            System.Diagnostics.Process.Start(startInfo);
        }
    }
}