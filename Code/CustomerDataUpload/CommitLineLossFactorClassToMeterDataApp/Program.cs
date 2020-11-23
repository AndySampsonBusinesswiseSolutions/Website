using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace CommitLineLossFactorClassToMeterDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "BUJXnVrsxzCBEX58";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitLineLossFactorClassToMeterDataAPI, password);
                var commitLineLossFactorClassToMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitLineLossFactorClassToMeterDataAPI);

                var systemMethods = new Methods.SystemSchema();
                var informationMethods = new Methods.InformationSchema();

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
                    commitLineLossFactorClassToMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitLineLossFactorClassToMeterDataAPI, commitLineLossFactorClassToMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //TODO: Build once LLF process is built

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
