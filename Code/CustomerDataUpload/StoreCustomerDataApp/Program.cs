using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreCustomerDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "qkaux33qraa6EZ9H";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreCustomerDataAPI, password);
                var storeCustomerDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreCustomerDataAPI);

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
                    storeCustomerDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreCustomerDataAPI, storeCustomerDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeCustomerDataAPIId);

                var tempCustomerDataUploadCustomerMethods = new Methods.TempSchema.CustomerDataUpload.Customer();

                //Get Customer data from Customer Data Upload
                var customerDictionary = new Methods.TempSchema.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets.Customers");

                foreach(var row in customerDictionary.Keys)
                {
                    var values = customerDictionary[row];

                    //Insert customer data into [Temp.CustomerDataUpload].[Customer]
                    //TODO: Make into BulkInsert
                    tempCustomerDataUploadCustomerMethods.Customer_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeCustomerDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeCustomerDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
