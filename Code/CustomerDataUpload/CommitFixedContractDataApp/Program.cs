using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CommitFixedContractDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "PQrhQL3PCrchDXnj";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitFixedContractDataAPI, password);
                var commitFixedContractDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitFixedContractDataAPI);

                var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
                var systemAPIMethods = new Methods.SystemSchema.API();
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
                    commitFixedContractDataAPIId);

                if(!systemAPIMethods.PrerequisiteAPIsAreSuccessful(systemAPIGUIDEnums.CommitFixedContractDataAPI, commitFixedContractDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitFixedContractDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[FixedContract] where CanCommit = 1
                var fixedContractEntities = new Methods.TempSchema.CustomerDataUpload.FixedContract().FixedContract_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableFixedContractEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(fixedContractEntities);

                if(!commitableFixedContractEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFixedContractDataAPIId, false, null);
                    return;
                }

                var systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();

                //Add ContractType to jsonObject
                jsonObject.Add(systemAPIRequiredDataKeyEnums.ContractType, new Enums.InformationSchema.ContractType().Fixed);

                //Convert entities to a string
                var commitableEntityJSON = JsonConvert.SerializeObject(commitableFixedContractEntities.Where(cfce => !string.IsNullOrWhiteSpace(cfce.Value)));

                //Add ContractData to jsonObject
                jsonObject.Add(systemAPIRequiredDataKeyEnums.ContractData, commitableEntityJSON);

                //Call CommitContractData API and wait for response
                var APIId = systemAPIMethods.API_GetAPIIdByAPIGUID(systemAPIGUIDEnums.CommitContractDataAPI);
                var API = systemAPIMethods.PostAsJsonAsync(APIId, systemAPIGUIDEnums.CommitFixedContractDataAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFixedContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFixedContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
