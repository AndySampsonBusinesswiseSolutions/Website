using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace Website.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class WebsiteController : ControllerBase
    {
        #region Variables
        private readonly ILogger<WebsiteController> _logger;
        private readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        public readonly Methods.Information _informationMethods = new Methods.Information();
        public readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private readonly Enums.SystemSchema.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.SystemSchema.ProcessArchive.Attribute _systemProcessArchiveAttributeEnums = new Enums.SystemSchema.ProcessArchive.Attribute();
        private readonly Int64 websiteAPIId;
        private readonly string hostEnvironment;
        #endregion

        public WebsiteController(ILogger<WebsiteController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().WebsiteAPI, password);
            websiteAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.WebsiteAPI);
        }

        [HttpPost]
        [Route("Website/Validate")]
        public void Validate([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
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
                var routingAPIId = _systemAPIMethods.GetRoutingAPIId();

                //Connect to Routing API and POST data
                _systemAPIMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.WebsiteAPI, hostEnvironment, jsonObject);

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
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
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
            var websiteAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(APIGUID);

            //Get Process Archive Detail Id List by API Id
            var APIProcessArchiveDetailIdList = _mappingMethods.APIToProcessArchiveDetail_GetProcessArchiveDetailIdListByAPIId(websiteAPIId);

            //Get Process Archive Detail Id that is in both lists
            var processArchiveDetailId = processArchiveDetailIdList.Intersect(APIProcessArchiveDetailIdList).FirstOrDefault();

            //Get Process Archive Detail Description by Process Archive Detail Id
            var processArchiveDetailDescription = _systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailDescriptionByProcessArchiveDetailId(processArchiveDetailId);
            
            //Return Process Archive Detail Description in message
            return new OkObjectResult(new { message = processArchiveDetailDescription });
        }

        [HttpPost]
        [Route("Website/GetPageRequestResult")]
        public IActionResult GetPageRequestResult([FromBody] string processQueueGUID)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get User Email Address
            // var userEmailAddress = _systemMethods.GetEmailAddressFromJObject(jsonObject);

            //Get UserId from Email Address
            var userId = administrationUserMethods.GetSystemUserId();

            //Get HTML from Page Request by Process Queue GUID and User Id
            var pageRequestResult = _systemMethods.PageRequest_GetPageRequestResultByProcessQueueGUIDAndUserId(processQueueGUID, userId);
            
            //Return Process Archive Detail Description in message
            return new OkObjectResult(new { message = pageRequestResult });
        }
    }
}