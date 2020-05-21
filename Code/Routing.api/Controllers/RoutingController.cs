using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Routing.api.Controllers
{
    [ApiController]
    [Route("Routing")]
    public class RoutingController : ControllerBase
    {
        private readonly ILogger<RoutingController> _logger;

        public RoutingController(ILogger<RoutingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Validate([FromBody] Routing data)
        {
            if(data.Page == "Login") {
                return Ok();
            }
            
            return null;
        }
    }
}
