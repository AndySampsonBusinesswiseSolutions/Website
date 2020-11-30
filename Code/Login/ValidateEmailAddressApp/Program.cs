using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace ValidateEmailAddressApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "}h8FfD2r[Rd~PPNR";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().ValidateEmailAddressAPI, password);
                var validateEmailAddressAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().ValidateEmailAddressAPI);

                var administrationUserMethods = new Methods.AdministrationSchema.User();
                var systemMethods = new Methods.SystemSchema();

                //Get base variables
                var createdByUserId = administrationUserMethods.GetSystemUserId();
                var sourceId = new Methods.InformationSchema().GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    validateEmailAddressAPIId);

                // if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().ValidateEmailAddressAPI, validateEmailAddressAPIId, hostEnvironment, jsonObject))
                // {
                //     return;
                // }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, validateEmailAddressAPIId);

                //Get Email Address
                var emailAddress = systemMethods.GetEmailAddressFromJObject(jsonObject);

                //Validate Email Address
                var emailAddressId = administrationUserMethods.UserDetail_GetUserDetailIdByEmailAddress(emailAddress);

                //If emailAddressId == 0 then the email address provided isn't valid so create an error
                string errorMessage = null;
                if(emailAddressId == 0)
                {
                    errorMessage = $"Email address {emailAddress} does not exist in [Administration.User].[UserDetail] table";
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateEmailAddressAPIId, emailAddressId == 0, errorMessage);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, validateEmailAddressAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
