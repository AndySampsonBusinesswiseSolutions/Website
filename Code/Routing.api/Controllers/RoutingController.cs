using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using databaseInteraction;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Linq;

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
            var processGUID = jsonObject["ProcessGUID"].ToString();
            var processId = _processMethods.ProcessId_GetByGUID(_databaseInteraction, processGUID);

            //If processId == 0 then the GUID provided isn't valid so create an error
            if(processId == 0)
            {
                var queueGUID = jsonObject["QueueGUID"].ToString();

                //Insert into ProcessQueue
                _processMethods.ProcessQueue_Insert(_databaseInteraction, queueGUID, "743E21EE-2185-45D4-9003-E35060B751E2", "User Generated", "A4F25D07-86AA-42BD-ACD7-51A8F772A92B", true);
            }

            //Get APIId list
            List<long> APIIdList = _apiMethods.API_GetAPIIdListByProcessId(_databaseInteraction, processId);

            foreach(var APIId in APIIdList)
            {
                //Get API URL
                var APIURL = _apiMethods.GetAPIDetailByAPIId(_databaseInteraction, APIId, "HTTP Application URL").First();

                //Get data keys required for API
                var dataKeys = _apiMethods.GetAPIDetailByAPIId(_databaseInteraction, APIId, "Required Data Key");

                //Build new object with only those values API requires
                var apiData = new JObject();
                foreach(var dataKey in dataKeys)
                {
                    apiData.Add(dataKey, jsonObject[dataKey].ToString());
                }

                //Get API POST route
                var postRoute = _apiMethods.GetAPIDetailByAPIId(_databaseInteraction, APIId, "POST Route").First();

                //Call API
                _apiMethods.CreateAPI(APIURL).PostAsJsonAsync(postRoute, apiData);
            }

            //Get Archive.API URL
            var archiveAPIURL = _apiMethods.GetArchiveProcessQueueAPIURL(_databaseInteraction);

            //Get Archive.API POST Route
            var archiveAPIPOSTRoute = _apiMethods.GetArchiveProcessQueueAPIPOSTRoute(_databaseInteraction);

            //Connect to Archive API and POST API list
            _apiMethods.CreateAPI(archiveAPIURL).PostAsJsonAsync(archiveAPIPOSTRoute, APIIdList);
        }
    }
}
