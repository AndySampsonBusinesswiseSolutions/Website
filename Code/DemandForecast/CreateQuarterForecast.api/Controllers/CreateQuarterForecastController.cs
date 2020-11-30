using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CreateQuarterForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateQuarterForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateQuarterForecastController> _logger;
        private readonly Int64 createQuarterForecastAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CreateQuarterForecastController(ILogger<CreateQuarterForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateQuarterForecastAPI, password);
            createQuarterForecastAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateQuarterForecastAPI);
        }

        [HttpPost]
        [Route("CreateQuarterForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(createQuarterForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateQuarterForecast/Create")]
        public void Create([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateQuarterForecastApp\bin\Debug\netcoreapp3.1\CreateQuarterForecastApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CreateQuarterForecastAPI, 
                createQuarterForecastAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}