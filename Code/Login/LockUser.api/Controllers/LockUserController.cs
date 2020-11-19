using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace LockUser.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class LockUserController : ControllerBase
    {
        #region Variables
        private readonly ILogger<LockUserController> _logger;
        private readonly Int64 lockUserAPIId;
        private readonly string hostEnvironment;
        #endregion

        public LockUserController(ILogger<LockUserController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().LockUserAPI, password);
            lockUserAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().LockUserAPI);
        }

        [HttpPost]
        [Route("LockUser/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(lockUserAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("LockUser/Lock")]
        public void Lock([FromBody] object data)
        {
            var informationMethods = new Methods.Information();
            var administrationUserGUIDEnums = new Enums.AdministrationSchema.User.GUID();
            var systemAPIMethods = new Methods.System.API();
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    lockUserAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = systemAPIMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var result = systemAPIMethods.PostAsJsonAsyncAndAwaitResult(checkPrerequisiteAPIAPIId, new Enums.SystemSchema.API.GUID().LockUserAPI, hostEnvironment, jsonObject);
                var erroredPrerequisiteAPIs = new Methods().GetArray(result);
                var errorMessage = erroredPrerequisiteAPIs.Any() ? $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored" : null;

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, lockUserAPIId);

                //Get User Id
                var administrationUserMethods = new Methods.Administration.User();
                var userId = administrationUserMethods.GetUserIdByEmailAddress(jsonObject);
                
                //Check to see if account is already locked
                var administrationUserAttributeEnums = new Enums.AdministrationSchema.User.Attribute();
                var accountLockedAttributeId = administrationUserMethods.UserAttribute_GetUserAttributeIdByUserAttributeDescription(administrationUserAttributeEnums.AccountLocked);
                var accountLockedId = administrationUserMethods.UserDetail_GetUserDetailIdByUserIdAndUserAttributeId(userId, accountLockedAttributeId);

                if(accountLockedId == 0)
                {
                    //Account is not currently locked so check if it should be locked
                    if(erroredPrerequisiteAPIs.Any())
                    {
                        //A prerequisite API failed (either email address or password)
                        if(userId != 0)
                        {
                            //Get logins by user id and order by descending
                            var mappingMethods = new Methods.Mapping();
                            var loginList = mappingMethods.LoginToUser_GetLoginIdListByUserId(userId).OrderByDescending(l => l);

                            //Get invalidAttempts count
                            var administrationLoginMethods = new Methods.Administration.Login();
                            var invalidAttempts = administrationLoginMethods.CountInvalidAttempts(loginList);

                            //Get maximum attempts allowed before locking
                            var lockUserAPIId = systemAPIMethods.API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().LockUserAPI);

                            var systemAPIAttributeEnums = new Enums.SystemSchema.API.Attribute();
                            var maximumInvalidLoginAttemptsAttributeId = systemAPIMethods.APIAttribute_GetAPIAttributeIdByAPIAttributeDescription(systemAPIAttributeEnums.MaximumInvalidLoginAttempts);
                            var maximumInvalidAttempts = systemAPIMethods.APIDetail_GetAPIDetailDescriptionListByAPIIdAndAPIAttributeId(lockUserAPIId, maximumInvalidLoginAttemptsAttributeId)
                                .Select(a => Convert.ToInt64(a))
                                .FirstOrDefault();

                            //If invalid attempt count >= maximum allowed invalid attempts, lock the user
                            if (invalidAttempts >= maximumInvalidAttempts)
                            {
                                //Lock account by adding 'Account Locked' to user detail
                                administrationUserMethods.UserDetail_Insert(
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
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, lockUserAPIId, erroredPrerequisiteAPIs.Any(), errorMessage);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, lockUserAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}