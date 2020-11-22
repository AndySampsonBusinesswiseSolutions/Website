using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace ValidatePasswordApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "7zhGuASSgmrJJFkd";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidatePasswordAPI, password);
                var validatePasswordAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidatePasswordAPI);

                var systemMethods = new Methods.SystemSchema();         

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    validatePasswordAPIId);

                // if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidatePasswordAPI, validatePasswordAPIId, hostEnvironment, jsonObject))
                // {
                //     return;
                // }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validatePasswordAPIId);

                //Get Password
                var loginPassword = systemMethods.GetPasswordFromJObject(jsonObject);

                //Validate Password
                var passwordId = new Methods.AdministrationSchema.Password().Password_GetPasswordIdByPassword(loginPassword);
                string errorMessage = null;

                //If passwordId == 0 then the password provided isn't valid so create an error
                if(passwordId == 0)
                {
                    errorMessage = $"Password {loginPassword} does not exist in [Administration.User].[Password] table";
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validatePasswordAPIId, passwordId == 0, errorMessage);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validatePasswordAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
