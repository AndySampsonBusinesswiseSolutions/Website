using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using System.Net.Http;

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
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.System.API.Attribute _systemAPIAttributes = new Enums.System.API.Attribute();
        private readonly Enums.System.ProcessArchive.Attribute _systemProcessArchiveAttributeEnums = new Enums.System.ProcessArchive.Attribute();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();

        public CheckPrerequisiteAPIController(ILogger<CheckPrerequisiteAPIController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CheckPrerequisiteAPIAPI, _systemAPIPasswordEnums.CheckPrerequisiteAPIAPI);
        }

        [HttpPost]
        [Route("CheckPrerequisiteAPI/Check")]
        public List<string> Check([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            var prerequisiteAPIGUIDs = new List<string>();
            var completedPrerequisiteAPIGUIDs = new List<string>();
            var erroredPrerequisiteAPIGUIDs = new List<string>();

            try
            {
                //If API list is passed through, then use that otherwise get API list from database
                if(jsonObject.ContainsKey(_systemAPIRequiredDataKeyEnums.APIGUIDList))
                {
                    var APIGUIDList = jsonObject[_systemAPIRequiredDataKeyEnums.APIGUIDList].ToString();
                    prerequisiteAPIGUIDs = APIGUIDList.Replace("\"","").Replace("[", "").Replace("]", "").Split(',').ToList();
                }
                else
                {
                    //Get prerequisite APIs from database
                    var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();
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
                        var APIId = _systemMethods.API_GetAPIIdByAPIGUID(prerequisiteAPIGUID);
                        var processQueueDataRow = _systemMethods.ProcessQueue_GetByProcessQueueGUIDAndAPIId(queueGUID, APIId);

                        if(processQueueDataRow == null)
                        {
                            var errorId = _systemMethods.InsertSystemError(createdByUserId, 
                                sourceId, 
                                $"API {APIId} Not Started",
                                "API Not Started",
                                Environment.StackTrace);

                            //API never started so create record
                            _systemMethods.ProcessQueue_Insert(
                                queueGUID, 
                                createdByUserId,
                                sourceId,
                                APIId);

                            _systemMethods.ProcessQueue_Update(queueGUID, APIId, true, $"System Error Id {errorId}");
                        }
                        else
                        {
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
                                var latestRunDate = effectiveFromDate.AddMinutes(1); //TODO: Make detail against API and make adjustable
                                var currentDate = DateTime.UtcNow;

                                if(currentDate > latestRunDate)
                                {
                                    erroredPrerequisiteAPIGUIDs.Add(prerequisiteAPIGUID);

                                    var errorId = _systemMethods.InsertSystemError(createdByUserId, 
                                        sourceId, 
                                        $"API {APIId} Timeout",
                                        "API Timeout",
                                        Environment.StackTrace);

                                    //Update Process Queue
                                    _systemMethods.ProcessQueue_Update(queueGUID, APIId, true, $"System Error Id {errorId}");
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
