using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using databaseInteraction;

namespace Website.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        private readonly ILogger<WebsiteController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("Website.api", @"\wU.D[ArWjPG!F4$");

        public WebsiteController(ILogger<WebsiteController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("Website/Validate")]
        public IActionResult Validate([FromBody] object data)
        {
            //Get Routing.API URL
            var routingAPIURL = _apiMethods.GetRoutingAPIURL(_databaseInteraction);

            //Get Routing.API POST Route
            var routingAPIPOSTRoute = _apiMethods.GetRoutingAPIPOSTRoute(_databaseInteraction);

            //Connect to Routing API and POST data
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(routingAPIURL);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.PostAsJsonAsync(routingAPIPOSTRoute, data);

            return Ok();
        }
    }
}
