using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MethodLibrary;
using enums;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Routing.api.Controllers
{
    [ApiController]
    public class RoutingController : ControllerBase
    {
        #region Variables
        private readonly ILogger<RoutingController> _logger;
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly string hostEnvironment;
        #endregion

        public RoutingController(ILogger<RoutingController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().RoutingAPI, password);
        }

        [HttpPost]
        [Route("Routing/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());
            var routingAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().RoutingAPI);
            var callingGUID = jsonObject[new Enums.SystemSchema.API.RequiredDataKey().CallingGUID].ToString();

            //Launch API process
            _systemAPIMethods.PostAsJsonAsyncAndDoNotAwaitResult(routingAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("Routing/POST")] //TODO:Change POST route to better name
        public void Route([FromBody] object data)
        {
            var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            try
            {
                //Get Queue GUID
                var jsonObject = JObject.Parse(data.ToString());
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
                var processId = 0L;
                
                //Get ValidateProcessGUID API Id
                var validateProcessGUIDAPIId = _systemAPIMethods.GetValidateProcessGUIDAPIId();
                
                try
                {
                    //Call ValidateProcessGUID API
                    var validateProcessGUIDAPIResult = _systemAPIMethods.PostAsJsonAsyncAndAwaitResult(validateProcessGUIDAPIId, systemAPIGUIDEnums.RoutingAPI, hostEnvironment, jsonObject);

                    //Get processId
                    processId = Convert.ToInt64(validateProcessGUIDAPIResult);
                }
                catch(Exception error)
                {
                    //API never started so create record
                    systemMethods.InsertProcessQueueError(processQueueGUID, createdByUserId, sourceId, validateProcessGUIDAPIId, error.Message);
                }
                
                //Get APIId list
                var APIIdList = new Methods.Mapping().APIToProcess_GetAPIIdListByProcessId(processId);
                var APIGUIDList = new List<string>
                    {
                        _systemAPIMethods.API_GetAPIGUIDByAPIId(validateProcessGUIDAPIId)
                    };

                foreach(var APIId in APIIdList)
                {
                    try
                    {
                        //If this doesn't fail then the API is running
                        var result = _systemAPIMethods.PostAsJsonAndAwaitResult(APIId, systemAPIGUIDEnums.RoutingAPI, hostEnvironment, jsonObject);
                    }
                    catch(Exception error)
                    {
                        //API never started so create record
                        systemMethods.InsertProcessQueueError(processQueueGUID, createdByUserId, sourceId, APIId, error.Message);
                    }
                    
                    APIGUIDList.Add(_systemAPIMethods.API_GetAPIGUIDByAPIId(APIId));
                }

                //Get ArchiveProcessQueueAPI Id
                var archiveProcessQueueAPIId = _systemAPIMethods.GetArchiveProcessQueueAPIId();

                //Create required jsonObject
                var archiveJsonObject = _systemAPIMethods.GetAPIData(archiveProcessQueueAPIId, systemAPIGUIDEnums.RoutingAPI, jsonObject);
                archiveJsonObject.Add(new Enums.SystemSchema.API.RequiredDataKey().APIGUIDList, JsonSerializer.Serialize(APIGUIDList));

                try
                {
                    //If this doesn't fail then the API is running
                    var result = _systemAPIMethods.PostAsJsonAndAwaitResult(archiveProcessQueueAPIId, systemAPIGUIDEnums.RoutingAPI, hostEnvironment, archiveJsonObject, false);
                }
                catch(Exception error)
                {
                    //API never started so create system error record
                    systemMethods.InsertSystemError(createdByUserId, sourceId, error);
                }
            }
            catch(Exception error)
            {
                systemMethods.InsertSystemError(createdByUserId, sourceId, error);
            }
        }
    }
}