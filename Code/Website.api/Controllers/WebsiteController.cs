using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using System.Net.Http;
using databaseInteraction;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Website.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        private readonly ILogger<WebsiteController> _logger;
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private static readonly CommonEnums.System.API.Name _systemAPINameEnums = new CommonEnums.System.API.Name();
        private static readonly CommonEnums.System.API.Password _systemAPIPasswordEnums = new CommonEnums.System.API.Password();
        private readonly CommonEnums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new CommonEnums.System.API.RequiredDataKey();
        private static readonly CommonEnums.System.API.GUID _apiGUIDEnums = new CommonEnums.System.API.GUID();
        private readonly CommonEnums.Administration.User.GUID _administrationUserGUIDEnums = new CommonEnums.Administration.User.GUID();
        private readonly CommonEnums.Information.SourceType _informationSourceTypeEnums = new CommonEnums.Information.SourceType();
        private readonly CommonEnums.System.ProcessArchive.Attribute _systemProcessArchiveAttributeEnums = new CommonEnums.System.ProcessArchive.Attribute();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction(_systemAPINameEnums.WebsiteAPI, _systemAPIPasswordEnums.WebsiteAPI);

        public WebsiteController(ILogger<WebsiteController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("Website/Validate")]
        public void Validate([FromBody] object data)
        {
            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Insert into ProcessQueue
            _processMethods.ProcessQueue_Insert(_databaseInteraction, 
                queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _apiGUIDEnums.WebsiteAPI);

            //Get Routing.API URL
            var routingAPIId = _apiMethods.GetRoutingAPIId(_databaseInteraction);

            //Connect to Routing API and POST data
            _apiMethods.CreateAPI(_databaseInteraction, routingAPIId)
                    .PostAsJsonAsync(
                        _apiMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, routingAPIId), 
                        _apiMethods.GetAPIData(_databaseInteraction, routingAPIId, jsonObject));

            //Update Process Queue
            _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _apiGUIDEnums.WebsiteAPI);
        }

        [HttpPost]
        [Route("website/GetResponse")]
        public IActionResult GetResponse([FromBody] string processQueueGuid)
        {
            //Get Process Archive Id
            var processArchiveId = _processMethods.ProcessArchiveId_GetByGUID(_databaseInteraction, processQueueGuid);
            while(processArchiveId == 0)
            {
                processArchiveId = _processMethods.ProcessArchiveId_GetByGUID(_databaseInteraction, processQueueGuid);
            }

            //Loop until a response record is written into ProcessArchiveDetail
            var response = _processMethods.ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId(_databaseInteraction, processArchiveId, _systemProcessArchiveAttributeEnums.Response).FirstOrDefault();

            while(response == null)
            {
                response = _processMethods.ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId(_databaseInteraction, processArchiveId, _systemProcessArchiveAttributeEnums.Response).FirstOrDefault();
            }

            //Create return object with response record
            if(response == "OK")
            {
                return new OkObjectResult(new { message = "200 OK" });
            }

            return new UnauthorizedResult();
        }
    }
}
