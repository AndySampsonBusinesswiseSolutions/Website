using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using MethodLibrary;
using enums;
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
        private readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.Attribute _systemAPIAttributes = new Enums.System.API.Attribute();

        public CheckPrerequisiteAPIController(ILogger<CheckPrerequisiteAPIController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CheckPrerequisiteAPIAPI, _systemAPIPasswordEnums.CheckPrerequisiteAPIAPI);
        }

        [HttpPost]
        [Route("CheckPrerequisiteAPI/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(_systemMethods.GetCheckPrerequisiteAPIAPIId(), JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CheckPrerequisiteAPI/Check")]
        public List<string> Check([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            var prerequisiteAPIGUIDs = new List<string>();
            var completedPrerequisiteAPIGUIDs = new List<string>();
            var erroredPrerequisiteAPIGUIDs = new List<string>();
            var timeoutSecondsAttributeId = _systemMethods.APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributes.TimeoutSeconds);

            try
            {
                //If API list is passed through, then use that otherwise get API list from database
                var APIGUIDList = _systemMethods.GetAPIGUIDListFromJObject(jsonObject);
                if(!string.IsNullOrWhiteSpace(APIGUIDList))
                {
                    prerequisiteAPIGUIDs = APIGUIDList.Replace("\"","").Replace("[", "").Replace("]", "").Split(',').ToList();
                }
                else
                {
                    //Get prerequisite APIs from database
                    var callingGUID = _systemMethods.GetCallingGUIDFromJObject(jsonObject);
                    var prerequisiteAPIId = _systemMethods.API_GetAPIIdByAPIGUID(callingGUID);
                    var prerequisiteAPIGUIDAttributeId = _systemMethods.APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributes.PrerequisiteAPIGUID);
                    prerequisiteAPIGUIDs = _systemMethods.APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(prerequisiteAPIId, prerequisiteAPIGUIDAttributeId);
                }

                //Wait until prerequisite APIs have completed
                while((completedPrerequisiteAPIGUIDs.Count() + erroredPrerequisiteAPIGUIDs.Count()) < prerequisiteAPIGUIDs.Count())
                {
                    foreach(var prerequisiteAPIGUID in prerequisiteAPIGUIDs)
                    {
                        if(completedPrerequisiteAPIGUIDs.Contains(prerequisiteAPIGUID) || erroredPrerequisiteAPIGUIDs.Contains(prerequisiteAPIGUID))
                        {
                            continue;
                        }

                        //Get prerequisite API EffectiveToDate from System.ProcessQueue
                        var prerequisiteAPIId = _systemMethods.API_GetAPIIdByAPIGUID(prerequisiteAPIGUID);
                        var processQueueDataRow = _systemMethods.ProcessQueue_GetByProcessQueueGUIDAndAPIId(processQueueGUID, prerequisiteAPIId);

                        if(processQueueDataRow != null)
                        {
                            processQueueDataRow = _systemMethods.ProcessQueue_GetByProcessQueueGUIDAndAPIId(processQueueGUID, prerequisiteAPIId);
                            
                            //If EffectiveToDate is '9999-12-31' then it is still processing
                            //otherwise, it has finished so add to completed if successful or errored if not
                            var effectiveToDate = Convert.ToDateTime(processQueueDataRow["EffectiveToDateTime"]);
                            if(effectiveToDate.Year != 9999)
                            {
                                var hasError = Convert.ToBoolean(processQueueDataRow["HasError"]);
                                if(hasError)
                                {
                                    erroredPrerequisiteAPIGUIDs.Add(prerequisiteAPIGUID);
                                }
                                else
                                {
                                    completedPrerequisiteAPIGUIDs.Add(prerequisiteAPIGUID);
                                }
                            }
                            else
                            {
                                //Check if process has been running for longer than it's anticipated run time
                                var effectiveFromDate = Convert.ToDateTime(processQueueDataRow["EffectiveFromDateTime"]);
                                var timeoutSeconds = Convert.ToDouble(_systemMethods.APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(prerequisiteAPIId, timeoutSecondsAttributeId).First());
                                var latestRunDate = effectiveFromDate.AddMinutes(timeoutSeconds/60);
                                var currentDate = DateTime.UtcNow;

                                if(currentDate > latestRunDate)
                                {
                                    erroredPrerequisiteAPIGUIDs.Add(prerequisiteAPIGUID);

                                    var errorId = _systemMethods.InsertSystemError(createdByUserId, 
                                        sourceId, 
                                        $"API {prerequisiteAPIId} Timeout",
                                        "API Timeout",
                                        Environment.StackTrace);

                                    //Update Process Queue
                                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, prerequisiteAPIId, true, $"System Error Id {errorId}");
                                }
                            }
                        }                    
                    }
                }
            }
            catch(Exception error)
            {
                _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                return prerequisiteAPIGUIDs;
            }

            return erroredPrerequisiteAPIGUIDs;
        }
    }
}
