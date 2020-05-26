using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using databaseInteraction;

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
        public IActionResult Route([FromBody] Routing data)
        {
            var db = new DatabaseInteraction();
            db.userId = "Routing.api";
            

            if(data.Page == "Login") {
                return Ok();
            }
            
            return null;
        }
    }
}
