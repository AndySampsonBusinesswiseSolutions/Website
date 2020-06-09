using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
using Newtonsoft.Json.Linq;

namespace ValidateProcessGUID.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidateProcessGUIDController : ControllerBase
    {
        private readonly ILogger<ValidateProcessGUIDController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("ValidateProcessGUID.api", @"Y4c?.KT(>HXj@f8D");

        public ValidateProcessGUIDController(ILogger<ValidateProcessGUIDController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("ValidateProcessGUID/Validate")]
        public long Validate([FromBody] object data)
        {
            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject["QueueGUID"].ToString();

            //Insert into ProcessQueue
            _processMethods.ProcessQueue_Insert(_databaseInteraction, queueGUID, "743E21EE-2185-45D4-9003-E35060B751E2", "User Generated", "87AFEDA8-6A0F-4143-BF95-E08E78721CF5");

            //Get Process GUID
            var processGUID = jsonObject["ProcessGUID"].ToString();

            //Validate Process GUID
            var processId = _processMethods.ProcessId_GetByGUID(_databaseInteraction, processGUID);

            //If processId == 0 then the GUID provided isn't valid so create an error
            if(processId == 0)
            {
                
            }

            //Update Process Queue
            _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, "87AFEDA8-6A0F-4143-BF95-E08E78721CF5", processId == 0);

            return processId;
        }
    }
}
