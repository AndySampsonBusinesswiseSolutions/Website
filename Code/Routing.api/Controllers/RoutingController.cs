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
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();

        public RoutingController(ILogger<RoutingController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.RoutingAPI, _systemAPIPasswordEnums.RoutingAPI);
        }

        [HttpPost]
        [Route("Routing/POST")] //TODO:Chane POST route to better name
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
                        _systemMethods.GetAPIData(validateProcessAPIId, jsonObject));
            var processTaskResponse = processTask.GetAwaiter().GetResult();
            var result = processTaskResponse.Content.ReadAsStringAsync();//TODO: Make into common method
            var processId = Convert.ToInt64(result.Result);

            //Get APIId list
            var APIIdList = _systemMethods.GetAPIIdListByProcessId(processId);
            var APIGUIDList = new List<string>();

            foreach(var APIId in APIIdList)
            {
                //Call API
                _systemMethods.CreateAPI(APIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(APIId), 
                        _systemMethods.GetAPIData(APIId, jsonObject));

                APIGUIDList.Add(_systemMethods.GetAPIGUIDById(APIId));
            }

            //Add Validate Process Id to list
            APIIdList.Add(validateProcessAPIId);
            APIGUIDList.Add(_systemMethods.GetAPIGUIDById(validateProcessAPIId));

            //Get Archive.API Id
            var archiveAPIId = _systemMethods.GetArchiveProcessQueueAPIId();

            //Create required jsonObject
            var archiveObject = _systemMethods.GetAPIData(archiveAPIId, jsonObject);
            archiveObject.Add(_systemAPIRequiredDataKeyEnums.APIList, JsonSerializer.Serialize(APIGUIDList));

            //Connect to Archive API and POST API list
            _systemMethods.CreateAPI(archiveAPIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(archiveAPIId), 
                        archiveObject);
        }
    }
}
