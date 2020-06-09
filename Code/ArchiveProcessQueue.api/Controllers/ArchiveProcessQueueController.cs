using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;

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
            var APIList = jsonObject["APIList"].ToString();
            var prerequisiteAPIs = APIList.Replace("[", "").Replace("]", "").Split(',').Select(v => Convert.ToInt64(v)).ToList();
            
            //Wait until prerequisite APIs have completed
            var completedPrerequisiteAPIs = new List<long>();

            while(completedPrerequisiteAPIs.Count() < prerequisiteAPIs.Count())
            {
                foreach(var prerequisiteAPI in prerequisiteAPIs)
                {
                    if(completedPrerequisiteAPIs.Contains(prerequisiteAPI))
                    {
                        continue;
                    }

                    //Get prerequisite API EffectiveToDate from System.ProcessQueue
                    var processQueueDataRow = _processMethods.ProcessQueue_GetByGUIDAndAPIId(_databaseInteraction, queueGUID, prerequisiteAPI);

                    //If EffectiveToDate is '9999-12-31' then it is still processing
                    //otherwise, it has finished so add to completed if successful or errored if not
                    var effectiveToDate = Convert.ToDateTime(processQueueDataRow["EffectiveToDateTime"]);
                    if(effectiveToDate.Year != 9999)
                    {
                        completedPrerequisiteAPIs.Add(prerequisiteAPI);
                    }
                }
            }

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
