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
            _systemAPIMethods.PostAsJsonAsync(routingAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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
                
                //Get ValidateProcessGUID API Id
                var validateProcessGUIDAPIId = _systemAPIMethods.GetValidateProcessGUIDAPIId();
                
                //Call ValidateProcessGUID API
                var API = _systemAPIMethods.PostAsJsonAsync(validateProcessGUIDAPIId, systemAPIGUIDEnums.RoutingAPI, hostEnvironment, jsonObject);

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
                    //Call API
                    API = _systemAPIMethods.PostAsJson(APIId, systemAPIGUIDEnums.RoutingAPI, hostEnvironment, jsonObject);

                    try
                    {
                        //If this doesn't fail then the API is running
                        var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                    }
                    catch(Exception error)
                    {
                        //API never started so create record
                        systemMethods.InsertProcessQueueError(processQueueGUID, createdByUserId, sourceId, APIId, error.Message);
                    }
                    
                    APIGUIDList.Add(_systemAPIMethods.API_GetAPIGUIDByAPIId(APIId));
                }

                //Get Archive.API Id
                var archiveAPIId = _systemAPIMethods.GetArchiveProcessQueueAPIId();

                //Create required jsonObject
                var archiveObject = _systemAPIMethods.GetAPIData(archiveAPIId, systemAPIGUIDEnums.RoutingAPI, jsonObject);
                archiveObject.Add(new Enums.SystemSchema.API.RequiredDataKey().APIGUIDList, JsonSerializer.Serialize(APIGUIDList));

                //Connect to Archive API and POST API list
                API = _systemAPIMethods.PostAsJson(archiveAPIId, systemAPIGUIDEnums.RoutingAPI, hostEnvironment, archiveObject, false);

                try
                {
                    //If this doesn't fail then the API is running
                    var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
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
