using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

namespace ValidatePageGUID.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class ValidatePageGUIDController : ControllerBase
    {
        private readonly ILogger<ValidatePageGUIDController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Page _pageMethods = new CommonMethods.Page();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("ValidatePageGUID.api", @"n:Q>V&6P9KtG`(5k");

        public ValidatePageGUIDController(ILogger<ValidatePageGUIDController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("ValidatePageGUID/Validate")]
        public void Validate([FromBody] object data)
        {
            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject["QueueGUID"].ToString();

            //Insert into ProcessQueue
            _processMethods.ProcessQueue_Insert(_databaseInteraction, queueGUID, "743E21EE-2185-45D4-9003-E35060B751E2", "User Generated", "F916F19F-9408-4969-84DC-9905D2FEFB0B");

            //Get prerequisite APIs from database
            var prerequisiteAPIs = _apiMethods.GetAPIDetailByAPIGUID(_databaseInteraction, "F916F19F-9408-4969-84DC-9905D2FEFB0B", "Prerequisite API GUID");

            //Wait until prerequisite APIs have completed
            var completedPrerequisiteAPIs = new List<string>();
            var erroredPrerequisiteAPIs = new List<string>();

            while((completedPrerequisiteAPIs.Count() + erroredPrerequisiteAPIs.Count()) < prerequisiteAPIs.Count())
            {
                foreach(var prerequisiteAPI in prerequisiteAPIs)
                {
                    if(completedPrerequisiteAPIs.Contains(prerequisiteAPI) || erroredPrerequisiteAPIs.Contains(prerequisiteAPI))
                    {
                        continue;
                    }

                    //Get prerequisite API EffectiveToDate from System.ProcessQueue
                    var apiId = _apiMethods.GetAPIIdByGUID(_databaseInteraction, prerequisiteAPI);
                    var processQueueDataRow = _processMethods.ProcessQueue_GetByGUIDAndAPIId(_databaseInteraction, queueGUID, apiId);

                    //If EffectiveToDate is '9999-12-31' then it is still processing
                    //otherwise, it has finished so add to completed if successful or errored if not
                    var effectiveToDate = Convert.ToDateTime(processQueueDataRow["EffectiveToDateTime"]);
                    if(effectiveToDate.Year != 9999)
                    {
                        var hasError = Convert.ToBoolean(processQueueDataRow["HasError"]);
                        if(hasError)
                        {
                            erroredPrerequisiteAPIs.Add(prerequisiteAPI);
                        }
                        else
                        {
                            completedPrerequisiteAPIs.Add(prerequisiteAPI);
                        }
                    }
                }
            }

            if(!erroredPrerequisiteAPIs.Any())
            {
                //Get Page GUID
                var pageGUID = jsonObject["PageGUID"].ToString();

                //Validate Page GUID
                var pageId = _pageMethods.PageId_GetByGUID(_databaseInteraction, pageGUID);

                //If pageId == 0 then the GUID provided isn't valid so create an error
                if(pageId == 0)
                {
                    
                }

                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, "F916F19F-9408-4969-84DC-9905D2FEFB0B", pageId == 0);
            }
            else
            {
                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, "F916F19F-9408-4969-84DC-9905D2FEFB0B", true);
            }
        }
    }
}
