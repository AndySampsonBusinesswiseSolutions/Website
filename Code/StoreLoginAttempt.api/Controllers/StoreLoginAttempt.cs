using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using databaseInteraction;
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
        private readonly CommonMethods.Mapping _mappingMethods = new CommonMethods.Mapping();
        private readonly CommonMethods.System _systemMethods = new CommonMethods.System();
        private readonly CommonMethods.Information _informationMethods = new CommonMethods.Information();
        private readonly CommonMethods.Administration _administrationMethods = new CommonMethods.Administration();
        private static readonly CommonEnums.System.API.Name _systemAPINameEnums = new CommonEnums.System.API.Name();
        private static readonly CommonEnums.System.API.Password _systemAPIPasswordEnums = new CommonEnums.System.API.Password();
        private readonly CommonEnums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new CommonEnums.System.API.RequiredDataKey();
        private static readonly CommonEnums.System.API.GUID _systemAPIGUIDEnums = new CommonEnums.System.API.GUID();
        private readonly CommonEnums.Administration.User.GUID _administrationUserGUIDEnums = new CommonEnums.Administration.User.GUID();
        private readonly CommonEnums.Information.SourceType _informationSourceTypeEnums = new CommonEnums.Information.SourceType();
        private readonly DatabaseInteraction _databaseInteraction = new DatabaseInteraction(_systemAPINameEnums.StoreLoginAttemptAPI, _systemAPIPasswordEnums.StoreLoginAttemptAPI);

        public StoreLoginAttemptController(ILogger<StoreLoginAttemptController> logger)
        {
            _logger = logger;
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
            _systemMethods.ProcessQueue_Insert(_databaseInteraction, 
                queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _systemAPIGUIDEnums.StoreLoginAttemptAPI);

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId(_databaseInteraction);

            //Build JObject
            var apiData = _systemMethods.GetAPIData(_databaseInteraction, checkPrerequisiteAPIAPIId, jsonObject);
            apiData.Add(_systemAPIRequiredDataKeyEnums.CallingGUID, _systemAPIGUIDEnums.StoreLoginAttemptAPI);
            
            //Call CheckPrerequisiteAPI API
            var processTask = _systemMethods.CreateAPI(_databaseInteraction, checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, checkPrerequisiteAPIAPIId), 
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
            var userDetailId = _administrationMethods.UserDetailId_GetByEmailAddress(_databaseInteraction, emailAddress);
            var userId = _administrationMethods.UserId_GetByUserDetailId(_databaseInteraction, userDetailId);

            //Get Source Type Id
            var sourceTypeId = _informationMethods.SourceTypeId_GetBySourceTypeDescription(_databaseInteraction, _informationSourceTypeEnums.UserGenerated);

            //Get Source Id
            var sourceId = _informationMethods.Source_GetBySourceTypeIdAndSourceTypeEntityId(_databaseInteraction, sourceTypeId, 0);

            //Store login attempt
            _administrationMethods.Login_Insert(_databaseInteraction, userId, sourceId, !erroredPrerequisiteAPIs.Any(), queueGUID);

            //Get Login Id
            var loginId = _administrationMethods.LoginId_GetByProcessArchiveGUID(_databaseInteraction, queueGUID);

            //Store mapping between login attempt and user
            _mappingMethods.LoginToUser_Insert(_databaseInteraction, 1, sourceId, loginId, userId);//TODO: Create GetSystemUserId method

            //Update Process Queue
            _systemMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _systemAPIGUIDEnums.StoreLoginAttemptAPI, erroredPrerequisiteAPIs.Any());
        }
    }
}
