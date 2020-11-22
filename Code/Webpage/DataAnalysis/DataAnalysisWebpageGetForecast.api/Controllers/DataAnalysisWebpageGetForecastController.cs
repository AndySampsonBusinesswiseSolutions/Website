using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace DataAnalysisWebpageGetForecast.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class DataAnalysisWebpageGetForecastController : ControllerBase
    {
        #region Variables
        private readonly ILogger<DataAnalysisWebpageGetForecastController> _logger;
        private readonly Int64 dataAnalysisWebpageGetForecastAPIId;
        private string hostEnvironment;
        #endregion

        public DataAnalysisWebpageGetForecastController(ILogger<DataAnalysisWebpageGetForecastController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().DataAnalysisWebpageGetForecastAPI, password);
            dataAnalysisWebpageGetForecastAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().DataAnalysisWebpageGetForecastAPI);
        }

        [HttpPost]
        [Route("DataAnalysisWebpageGetForecast/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(dataAnalysisWebpageGetForecastAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("DataAnalysisWebpageGetForecast/GetForecast")]
        public void GetForecast([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\Webpage\DataAnalysis\DataAnalysisWebpageGetForecastApp\bin\Debug\netcoreapp3.1\DataAnalysisWebpageGetForecastApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().DataAnalysisWebpageGetForecastAPI, 
                dataAnalysisWebpageGetForecastAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}