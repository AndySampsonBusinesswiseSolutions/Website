using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using commonMethods;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Linq;

namespace StoreLoginAttempt.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class StoreLoginAttemptController : ControllerBase
    {
        private readonly ILogger<StoreLoginAttemptController> _logger;
        private readonly CommonMethods _methods = new CommonMethods();
        private readonly CommonMethods.Mapping _mappingMethods = new CommonMethods.Mapping();
        private readonly CommonMethods.System _systemMethods = new CommonMethods.System();
        private readonly CommonMethods.Information _informationMethods = new CommonMethods.Information();
        private readonly CommonMethods.Administration _administrationMethods = new CommonMethods.Administration();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Enums.Information.SourceType _informationSourceTypeEnums = new Enums.Information.SourceType();

        public StoreLoginAttemptController(ILogger<StoreLoginAttemptController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.StoreLoginAttemptAPI, _systemAPIPasswordEnums.StoreLoginAttemptAPI);
        }

        [HttpPost]
        [Route("StoreLoginAttempt/Store")]
        public void Store([FromBody] object data)
        {
            //TODO: Add try/catch

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Insert into ProcessQueue
            _systemMethods.ProcessQueue_Insert(
                queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _systemAPIGUIDEnums.StoreLoginAttemptAPI);

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

            //Build JObject
            var apiData = _systemMethods.GetAPIData(checkPrerequisiteAPIAPIId, jsonObject);
            apiData.Add(_systemAPIRequiredDataKeyEnums.CallingGUID, _systemAPIGUIDEnums.StoreLoginAttemptAPI);
            
            //Call CheckPrerequisiteAPI API
            var processTask = _systemMethods.CreateAPI(checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(checkPrerequisiteAPIAPIId), 
                        apiData);
            var processTaskResponse = processTask.GetAwaiter().GetResult();
            var result = processTaskResponse.Content.ReadAsStringAsync();//TODO: Make into common method
            var erroredPrerequisiteAPIs = result.Result.ToString()
                .Replace("\"","")
                .Replace("[","")
                .Replace("]","")
                .Split(',', StringSplitOptions.RemoveEmptyEntries);//TODO: Make into extension

            //Get User Id
            var emailAddress = jsonObject[_systemAPIRequiredDataKeyEnums.EmailAddress].ToString();
            var userDetailId = _administrationMethods.UserDetailId_GetByEmailAddress(emailAddress);
            var userId = _administrationMethods.UserId_GetByUserDetailId(userDetailId);

            //Get Source Type Id
            var sourceTypeId = _informationMethods.SourceTypeId_GetBySourceTypeDescription(_informationSourceTypeEnums.UserGenerated);

            //Get Source Id
            var sourceId = _informationMethods.SourceId_GetBySourceTypeIdAndSourceTypeEntityId(sourceTypeId, 0);

            //Store login attempt
            _administrationMethods.Login_Insert(userId, sourceId, !erroredPrerequisiteAPIs.Any(), queueGUID);

            //Get Login Id
            var loginId = _administrationMethods.LoginId_GetByProcessArchiveGUID(queueGUID);

            //Store mapping between login attempt and user
            _mappingMethods.LoginToUser_Insert(1, sourceId, loginId, userId);//TODO: Create GetSystemUserId method

            //Update Process Queue
            _systemMethods.ProcessQueue_Update(queueGUID, _systemAPIGUIDEnums.StoreLoginAttemptAPI, erroredPrerequisiteAPIs.Any());
        }
    }
}
