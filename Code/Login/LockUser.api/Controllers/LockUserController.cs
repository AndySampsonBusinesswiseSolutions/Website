using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

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
        private readonly Int64 lockUserAPIId;

        public LockUserController(ILogger<LockUserController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.LockUserAPI, _systemAPIPasswordEnums.LockUserAPI);
            lockUserAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.LockUserAPI);
        }

        [HttpPost]
        [Route("LockUser/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(lockUserAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("LockUser/Lock")]
        public void Lock([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.User_GetUserIdByUserGUID(_administrationUserGUIDEnums.System);
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    lockUserAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = _systemMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = _systemMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, _systemAPIGUIDEnums.LockUserAPI, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = _methods.GetArray(result.Result.ToString());
                var errorMessage = erroredPrerequisiteAPIs.Any() ? $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored" : null;

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, lockUserAPIId);

                //Get User Id
                var userId = _administrationMethods.GetUserIdByEmailAddress(jsonObject);
                
                //Check to see if account is already locked
                var accountLockedAttributeId = _administrationMethods.UserAttribute_GetUserAttributeIdByUserAttributeDescription(_administrationUserAttributeEnums.AccountLocked);
                var accountLockedId = _administrationMethods.UserDetail_GetUserDetailIdByUserIdAndUserAttributeId(userId, accountLockedAttributeId);

                if(accountLockedId == 0)
                {
                    //Account is not currently locked so check if it should be locked
                    if(erroredPrerequisiteAPIs.Any())
                    {
                        //A prerequisite API failed (either email address or password)
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
                                .FirstOrDefault();

                            //If invalid attempt count >= maximum allowed invalid attempts, lock the user
                            if (invalidAttempts >= maximumInvalidAttempts)
                            {
                                //Lock account by adding 'Account Locked' to user detail
                                _administrationMethods.UserDetail_Insert(
                                    createdByUserId,
                                    sourceId,
                                    userId,
                                    accountLockedAttributeId,
                                    "true");

                                //Update error message
                                errorMessage = $"Account Locked.{errorMessage}";
                            }
                        }
                    }
                }
                else
                {
                    //Update error message
                    errorMessage = $"Account Locked.{errorMessage ?? ""}";
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, lockUserAPIId, erroredPrerequisiteAPIs.Any(), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, lockUserAPIId, true, $"System Error Id {errorId}");
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
