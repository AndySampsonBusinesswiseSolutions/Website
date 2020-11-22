using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CreateFiveMinuteForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateFiveMinuteForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateFiveMinuteForecastController> _logger;
        private readonly Int64 createFiveMinuteForecastAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CreateFiveMinuteForecastController(ILogger<CreateFiveMinuteForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateFiveMinuteForecastAPI, password);
            createFiveMinuteForecastAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateFiveMinuteForecastAPI);
        }

        [HttpPost]
        [Route("CreateFiveMinuteForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(createFiveMinuteForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateFiveMinuteForecast/Create")]
        public void Create([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateFiveMinuteForecastApp\bin\Debug\netcoreapp3.1\CreateFiveMinuteForecastApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CreateFiveMinuteForecastAPI, 
                createFiveMinuteForecastAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}