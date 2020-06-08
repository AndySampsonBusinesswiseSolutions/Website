using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using databaseInteraction;
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
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction("Routing.api", @"E{*Jj5&nLfC}@Q$:");

        public RoutingController(ILogger<RoutingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("Routing/POST")]
        public void Route([FromBody] object data)
        {
            //Get processId
            var jsonObject = JObject.Parse(data.ToString());
            
            //Get ValidateProcessGUID API Id
            var validateProcessAPIId = _apiMethods.GetValidateProcessGUIDAPIId(_databaseInteraction);
            
            //Call ValidateProcessGUID API
            var processTask = _apiMethods.CreateAPI(_databaseInteraction, validateProcessAPIId)
                    .PostAsJsonAsync(
                        _apiMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, validateProcessAPIId), 
                        _apiMethods.GetAPIData(_databaseInteraction, validateProcessAPIId, jsonObject));
            var processTaskResponse = processTask.GetAwaiter().GetResult();
            var result = processTaskResponse.Content.ReadAsStringAsync();
            var processId = Convert.ToInt64(result.Result);

            //Get APIId list
            List<long> APIIdList = _apiMethods.GetAPIIdListByProcessId(_databaseInteraction, processId);

            foreach(var APIId in APIIdList)
            {
                //Call API
                _apiMethods.CreateAPI(_databaseInteraction, APIId)
                    .PostAsJsonAsync(
                        _apiMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, APIId), 
                        _apiMethods.GetAPIData(_databaseInteraction, APIId, jsonObject));
            }

            //Add Validate Process Id to list
            APIIdList.Add(validateProcessAPIId);

            //Get Archive.API Id
            var archiveAPIId = _apiMethods.GetArchiveProcessQueueAPIId(_databaseInteraction);

            //Create required jsonObject
            var archiveObject = _apiMethods.GetAPIData(_databaseInteraction, archiveAPIId, jsonObject);
            archiveObject.Add("APIList", JsonSerializer.Serialize(APIIdList));

            //Connect to Archive API and POST API list
            _apiMethods.CreateAPI(_databaseInteraction, archiveAPIId)
                    .PostAsJsonAsync(
                        _apiMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, archiveAPIId), 
                        archiveObject);
        }
    }
}
