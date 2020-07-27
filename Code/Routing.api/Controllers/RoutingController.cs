using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MethodLibrary;
using enums;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;

namespace Routing.api.Controllers
{
    [ApiController]
    public class RoutingController : ControllerBase
    {
        private readonly ILogger<RoutingController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();

        public RoutingController(ILogger<RoutingController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.RoutingAPI, _systemAPIPasswordEnums.RoutingAPI);
        }

        [HttpPost]
        [Route("Routing/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());
            var routingAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.RoutingAPI);
            var callingGUID = jsonObject[_systemAPIRequiredDataKeyEnums.CallingGUID].ToString();

            //Launch API process
            _systemMethods.PostAsJsonAsync(routingAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("Routing/POST")] //TODO:Change POST route to better name
        public void Route([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            try
            {
                //Get Queue GUID
                var jsonObject = JObject.Parse(data.ToString());
                var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
                
                //Get ValidateProcessGUID API Id
                var validateProcessGUIDAPIId = _systemMethods.GetValidateProcessGUIDAPIId();
                
                //Call ValidateProcessGUID API
                var API = _systemMethods.PostAsJsonAsync(validateProcessGUIDAPIId, _systemAPIGUIDEnums.RoutingAPI, jsonObject);

                var processId = 0L;

                try
                {
                    var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                    //Get processId
                    processId = Convert.ToInt64(result.Result);
                }
                catch(Exception error)
                {
                    //API never started so create record
                    _systemMethods.InsertProcessQueueError(processQueueGUID, createdByUserId, sourceId, validateProcessGUIDAPIId, error.Message);
                }

                //Get APIId list
                var APIIdList = _mappingMethods.APIToProcess_GetAPIIdListByProcessId(processId);
                var APIGUIDList = new List<string>
                    {
                        _systemMethods.API_GetAPIGUIDByAPIId(validateProcessGUIDAPIId)
                    };

                foreach(var APIId in APIIdList)
                {
                    //Call API
                    API = _systemMethods.PostAsJson(APIId, _systemAPIGUIDEnums.RoutingAPI, jsonObject);

                    try
                    {
                        //If this doesn't fail then the API is running
                        var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                    }
                    catch(Exception error)
                    {
                        //API never started so create record
                        _systemMethods.InsertProcessQueueError(processQueueGUID, createdByUserId, sourceId, APIId, error.Message);
                    }
                    
                    APIGUIDList.Add(_systemMethods.API_GetAPIGUIDByAPIId(APIId));
                }

                //Get Archive.API Id
                var archiveAPIId = _systemMethods.GetArchiveProcessQueueAPIId();

                //Create required jsonObject
                var archiveObject = _systemMethods.GetAPIData(archiveAPIId, _systemAPIGUIDEnums.RoutingAPI, jsonObject);
                archiveObject.Add(_systemAPIRequiredDataKeyEnums.APIGUIDList, JsonSerializer.Serialize(APIGUIDList));

                //Connect to Archive API and POST API list
                API = _systemMethods.PostAsJson(archiveAPIId, _systemAPIGUIDEnums.RoutingAPI, archiveObject, false);

                try
                {
                    //If this doesn't fail then the API is running
                    var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                }
                catch(Exception error)
                {
                    //API never started so create system error record
                    _systemMethods.InsertSystemError(createdByUserId, sourceId, error);
                }
            }
            catch(Exception error)
            {
                _systemMethods.InsertSystemError(createdByUserId, sourceId, error);
            }
        }
    }
}
