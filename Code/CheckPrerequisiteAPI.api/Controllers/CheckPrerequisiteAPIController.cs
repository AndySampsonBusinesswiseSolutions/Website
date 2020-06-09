﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

namespace CheckPrerequisiteAPI.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CheckPrerequisiteAPIController : ControllerBase
    {
        private readonly ILogger<CheckPrerequisiteAPIController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Page _pageMethods = new CommonMethods.Page();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("CheckPrerequisiteAPI.api", @"w8chCkRAW]\N[7Hh");

        public CheckPrerequisiteAPIController(ILogger<CheckPrerequisiteAPIController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("CheckPrerequisiteAPI/Check")]
        public List<string> Check([FromBody] object data)
        {
            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject["QueueGUID"].ToString();
            var prerequisiteAPIs = new List<string>();

            //If API list is passed through, then use that otherwise get API list from database
            if(jsonObject.ContainsKey("APIList"))
            {
                var APIList = jsonObject["APIList"].ToString();
                prerequisiteAPIs = APIList.Replace("\"","").Replace("[", "").Replace("]", "").Split(',').ToList();
            }
            else
            {
                //Get prerequisite APIs from database
                var callingGUID = jsonObject["CallingGUID"].ToString();
                prerequisiteAPIs = _apiMethods.GetAPIDetailByAPIGUID(_databaseInteraction, callingGUID, "Prerequisite API GUID");
            }

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

                    if(processQueueDataRow != null)
                    {
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
            }

            return erroredPrerequisiteAPIs;
        }
    }
}
