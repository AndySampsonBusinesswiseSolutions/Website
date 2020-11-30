using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;

namespace StoreMeterExemptionDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "CNs2z2TrsqzZMu2J";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().StoreMeterExemptionDataAPI, password);
                var storeMeterExemptionDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().StoreMeterExemptionDataAPI);

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
                    storeMeterExemptionDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().StoreMeterExemptionDataAPI, storeMeterExemptionDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, storeMeterExemptionDataAPIId);

                var methods = new Methods();
                var tempCustomerDataUploadMeterExemptionMethods = new Methods.TempSchema.CustomerDataUpload.MeterExemption();

                //Get Meter Exemption data from Customer Data Upload
                //TODO: Make into Bulk Insert
                var meterExemptionDictionary = new Methods.TempSchema.CustomerDataUpload().ConvertCustomerDataUploadToDictionary(jsonObject, "Sheets['Meter Exemptions']");

                foreach(var row in meterExemptionDictionary.Keys)
                {
                    var values = meterExemptionDictionary[row];
                    var dateFrom = methods.GetDateTimeSqlParameterFromDateTimeString(values[1]);
                    var dateTo = methods.GetDateTimeSqlParameterFromDateTimeString(values[2]);

                    //Insert meter exemption data into [Temp.CustomerDataUpload].[MeterExemption]
                    tempCustomerDataUploadMeterExemptionMethods.MeterExemption_Insert(processQueueGUID, row, values[0], dateFrom, dateTo, values[3], values[4]);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeMeterExemptionDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, storeMeterExemptionDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
