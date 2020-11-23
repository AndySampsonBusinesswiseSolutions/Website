using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;

namespace StoreFixedContractDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "ReAjquZxWE6SrqjB";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFixedContractDataAPI, password);
                var storeFixedContractDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreFixedContractDataAPI);

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
                    storeFixedContractDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreFixedContractDataAPI, storeFixedContractDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeFixedContractDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadFixedContractMethods = new Methods.TempSchema.CustomerDataUpload.FixedContract();

                //Get Fixed Contract data from Customer Data Upload
                var fixedContractDictionary = new Methods.TempSchema.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Fixed Contracts']");
                var columns = new List<string>
                {
                    "StandingCharge", "CapacityCharge", "Rate1", "Rate2", "Rate3", "Rate4", "Rate5", "Rate6", "Rate7", "Rate8", "Rate9", "Rate10"
                };

                foreach(var row in fixedContractDictionary.Keys)
                {
                    var values = fixedContractDictionary[row];
                    var contractStartDate = methods.GetDateTimeSqlParameterFromDateTimeString(values[3]);
                    var contractEndDate = methods.GetDateTimeSqlParameterFromDateTimeString(values[4]);

                    for(var rateCount = 7; rateCount < values.Count(); rateCount++)
                    {
                        //Insert fixed contract data into [Temp.CustomerDataUpload].[FlexContract]
                        tempCustomerDataUploadFixedContractMethods.FixedContract_Insert(processQueueGUID, row, values[0], values[1], values[2], contractStartDate, contractEndDate, values[5], values[6], columns[rateCount - 7], values[rateCount]);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFixedContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFixedContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
