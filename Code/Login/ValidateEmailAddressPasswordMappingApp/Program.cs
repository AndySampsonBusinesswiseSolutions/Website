using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace ValidateEmailAddressPasswordMappingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "GQzD2!aZNvffr*zC";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateEmailAddressPasswordMappingAPI, password);
                var validateEmailAddressPasswordMappingAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateEmailAddressPasswordMappingAPI);

                var administrationUserMethods = new Methods.AdministrationSchema.User();
                var informationMethods = new Methods.InformationSchema();
                var systemMethods = new Methods.SystemSchema();

                //Get base variables
                var createdByUserId = administrationUserMethods.GetSystemUserId();
                var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    validateEmailAddressPasswordMappingAPIId);

                // if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateEmailAddressPasswordMappingAPI, validateEmailAddressPasswordMappingAPIId, hostEnvironment, jsonObject))
                // {
                //     return;
                // }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateEmailAddressPasswordMappingAPIId);

                //Get Password Id
                var loginPassword = systemMethods.GetPasswordFromJObject(jsonObject);
                var passwordId = new Methods.AdministrationSchema.Password().Password_GetPasswordIdByPassword(loginPassword);

                //Get User Id
                var userId = administrationUserMethods.GetUserIdByEmailAddress(jsonObject);

                //Validate Password and User combination
                var mappingId = new Methods.MappingSchema().PasswordToUser_GetPasswordFromJObjectToUserIdByPasswordIdAndUserId(passwordId, userId);
                string errorMessage = null;

                //If mappingId == 0 then the combination of email address and password provided isn't valid so create an error
                if(mappingId == 0)
                {
                    errorMessage = $"UserId/PasswordId combination {userId}/{passwordId} does not exist in [Mapping].[PasswordToUser] table";
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateEmailAddressPasswordMappingAPIId, mappingId == 0, errorMessage);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateEmailAddressPasswordMappingAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
