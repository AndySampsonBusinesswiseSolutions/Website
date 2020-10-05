using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;

namespace Website.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        private readonly ILogger<WebsiteController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        public readonly Methods.Information _informationMethods = new Methods.Information();
        public readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.ProcessArchive.Attribute _systemProcessArchiveAttributeEnums = new Enums.System.ProcessArchive.Attribute();
        private readonly Int64 websiteAPIId;

        public WebsiteController(ILogger<WebsiteController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.WebsiteAPI, _systemAPIPasswordEnums.WebsiteAPI);
            websiteAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.WebsiteAPI);
        }

        [HttpPost]
        [Route("Website/Validate")]
        public void Validate([FromBody] object data)
        {
            var createdByUserId = _administrationMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            try
            {
                //Get Process Queue GUID
                var jsonObject = JObject.Parse(data.ToString());
                var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    websiteAPIId);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, websiteAPIId);

                //Get Routing.API URL
                var routingAPIId = _systemMethods.GetRoutingAPIId();

                //Connect to Routing API and POST data
                _systemMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.WebsiteAPI, jsonObject);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, websiteAPIId);
            }
            catch(Exception error)
            {
                _systemMethods.InsertSystemError(createdByUserId, sourceId, error);
            }        
        }

        [HttpPost]
        [Route("Website/GetProcessResponse")]
        public IActionResult GetProcessResponse([FromBody] string processQueueGUID)
        {
            var createdByUserId = _administrationMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            try
            {
                //Get Process Archive Id
                var processArchiveId = _systemMethods.ProcessArchive_GetProcessArchiveIdByProcessArchiveGUID(processQueueGUID);
                while(processArchiveId == 0)
                {
                    processArchiveId = _systemMethods.ProcessArchive_GetProcessArchiveIdByProcessArchiveGUID(processQueueGUID);
                }

                //Loop until a response record is written into ProcessArchiveDetail
                var responseAttributeId = _systemMethods.ProcessArchiveAttribute_GetProcessArchiveAttributeIdByProcessArchiveAttributeDescription(_systemProcessArchiveAttributeEnums.Response);
                var response = _systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailDescriptionListByProcessArchiveIDAndProcessArchiveAttributeId(processArchiveId, responseAttributeId).FirstOrDefault();

                while(response == null)
                {
                    response = _systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailDescriptionListByProcessArchiveIDAndProcessArchiveAttributeId(processArchiveId, responseAttributeId).FirstOrDefault();
                }

                //Create return object with response record
                switch(response)
                {
                    case "OK":
                        return new OkObjectResult(new { message = "OK" });
                    case "ERROR":
                        return new UnauthorizedResult(); //status = 401
                    case "SYSTEM ERROR":
                    default:
                        return new BadRequestResult(); //status = 400
                }
            }
            catch(Exception error)
            {
                _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                return new BadRequestResult();
            }
        }

        [HttpPost]
        [Route("Website/GetProcessResponseDetail")]
        public IActionResult GetProcessResponseDetail([FromBody] object data)
        {
            var jsonObject = JObject.Parse(data.ToString());

            //Get Process Queue GUID
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            //Get Process Archive Id
            var processArchiveId = _systemMethods.ProcessArchive_GetProcessArchiveIdByProcessArchiveGUID(processQueueGUID);

            //Get API Response Process Archive Attribute Id
            var APIResponseProcesArchiveAttributeId = _systemMethods.ProcessArchiveAttribute_GetProcessArchiveAttributeIdByProcessArchiveAttributeDescription(_systemProcessArchiveAttributeEnums.APIResponse);

            //Get Process Archive Detail Id List By Process Archive Id and Process Archive Attribute Id
            var processArchiveDetailIdList = _systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailIdListByProcessArchiveIDAndProcessArchiveAttributeId(processArchiveId, APIResponseProcesArchiveAttributeId);

            //Get API GUID
            var APIGUID = jsonObject[_systemAPIRequiredDataKeyEnums.APIGUID].ToString();

            //Get API Id
            var websiteAPIId = _systemMethods.API_GetAPIIdByAPIGUID(APIGUID);

            //Get Process Archive Detail Id List by API Id
            var APIProcessArchiveDetailIdList = _mappingMethods.APIToProcessArchiveDetail_GetProcessArchiveDetailIdListByAPIId(websiteAPIId);

            //Get Process Archive Detail Id that is in both lists
            var processArchiveDetailId = processArchiveDetailIdList.Intersect(APIProcessArchiveDetailIdList).FirstOrDefault();

            //Get Process Archive Detail Description by Process Archive Detail Id
            var processArchiveDetailDescription = _systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailDescriptionByProcessArchiveDetailId(processArchiveDetailId);
            
            //Return Process Archive Detail Description in message
            return new OkObjectResult(new { message = processArchiveDetailDescription });
        }
    }
}