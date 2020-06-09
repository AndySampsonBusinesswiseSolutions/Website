using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System.Net.Http;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Website.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        private readonly ILogger<WebsiteController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("Website.api", @"\wU.D[ArWjPG!F4$");

        public WebsiteController(ILogger<WebsiteController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("Website/Validate")]
        public void Validate([FromBody] object data)
        {
            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject["QueueGUID"].ToString();

            //Insert into ProcessQueue
            _processMethods.ProcessQueue_Insert(_databaseInteraction, queueGUID, "743E21EE-2185-45D4-9003-E35060B751E2", "User Generated", "CBB27186-B65F-4F6C-9FFA-B1E6C63C04EE");

            //Get Routing.API URL
            var routingAPIId = _apiMethods.GetRoutingAPIId(_databaseInteraction);

            //Connect to Routing API and POST data
            _apiMethods.CreateAPI(_databaseInteraction, routingAPIId)
                    .PostAsJsonAsync(
                        _apiMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, routingAPIId), 
                        _apiMethods.GetAPIData(_databaseInteraction, routingAPIId, jsonObject));

            //Update Process Queue
            _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, "CBB27186-B65F-4F6C-9FFA-B1E6C63C04EE");
        }

        [HttpPost]
        [Route("website/GetResponse")]
        public IActionResult GetResponse([FromBody] string processQueueGuid)
        {
            //Get Process Archive Id
            var processArchiveId = _processMethods.ProcessArchiveId_GetByGUID(_databaseInteraction, processQueueGuid);
            while(processArchiveId == 0)
            {
                processArchiveId = _processMethods.ProcessArchiveId_GetByGUID(_databaseInteraction, processQueueGuid);
            }

            //Loop until a response record is written into ProcessArchiveDetail
            var response = _processMethods.ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId(_databaseInteraction, processArchiveId, "Response").FirstOrDefault();

            while(response == null)
            {
                response = _processMethods.ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId(_databaseInteraction, processArchiveId, "Response").FirstOrDefault();
            }

            //Create return object with response record
            if(response == "OK")
            {
                return new OkObjectResult(new { message = "200 OK" });
            }

            return new UnauthorizedResult();
        }
    }
}
