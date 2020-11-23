using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreFlexReferenceVolumeDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "L5msq6pjxEqMAAf4";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreFlexReferenceVolumeDataAPI, password);
                var storeFlexReferenceVolumeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreFlexReferenceVolumeDataAPI);

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
                    storeFlexReferenceVolumeDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreFlexReferenceVolumeDataAPI, storeFlexReferenceVolumeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeFlexReferenceVolumeDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadFlexReferenceVolumeMethods = new Methods.TempSchema.CustomerDataUpload.FlexReferenceVolume();

                //Get Flex Reference Volume data from Customer Data Upload
                //TODO: Make into Bulk Insert
                var flexReferenceVolumeDictionary = new Methods.TempSchema.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Flex Reference Volumes']");

                foreach(var row in flexReferenceVolumeDictionary.Keys)
                {
                    var values = flexReferenceVolumeDictionary[row];
                    var dateFrom = methods.GetDateTimeSqlParameterFromDateTimeString(values[1]);
                    var dateTo = methods.GetDateTimeSqlParameterFromDateTimeString(values[2]);

                    //Insert flex reference volume data into [Temp.CustomerDataUpload].[FlexReferenceVolume]
                    tempCustomerDataUploadFlexReferenceVolumeMethods.FlexReferenceVolume_Insert(processQueueGUID, row, values[0], dateFrom, dateTo, values[3]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexReferenceVolumeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeFlexReferenceVolumeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
