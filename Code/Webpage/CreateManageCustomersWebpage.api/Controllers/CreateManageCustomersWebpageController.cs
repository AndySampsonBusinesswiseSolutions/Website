using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CreateManageCustomersWebpage.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CreateManageCustomersWebpageController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CreateManageCustomersWebpageController> _logger;
        private readonly Int64 createManageCustomersWebpageAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CreateManageCustomersWebpageController(ILogger<CreateManageCustomersWebpageController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CreateManageCustomersWebpageAPI, password);
            createManageCustomersWebpageAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CreateManageCustomersWebpageAPI);
        }

        [HttpPost]
        [Route("CreateManageCustomersWebpage/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(createManageCustomersWebpageAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CreateManageCustomersWebpage/Create")]
        public void Create([FromBody] object data)
        {
            var fileName = @"C:\wamp64\www\Website\Code\Webpage\CreateManageCustomersWebpageApp\bin\Debug\netcoreapp3.1\CreateManageCustomersWebpageApp.exe";
            new Methods.SystemSchema.Application().LaunchApplication(
                data, 
                new Enums.SystemSchema.API.GUID().CreateManageCustomersWebpageAPI, 
                createManageCustomersWebpageAPIId, 
                hostEnvironment, 
                fileName
            );
        }
    }
}