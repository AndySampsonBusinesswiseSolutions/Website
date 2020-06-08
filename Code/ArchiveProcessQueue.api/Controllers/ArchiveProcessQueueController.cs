using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ArchiveProcessQueue.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ArchiveProcessQueueController : ControllerBase
    {
        private readonly ILogger<ArchiveProcessQueueController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("ArchiveProcessQueue.api", @"nb@89qWEW5!6=2s*");

        public ArchiveProcessQueueController(ILogger<ArchiveProcessQueueController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("ArchiveProcessQueue/Archive")]
        public void Archive([FromBody] object data)
        {
            //Get API List
            var jsonObject = JObject.Parse(data.ToString());
            
            //Loop through list

            //Check if API has finished

            //All APIs have finished so create record in ProcessArchive
            var queueGUID = jsonObject["QueueGUID"].ToString();
            _processMethods.ProcessArchive_Insert(_databaseInteraction, queueGUID, "743E21EE-2185-45D4-9003-E35060B751E2", "User Generated");
            var processArchiveId = _processMethods.ProcessArchiveId_GetByGUID(_databaseInteraction, queueGUID);

            //Write records for each API into ProcessArchiveDetail

            //Write response into ProcessArchiveDetail
            _processMethods.ProcessArchiveDetail_Insert(_databaseInteraction, queueGUID, "743E21EE-2185-45D4-9003-E35060B751E2", "User Generated", "Response", "OK");

            //Update ProcessArchive
            _processMethods.ProcessArchive_Update(_databaseInteraction, queueGUID);
        }
    }
}
