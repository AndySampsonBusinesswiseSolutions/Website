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
    [Route("Website")]
    public class WebsiteController : ControllerBase
    {
        private readonly ILogger<WebsiteController> _logger;

        public WebsiteController(ILogger<WebsiteController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Validate([FromBody] Website data)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5002/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.PostAsJsonAsync("Routing", data).Result;

            return Ok();
        }
    }
}
