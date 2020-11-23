using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CommitFlexContractDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "MnXB6w8fSZuKuHL9";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitFlexContractDataAPI, password);
                var commitFlexContractDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitFlexContractDataAPI);

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
                    commitFlexContractDataAPIId);

                if(!systemAPIMethods.PrerequisiteAPIsAreSuccessful(systemAPIGUIDEnums.CommitFlexContractDataAPI, commitFlexContractDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitFlexContractDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[FlexContract] where CanCommit = 1
                var flexContractEntities = new Methods.TempSchema.CustomerDataUpload.FlexContract().FlexContract_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableFlexContractEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(flexContractEntities);

                if(!commitableFlexContractEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexContractDataAPIId, false, null);
                    return;
                }

                var systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();

                //Add ContractType to jsonObject
                jsonObject.Add(systemAPIRequiredDataKeyEnums.ContractType, new Enums.InformationSchema.ContractType().Flex);

                //Convert dataRows to a string
                var commitableEntityJSON = JsonConvert.SerializeObject(commitableFlexContractEntities.Where(cfce => !string.IsNullOrWhiteSpace(cfce.Value)));

                //Add ContractData to jsonObject
                jsonObject.Add(systemAPIRequiredDataKeyEnums.ContractData, commitableEntityJSON);

                //Call CommitContractData API and wait for response
                var APIId = systemAPIMethods.API_GetAPIIdByAPIGUID(systemAPIGUIDEnums.CommitContractDataAPI);
                var API = systemAPIMethods.PostAsJsonAsync(APIId, systemAPIGUIDEnums.CommitFlexContractDataAPI, hostEnvironment, jsonObject);
                var result = API.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitFlexContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
