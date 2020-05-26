using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using databaseInteraction;

namespace ValidateEmailAddress.api.Controllers
{
    [ApiController]
    [Route("ValidateEmailAddress")]
    public class ValidateEmailAddressController : ControllerBase
    {
        private readonly ILogger<ValidateEmailAddressController> _logger;

        public ValidateEmailAddressController(ILogger<ValidateEmailAddressController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void Get([FromBody] ValidateEmailAddress data)
        {
            var databaseInteraction = new DatabaseInteraction();
            databaseInteraction.userId = "ValidateEmailAddress.api";
            databaseInteraction.password = @"~/@X@4Xc88$\~h;]";

            //get prerequisites
            //check if all completed or errored
            //if all completed:
                //insert entry into System.ProcessQueue
                //check email address against db
                //if valid email address, update System.ProcessQueue
                //if invalid, update System.ProcessQueue and write error
            //if errored:
                //insert entry into System.ProcessQueue and write error
        }
    }
}
