using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CheckPrerequisiteAPI.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CheckPrerequisiteAPIController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CheckPrerequisiteAPIController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private readonly Enums.SystemSchema.API.Attribute _systemAPIAttributes = new Enums.SystemSchema.API.Attribute();
        private readonly string hostEnvironment;
        #endregion

        public CheckPrerequisiteAPIController(ILogger<CheckPrerequisiteAPIController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CheckPrerequisiteAPIAPI, password);
        }

        [HttpPost]
        [Route("CheckPrerequisiteAPI/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(_systemAPIMethods.GetCheckPrerequisiteAPIAPIId(), hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CheckPrerequisiteAPI/Check")]
        public List<string> Check([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            var prerequisiteAPIGUIDs = new List<string>();
            var completedPrerequisiteAPIGUIDs = new List<string>();
            var erroredPrerequisiteAPIGUIDs = new List<string>();
            var timeoutSecondsAttributeId = _systemAPIMethods.APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributes.TimeoutSeconds);

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
                    var prerequisiteAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(callingGUID);
                    var prerequisiteAPIGUIDAttributeId = _systemAPIMethods.APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributes.PrerequisiteAPIGUID);
                    prerequisiteAPIGUIDs = _systemAPIMethods.APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(prerequisiteAPIId, prerequisiteAPIGUIDAttributeId);
                }

                var prerequisiteAPIDictionary = prerequisiteAPIGUIDs
                    .ToDictionary(api => api, api => _systemAPIMethods.API_GetAPIIdByAPIGUID(api));
                var prerequisiteAPITimeoutDictionary = prerequisiteAPIGUIDs
                    .ToDictionary(api => api, api => Convert.ToDouble(_systemAPIMethods.APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(prerequisiteAPIDictionary[api], timeoutSecondsAttributeId).First()));

                //Wait until prerequisite APIs have completed
                while(prerequisiteAPIDictionary.Any())
                {
                    var remainingPrerequisiteAPIGUIDs = prerequisiteAPIGUIDs.Where(api=> prerequisiteAPIDictionary.ContainsKey(api)).ToList();
                    foreach(var prerequisiteAPIGUID in remainingPrerequisiteAPIGUIDs)
                    {
                        //Get prerequisite API EffectiveToDate from System.ProcessQueue
                        var prerequisiteAPIId = prerequisiteAPIDictionary[prerequisiteAPIGUID];
                        var processQueueDataRow = _systemMethods.ProcessQueue_GetByProcessQueueGUIDAndAPIId(processQueueGUID, prerequisiteAPIId);

                        if(processQueueDataRow != null)
                        {
                            //If EffectiveToDate is '9999-12-31' then it is still processing
                            //otherwise, it has finished so add to completed if successful or errored if not
                            var effectiveToDate = Convert.ToDateTime(processQueueDataRow["EffectiveToDateTime"]);
                            if(effectiveToDate.Year != 9999)
                            {
                                prerequisiteAPIDictionary.Remove(prerequisiteAPIGUID);

                                if(Convert.ToBoolean(processQueueDataRow["HasError"]))
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
                                var latestRunDate = effectiveFromDate.AddSeconds(prerequisiteAPITimeoutDictionary[prerequisiteAPIGUID]);

                                if(DateTime.UtcNow > latestRunDate)
                                {
                                    prerequisiteAPIDictionary.Remove(prerequisiteAPIGUID);
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

                    //wait 1 second
                    System.Threading.Thread.Sleep(1000);
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