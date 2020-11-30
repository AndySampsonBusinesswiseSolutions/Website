using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace LockUserApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "JM7!?q#g#uTyM^!v";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().LockUserAPI, password);
                var lockUserAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().LockUserAPI);

                var informationMethods = new Methods.InformationSchema();
                var administrationUserGUIDEnums = new Enums.AdministrationSchema.User.GUID();
                var systemAPIMethods = new Methods.SystemSchema.API();
                var systemMethods = new Methods.SystemSchema();

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    lockUserAPIId);

                //Get CheckPrerequisiteAPI API Id
                var checkPrerequisiteAPIAPIId = systemAPIMethods.GetCheckPrerequisiteAPIAPIId();

                //Call CheckPrerequisiteAPI API
                var API = systemAPIMethods.PostAsJsonAsync(checkPrerequisiteAPIAPIId, new Enums.SystemSchema.API.GUID().LockUserAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();
                var erroredPrerequisiteAPIs = new Methods().GetArray(result.Result.ToString());
                var errorMessage = erroredPrerequisiteAPIs.Any() ? $" Prerequisite APIs {string.Join(",", erroredPrerequisiteAPIs)} errored" : null;

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, lockUserAPIId);

                //Get User Id
                var administrationUserMethods = new Methods.AdministrationSchema.User();
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
                            var mappingMethods = new Methods.MappingSchema();
                            var loginList = mappingMethods.LoginToUser_GetLoginIdListByUserId(userId).OrderByDescending(l => l);

                            //Get invalidAttempts count
                            var administrationLoginMethods = new Methods.AdministrationSchema.Login();
                            var invalidAttempts = administrationLoginMethods.CountInvalidAttempts(loginList);

                            //Get maximum attempts allowed before locking
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
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, lockUserAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
