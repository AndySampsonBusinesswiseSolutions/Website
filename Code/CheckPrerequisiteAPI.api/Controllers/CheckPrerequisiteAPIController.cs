using Microsoft.AspNetCore.Mvc;
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
        private readonly CommonMethods.System _systemMethods = new CommonMethods.System();
        private static readonly CommonEnums.System.API.Name _systemAPINameEnums = new CommonEnums.System.API.Name();
        private static readonly CommonEnums.System.API.Password _systemAPIPasswordEnums = new CommonEnums.System.API.Password();
        private readonly CommonEnums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new CommonEnums.System.API.RequiredDataKey();
        private readonly CommonEnums.System.API.Attribute _systemAPIAttributes = new CommonEnums.System.API.Attribute();
        private readonly CommonEnums.System.ProcessArchive.Attribute _systemProcessArchiveAttributeEnums = new CommonEnums.System.ProcessArchive.Attribute();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction(_systemAPINameEnums.CheckPrerequisiteAPIAPI, _systemAPIPasswordEnums.CheckPrerequisiteAPIAPI);

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
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();
            var prerequisiteAPIs = new List<string>();

            //If API list is passed through, then use that otherwise get API list from database
            if(jsonObject.ContainsKey(_systemAPIRequiredDataKeyEnums.APIList))
            {
                var APIList = jsonObject[_systemAPIRequiredDataKeyEnums.APIList].ToString();
                prerequisiteAPIs = APIList.Replace("\"","").Replace("[", "").Replace("]", "").Split(',').ToList();
            }
            else
            {
                //Get prerequisite APIs from database
                var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();
                prerequisiteAPIs = _systemMethods.GetAPIDetailByAPIGUID(_databaseInteraction, callingGUID, _systemAPIAttributes.PrerequisiteAPIGUID);
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
                    var apiId = _systemMethods.GetAPIIdByGUID(_databaseInteraction, prerequisiteAPI);
                    var processQueueDataRow = _systemMethods.ProcessQueue_GetByGUIDAndAPIId(_databaseInteraction, queueGUID, apiId);

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
