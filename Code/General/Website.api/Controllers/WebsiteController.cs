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
        #region Variables
        private readonly ILogger<WebsiteController> _logger;
        private readonly Entity.System.API.Website.Configuration _configuration;
        #endregion

        public WebsiteController(ILogger<WebsiteController> logger, Entity.System.API.Website.Configuration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Website/Validate")]
        public void Validate([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            try
            {
                //Get Process Queue GUID
                var jsonObject = JObject.Parse(data.ToString());
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    _configuration.APIId);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, _configuration.APIId);

                //Connect to Routing API and POST data
                new Methods.System.API().PostAsJsonAsync(_configuration.RoutingAPIId, _configuration.APIGUID, _configuration.HostEnvironment, jsonObject);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, _configuration.APIId);
            }
            catch(Exception error)
            {
                systemMethods.InsertSystemError(createdByUserId, sourceId, error);
            }        
        }

        [HttpPost]
        [Route("Website/GetProcessResponse")]
        public IActionResult GetProcessResponse([FromBody] string processQueueGUID)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            try
            {
                var response = systemMethods.GetProcessResponse(processQueueGUID);

                //Create return object with response record
                switch (response)
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
            catch (Exception error)
            {
                systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                return new BadRequestResult();
            }
        }

        [HttpPost]
        [Route("Website/GetProcessResponseDetail")]
        public IActionResult GetProcessResponseDetail([FromBody] object data)
        {
            var systemMethods = new Methods.System();
            var jsonObject = JObject.Parse(data.ToString());

            //Get Process Queue GUID
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            //Get Process Archive Id
            var processArchiveId = systemMethods.ProcessArchive_GetProcessArchiveIdByProcessArchiveGUID(processQueueGUID);

            //Get API Response Process Archive Attribute Id
            var APIResponseProcessArchiveAttributeId = systemMethods.ProcessArchiveAttribute_GetProcessArchiveAttributeIdByProcessArchiveAttributeDescription(new Enums.SystemSchema.ProcessArchive.Attribute().APIResponse);

            //Get Process Archive Detail Id List By Process Archive Id and Process Archive Attribute Id
            var processArchiveDetailIdList = systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailIdListByProcessArchiveIDAndProcessArchiveAttributeId(processArchiveId, APIResponseProcessArchiveAttributeId);

            //Get API GUID
            var APIGUID = jsonObject[new Enums.SystemSchema.API.RequiredDataKey().APIGUID].ToString();

            //Get API Id
            var APIId = new Methods.System.API().API_GetAPIIdByAPIGUID(APIGUID);

            //Get Process Archive Detail Id List by API Id
            var APIProcessArchiveDetailIdList = new Methods.Mapping().APIToProcessArchiveDetail_GetProcessArchiveDetailIdListByAPIId(APIId);

            //Get Process Archive Detail Id that is in both lists
            var processArchiveDetailId = processArchiveDetailIdList.Intersect(APIProcessArchiveDetailIdList).FirstOrDefault();

            //Get Process Archive Detail Description by Process Archive Detail Id
            var processArchiveDetailDescription = systemMethods.ProcessArchiveDetail_GetProcessArchiveDetailDescriptionByProcessArchiveDetailId(processArchiveDetailId);
            
            //Return Process Archive Detail Description in message
            return new OkObjectResult(new { message = processArchiveDetailDescription });
        }

        [HttpPost]
        [Route("Website/GetPageRequestResult")]
        public IActionResult GetPageRequestResult([FromBody] string processQueueGUID)
        {

            //Get User Email Address
            // var userEmailAddress = _systemMethods.GetEmailAddressFromJObject(jsonObject);

            //Get UserId from Email Address
            var userId = new Methods.Administration.User().GetSystemUserId();

            //Get HTML from Page Request by Process Queue GUID and User Id
            var pageRequestResult = new Methods.System().PageRequest_GetPageRequestResultByProcessQueueGUIDAndUserId(processQueueGUID, userId);
            
            //Return Process Archive Detail Description in message
            return new OkObjectResult(new { message = pageRequestResult });
        }
    }
}