using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

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
            createYearForecastAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateYearForecastAPI);
        }

        [HttpPost]
        [Route("CreateYearForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(createYearForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateYearForecast/Create")]
        public void Create([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateYearForecastApp\bin\Debug\netcoreapp3.1\CreateYearForecastApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CreateYearForecastAPI, 
                createYearForecastAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}