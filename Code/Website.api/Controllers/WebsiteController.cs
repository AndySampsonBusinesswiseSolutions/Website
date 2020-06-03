using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Website.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        private readonly ILogger<WebsiteController> _logger;

        public WebsiteController(ILogger<WebsiteController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("Website/Validate")]
        public IActionResult Validate([FromBody] Website data)
        {
            //validate PageGUID and ProcessGUID
            //if invalid return error

            var routingData = new Routing();
            routingData.Data = data.Data;
            routingData.ProcessGUID = data.ProcessGUID;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5002/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.PostAsJsonAsync("Routing", routingData);

            return Ok();
        }
    }
}
