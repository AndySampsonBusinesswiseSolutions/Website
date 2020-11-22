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
            new Methods.SystemSchema.API().PostAsJsonAsync(new Methods.SystemSchema.API().GetCheckPrerequisiteAPIAPIId(), hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CheckPrerequisiteAPI/Check")]
        public List<string> Check([FromBody] object data)
        {
            var systemAPIAttributes = new Enums.SystemSchema.API.Attribute();
            var systemAPIMethods = new Methods.SystemSchema.API();
            var systemMethods = new Methods.SystemSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            var prerequisiteAPIGUIDs = new List<string>();
            var completedPrerequisiteAPIGUIDs = new List<string>();
            var erroredPrerequisiteAPIGUIDs = new List<string>();

            try
            {
                var timeoutSecondsAttributeId = systemAPIMethods.APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(systemAPIAttributes.TimeoutSeconds);

                //If API list is passed through, then use that otherwise get API list from database
                var APIGUIDList = systemMethods.GetAPIGUIDListFromJObject(jsonObject);
                if(!string.IsNullOrWhiteSpace(APIGUIDList))
                {
                    prerequisiteAPIGUIDs = APIGUIDList.Replace("\"","").Replace("[", "").Replace("]", "").Split(',').ToList();
                }
                else
                {
                    //Get prerequisite APIs from database
                    var callingGUID = systemMethods.GetCallingGUIDFromJObject(jsonObject);
                    var prerequisiteAPIId = systemAPIMethods.API_GetAPIIdByAPIGUID(callingGUID);
                    var prerequisiteAPIGUIDAttributeId = systemAPIMethods.APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(systemAPIAttributes.PrerequisiteAPIGUID);
                    prerequisiteAPIGUIDs = systemAPIMethods.APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(prerequisiteAPIId, prerequisiteAPIGUIDAttributeId);
                }

                var prerequisiteAPIDictionary = prerequisiteAPIGUIDs
                    .ToDictionary(api => api, api => systemAPIMethods.API_GetAPIIdByAPIGUID(api));
                var prerequisiteAPITimeoutDictionary = prerequisiteAPIGUIDs
                    .ToDictionary(api => api, api => Convert.ToDouble(systemAPIMethods.APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(prerequisiteAPIDictionary[api], timeoutSecondsAttributeId).First()));

                //Wait until prerequisite APIs have completed
                while(prerequisiteAPIDictionary.Any())
                {
                    var remainingPrerequisiteAPIGUIDs = prerequisiteAPIGUIDs.Where(api=> prerequisiteAPIDictionary.ContainsKey(api)).ToList();
                    foreach(var prerequisiteAPIGUID in remainingPrerequisiteAPIGUIDs)
                    {
                        //Get prerequisite API EffectiveToDate from System.ProcessQueue
                        var prerequisiteAPIId = prerequisiteAPIDictionary[prerequisiteAPIGUID];
                        var processQueueDataRow = systemMethods.ProcessQueue_GetByProcessQueueGUIDAndAPIId(processQueueGUID, prerequisiteAPIId);

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

                                    var errorId = systemMethods.InsertSystemError(createdByUserId, 
                                        sourceId, 
                                        $"API {prerequisiteAPIId} Timeout",
                                        "API Timeout",
                                        Environment.StackTrace);

                                    //Update Process Queue
                                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, prerequisiteAPIId, true, $"System Error Id {errorId}");
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
                systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                return prerequisiteAPIGUIDs;
            }

            return erroredPrerequisiteAPIGUIDs;
        }
    }
}