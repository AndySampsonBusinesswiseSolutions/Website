using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;

namespace LockUser.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class LockUserController : ControllerBase
    {
        private readonly ILogger<LockUserController> _logger;
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
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();
            var APIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.LockUserAPI);

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
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.LockUserAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetAPIArray(result.Result.ToString());
                    
                string errorMessage = erroredPrerequisiteAPIs.Any() ? $"Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored" : null;

                if(erroredPrerequisiteAPIs.Any())
                {
                    //Get User Id
                    var userId = _administrationMethods.GetUserIdByEmailAddress(jsonObject);

                    if(userId != 0)
                    {
                        //Get logins by user id and order by descending
                        var loginList = _mappingMethods.LoginToUser_GetLoginIdListByUserId(userId).OrderByDescending(l => l);

                        //Get invalidAttempts count
                        var invalidAttempts = CountInvalidAttempts(loginList);

                        //Get maximum attempts allowed before locking
                        var lockUserAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.LockUserAPI);
                        var maximumInvalidLoginAttemptsAttributeId = _systemMethods.APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(_systemAPIAttributeEnums.MaximumInvalidLoginAttempts);
                        var maximumInvalidAttempts = _systemMethods.APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(lockUserAPIId, maximumInvalidLoginAttemptsAttributeId)
                            .Select(a => Convert.ToInt64(a))
                            .First();

                        //If invalid attempt count >= maximum allowed invalid attempts, lock the user
                        if (invalidAttempts >= maximumInvalidAttempts)
                        {
                            //Lock account by adding 'Account Locked' to user detail
                            var accountLockedAttributeId = _administrationMethods.UserAttribute_GetUserAttributeIdByUserAttributeDescription(_administrationUserAttributeEnums.AccountLocked);

                            _administrationMethods.UserDetail_Insert(
                                createdByUserId,
                                sourceId,
                                userId,
                                accountLockedAttributeId,
                                "true");

                            //Update error message
                            errorMessage = $"Account Locked. {errorMessage}";
                        }
                    }
                }

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

        private int CountInvalidAttempts(IOrderedEnumerable<long> loginList)
        {
            var invalidAttempts = 0;

            //Loop through each login
            foreach (var login in loginList)
            {
                //Get LoginSuccessful attribute
                var loginSucessful = _administrationMethods.Login_GetLoginSuccessfulByLoginId(login);

                //If login is successful, then exit loop
                //Else increment invalidAttempts
                if (loginSucessful)
                {
                    break;
                }
                else
                {
                    invalidAttempts++;
                }
            }

            return invalidAttempts;
        }
    }
}
