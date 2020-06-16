using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using commonMethods;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Linq;

namespace LockUser.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class LockUserController : ControllerBase
    {
        private readonly ILogger<LockUserController> _logger;
        private readonly CommonMethods _methods = new CommonMethods();
        private readonly CommonMethods.Mapping _mappingMethods = new CommonMethods.Mapping();
        private readonly CommonMethods.System _systemMethods = new CommonMethods.System();
        private readonly CommonMethods.Administration _administrationMethods = new CommonMethods.Administration();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.System.API.Attribute _systemAPIAttributeEnums = new Enums.System.API.Attribute();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Administration.User.GUID _administrationUserGUIDEnums = new Enums.Administration.User.GUID();
        private readonly Enums.Information.SourceType _informationSourceTypeEnums = new Enums.Information.SourceType();

        public LockUserController(ILogger<LockUserController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.LockUserAPI, _systemAPIPasswordEnums.LockUserAPI);
        }

        [HttpPost]
        [Route("LockUser/Lock")]
        public void Lock([FromBody] object data)
        {
            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var queueGUID = jsonObject[_systemAPIRequiredDataKeyEnums.QueueGUID].ToString();

            //Insert into ProcessQueue
            _systemMethods.ProcessQueue_Insert(
                queueGUID, 
                _administrationUserGUIDEnums.System, 
                _informationSourceTypeEnums.UserGenerated, 
                _systemAPIGUIDEnums.LockUserAPI);

            //Get CheckPrerequisiteAPI API Id
            var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

            //Build JObject
            var apiData = _systemMethods.GetAPIData(checkPrerequisiteAPIAPIId, jsonObject);
            apiData.Add(_systemAPIRequiredDataKeyEnums.CallingGUID, _systemAPIGUIDEnums.LockUserAPI);
            
            //Call CheckPrerequisiteAPI API
            var processTask = _systemMethods.CreateAPI(checkPrerequisiteAPIAPIId)
                    .PostAsJsonAsync(
                        _systemMethods.GetAPIPOSTRouteByAPIId(checkPrerequisiteAPIAPIId), 
                        apiData);
            var processTaskResponse = processTask.GetAwaiter().GetResult();
            var result = processTaskResponse.Content.ReadAsStringAsync(); //TODO: Make into common method
            var erroredPrerequisiteAPIs = result.Result.ToString()
                .Replace("\"","")
                .Replace("[","")
                .Replace("]","")
                .Split(',', StringSplitOptions.RemoveEmptyEntries); //TODO: Make into extension

            if(erroredPrerequisiteAPIs.Any()) //TODO: Add try/catch for system error
            {
                //Get User Id
                var emailAddress = jsonObject[_systemAPIRequiredDataKeyEnums.EmailAddress].ToString();
                var userDetailId = _administrationMethods.UserDetailId_GetByEmailAddress(emailAddress);
                var userId = _administrationMethods.UserId_GetByUserDetailId(userDetailId);

                //Get logins by user id and order by descending
                var loginList = _mappingMethods.Login_GetByUserId(userId).OrderByDescending(l => l);

                //Initialise invalidAttempts variable
                var invalidAttempts = 0;

                //Loop through each login
                foreach(var login in loginList)
                {
                    //Get LoginSuccessful attribute
                    var loginSucessful = _administrationMethods.LoginSuccessful_GetByLoginId(login);

                    //If login is successful, then exit loop
                    //Else increment invalidAttempts
                    if(loginSucessful)
                    {
                        break;
                    }
                    else
                    {
                        invalidAttempts++;
                    }
                }

                //Get maximum attempts allowed before locking
                var maximumInvalidAttempts = _systemMethods.GetAPIDetailByAPIGUID(
                    _systemAPIGUIDEnums.LockUserAPI, 
                    _systemAPIAttributeEnums.MaximumInvalidLoginAttempts)
                    .Select(a => Convert.ToInt64(a))
                    .First();

                var isAttemptValid = invalidAttempts < maximumInvalidAttempts;

                //If invalid attempt count >= maximum allowed invalid attempts, lock the user
                if(!isAttemptValid)
                {
                    //Lock account by adding 'Account Locked' to user detail
                    _administrationMethods.UserDetail_Insert(
                        _administrationUserGUIDEnums.System, 
                        _informationSourceTypeEnums.UserGenerated, 
                        "Account Locked", 
                        "true");
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(queueGUID, _systemAPIGUIDEnums.LockUserAPI, !isAttemptValid);
            }
            else
            {
                //Update Process Queue
                _systemMethods.ProcessQueue_Update(queueGUID, _systemAPIGUIDEnums.LockUserAPI, false);
            }
        }
    }
}
