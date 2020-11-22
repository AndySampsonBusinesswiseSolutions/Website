using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CreateHalfHourForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateHalfHourForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateHalfHourForecastController> _logger;
        private readonly Int64 createHalfHourForecastAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CreateHalfHourForecastController(ILogger<CreateHalfHourForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateHalfHourForecastAPI, password);
            createHalfHourForecastAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateHalfHourForecastAPI);
        }

        [HttpPost]
        [Route("CreateHalfHourForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(createHalfHourForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateHalfHourForecast/Create")]
        public void Create([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateHalfHourForecastApp\bin\Debug\netcoreapp3.1\CreateHalfHourForecastApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CreateHalfHourForecastAPI, 
                createHalfHourForecastAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}