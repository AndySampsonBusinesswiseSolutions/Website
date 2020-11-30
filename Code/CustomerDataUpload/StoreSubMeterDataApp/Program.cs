using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreSubMeterDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "HHq85F87Ymc7P4X7";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreSubMeterDataAPI, password);
                var storeSubMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreSubMeterDataAPI);

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
                    storeSubMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreSubMeterDataAPI, storeSubMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeSubMeterDataAPIId);

                var tempCustomerDataUploadSubMeterMethods = new Methods.TempSchema.CustomerDataUpload.SubMeter();

                //Get SubMeter data from Customer Data Upload
                //TODO: Make into Bulk Insert
                var subMeterDictionary = new Methods.TempSchema.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets.SubMeters");

                foreach(var row in subMeterDictionary.Keys)
                {
                    var values = subMeterDictionary[row];

                    //Insert submeter data into [Temp.CustomerDataUpload].[SubMeter]
                    tempCustomerDataUploadSubMeterMethods.SubMeter_Insert(processQueueGUID, row, values[0], values[1], values[2], values[3], values[4]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSubMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
