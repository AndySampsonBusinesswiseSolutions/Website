using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

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
            createDateForecastAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateDateForecastAPI);
        }

        [HttpPost]
        [Route("CreateDateForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(createDateForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateDateForecast/Create")]
        public void Create([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\DemandForecast\CreateDateForecastApp\bin\Debug\netcoreapp3.1\CreateDateForecastApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CreateDateForecastAPI, 
                createDateForecastAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}