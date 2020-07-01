using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;

namespace StoreUsageUpload.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreUsageUploadController : ControllerBase
    {
        private readonly ILogger<StoreUsageUploadController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.System.API.Attribute _systemAPIAttributeEnums = new Enums.System.API.Attribute();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Enums.Administration.User.Attribute _administrationUserAttributeEnums = new Enums.Administration.User.Attribute();
        private readonly Enums.Information.SourceAttribute _informationSourceAttributeEnums = new Enums.Information.SourceAttribute();

        public StoreUsageUploadController(ILogger<StoreUsageUploadController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreUsageUploadAPI, _systemAPIPasswordEnums.StoreUsageUploadAPI);
        }

        [HttpPost]
        [Route("StoreUsageUpload/Store")]
        public void Store([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();
            var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.StoreUsageUploadAPI);

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    queueGUID, 
                    createdByUserId,
                    sourceId,
                    APIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.StoreUsageUploadAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetAPIArray(result.Result.ToString());
                var errorMessage = erroredPrerequisiteAPIs.Any() ? $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored" : null;

                //TODO: Create Store logic

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(queueGUID, APIId, erroredPrerequisiteAPIs.Any(), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(queueGUID, APIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
