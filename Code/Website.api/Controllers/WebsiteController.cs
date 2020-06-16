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
        private readonly CommonMethods.System _systemMethods = new CommonMethods.System();
        private static readonly CommonEnums.System.API.Name _systemAPINameEnums = new CommonEnums.System.API.Name();
        private static readonly CommonEnums.System.API.Password _systemAPIPasswordEnums = new CommonEnums.System.API.Password();
        private readonly CommonEnums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new CommonEnums.System.API.RequiredDataKey();
        private static readonly CommonEnums.System.API.GUID _systemAPIGUIDEnums = new CommonEnums.System.API.GUID();
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
            //TODO: Add try/catch

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Insert into ProcessQueue
            _systemMethods.ProcessQueue_Insert(_databaseInteraction, 
                queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _systemAPIGUIDEnums.WebsiteAPI);

            //Get Routing.API URL
            var routingAPIId = _systemMethods.GetRoutingAPIId(_databaseInteraction);

            //Connect to Routing API and POST data
            _systemMethods.CreateAPI(_databaseInteraction, routingAPIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, routingAPIId), 
                        _systemMethods.GetAPIData(_databaseInteraction, routingAPIId, jsonObject));

            //Update Process Queue
            _systemMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _systemAPIGUIDEnums.WebsiteAPI);
        }

        [HttpPost]
        [Route("website/GetResponse")]
        public IActionResult GetResponse([FromBody] string processQueueGuid)
        {
            //TODO: Add try/catch
            
            //Get Process Archive Id
            var processArchiveId = _systemMethods.ProcessArchiveId_GetByGUID(_databaseInteraction, processQueueGuid);
            while(processArchiveId == 0)
            {
                processArchiveId = _systemMethods.ProcessArchiveId_GetByGUID(_databaseInteraction, processQueueGuid);
            }

            //Loop until a response record is written into ProcessArchiveDetail
            var response = _systemMethods.ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId(_databaseInteraction, processArchiveId, _systemProcessArchiveAttributeEnums.Response).FirstOrDefault();

            while(response == null)
            {
                response = _systemMethods.ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId(_databaseInteraction, processArchiveId, _systemProcessArchiveAttributeEnums.Response).FirstOrDefault();
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
