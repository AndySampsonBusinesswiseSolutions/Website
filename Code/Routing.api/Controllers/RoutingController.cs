using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using commonMethods;
using enums;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;

namespace Routing.api.Controllers
{
    [ApiController]
    public class RoutingController : ControllerBase
    {
        private readonly ILogger<RoutingController> _logger;
        private readonly CommonMethods _methods = new CommonMethods();
        private readonly CommonMethods.System _systemMethods = new CommonMethods.System();
        private readonly CommonMethods.Mapping _mappingMethods = new CommonMethods.Mapping();
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
        [Route("Routing/POST")] //TODO:Change POST route to better name
        public void Route([FromBody] object data)
        {
            //TODO: Add try/catch

            //Get processId
            var jsonObject = JObject.Parse(data.ToString());
            
            //Get ValidateProcessGUID API Id
            var validateProcessAPIId = _systemMethods.GetValidateProcessGUIDAPIId();
            
            //Call ValidateProcessGUID API
            var processTask = _systemMethods.CreateAPI(validateProcessAPIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(validateProcessAPIId), 
                        _systemMethods.GetAPIData(validateProcessAPIId, jsonObject, _systemAPIGUIDEnums.RoutingAPI));
                        
            var result = processTask.GetAwaiter().GetResult().Content.ReadAsStringAsync();
            var processId = Convert.ToInt64(result.Result);

            //Get APIId list
            var APIIdList = _mappingMethods.APIToProcess_GetAPIIdListByProcessId(processId);
            var APIGUIDList = new List<string>();

            foreach(var APIId in APIIdList)
            {
                //Call API
                _systemMethods.CreateAPI(APIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(APIId), 
                        _systemMethods.GetAPIData(APIId, jsonObject, _systemAPIGUIDEnums.RoutingAPI));

                APIGUIDList.Add(_systemMethods.API_GetAPIGUIDByAPIId(APIId));
            }

            //Add Validate Process Id to list
            APIIdList.Add(validateProcessAPIId);
            APIGUIDList.Add(_systemMethods.API_GetAPIGUIDByAPIId(validateProcessAPIId));

            //Get Archive.API Id
            var archiveAPIId = _systemMethods.GetArchiveProcessQueueAPIId();

            //Create required jsonObject
            var archiveObject = _systemMethods.GetAPIData(archiveAPIId, jsonObject, _systemAPIGUIDEnums.RoutingAPI);
            archiveObject.Add(_systemAPIRequiredDataKeyEnums.APIGUIDList, JsonSerializer.Serialize(APIGUIDList));

            //Connect to Archive API and POST API list
            _systemMethods.CreateAPI(archiveAPIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(archiveAPIId), 
                        archiveObject);
        }
    }
}
