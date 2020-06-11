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
        private readonly CommonMethods.API _apiMethods = new CommonMethods.API();
        private readonly CommonMethods.Mapping _mappingMethods = new CommonMethods.Mapping();
        private readonly CommonMethods.UserDetail _userDetailMethods = new CommonMethods.UserDetail();
        private readonly CommonMethods.Password _passwordMethods = new CommonMethods.Password();
        private readonly CommonMethods.Process _processMethods = new CommonMethods.Process();
        private static readonly CommonEnums.System.API.Name _systemAPINameEnums = new CommonEnums.System.API.Name();
        private static readonly CommonEnums.System.API.Password _systemAPIPasswordEnums = new CommonEnums.System.API.Password();
        private readonly CommonEnums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new CommonEnums.System.API.RequiredDataKey();
        private static readonly CommonEnums.System.API.GUID _apiGUIDEnums = new CommonEnums.System.API.GUID();
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
            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Insert into ProcessQueue
            _processMethods.ProcessQueue_Insert(_databaseInteraction, 
                queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _apiGUIDEnums.StoreLoginAttemptAPI);

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _apiMethods.GetCheckPrerequisiteAPIAPIId(_databaseInteraction);

            //Build JObject
            var apiData = _apiMethods.GetAPIData(_databaseInteraction, checkPrerequisiteAPIAPIId, jsonObject);
            apiData.Add(_systemAPIRequiredDataKeyEnums.CallingGUID, _apiGUIDEnums.StoreLoginAttemptAPI);
            
            //Call CheckPrerequisiteAPI API
            var processTask = _apiMethods.CreateAPI(_databaseInteraction, checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _apiMethods.GetAPIPOSTRouteByAPIId(_databaseInteraction, checkPrerequisiteAPIAPIId), 
                        apiData);
            var processTaskResponse = processTask.GetAwaiter().GetResult();
            var result = processTaskResponse.Content.ReadAsStringAsync();
            var erroredPrerequisiteAPIs = result.Result.ToString()
                .Replace("\"","")
                .Replace("[","")
                .Replace("]","")
                .Split(',', StringSplitOptions.RemoveEmptyEntries); //TODO: Make this an extension

            try
            {
                //Get User Id
                var emailAddress = jsonObject[_systemAPIRequiredDataKeyEnums.EmailAddress].ToString();
                var userDetailId = _userDetailMethods.UserDetailId_GetByEmailAddress(_databaseInteraction, emailAddress);
                var userId = _userDetailMethods.UserId_GetByUserDetailId(_databaseInteraction, userDetailId);

                //TODO: Insert Login attempt

                //TODO: Get Login Id

                //TODO: Map Login attempt to user

                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _apiGUIDEnums.StoreLoginAttemptAPI);
            }
            catch(Exception error)
            {
                //TODO: Store SystemError
                Console.WriteLine(error.Message);

                //Update Process Queue
                _processMethods.ProcessQueue_Update(_databaseInteraction, queueGUID, _apiGUIDEnums.StoreLoginAttemptAPI, true);
            }
        }
    }
}
