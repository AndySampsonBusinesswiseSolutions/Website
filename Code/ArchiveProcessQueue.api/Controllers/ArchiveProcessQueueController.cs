using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net.Http;

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
            var queueGUID = jsonObject["QueueGUID"].ToString();

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _apiMethods.GetCheckPrerequisiteAPIAPIId(_databaseInteraction);

            //Call CheckPrerequisiteAPI API
            var processTask = _apiMethods.CreateAPI(_databaseInteraction, checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _apiMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, checkPrerequisiteAPIAPIId), 
                        _apiMethods.GetAPIData(_databaseInteraction, checkPrerequisiteAPIAPIId, jsonObject));
            var processTaskResponse = processTask.GetAwaiter().GetResult();
            var result = processTaskResponse.Content.ReadAsStringAsync();

            //All APIs have finished so create record in ProcessArchive
            _processMethods.ProcessArchive_Insert(_databaseInteraction, queueGUID, "743E21EE-2185-45D4-9003-E35060B751E2", "User Generated");
            var processArchiveId = _processMethods.ProcessArchiveId_GetByGUID(_databaseInteraction, queueGUID);

            //Write records for each API into ProcessArchiveDetail

            //Write response into ProcessArchiveDetail
            _processMethods.ProcessArchiveDetail_Insert(_databaseInteraction, queueGUID, "743E21EE-2185-45D4-9003-E35060B751E2", "User Generated", "Response", "OK");

            //Update ProcessArchive
            _processMethods.ProcessArchive_Update(_databaseInteraction, queueGUID);

            //Delete GUID from ProcessQueue
        }
    }
}
