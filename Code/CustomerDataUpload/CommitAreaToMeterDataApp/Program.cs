using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitAreaToMeterDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "x4dzGMHS2CCJgyqD";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitAreaToMeterDataAPI, password);
                var commitAreaToMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitAreaToMeterDataAPI);

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
                    commitAreaToMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitAreaToMeterDataAPI, commitAreaToMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitAreaToMeterDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.TempSchema.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAreaToMeterDataAPIId, false, null);
                    return;
                }

                var mappingMethods = new Methods.MappingSchema();
                var customerMethods = new Methods.CustomerSchema();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                
                var areas = commitableMeterEntities.Select(cme => cme.Area).Distinct()
                    .ToDictionary(a => a, a => informationMethods.GetAreaId(createdByUserId, sourceId, a));
                
                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                var newAreaToMeterEntities = commitableMeterEntities.Where(cme => mappingMethods.AreaToMeter_GetAreaToMeterIdByAreaIdAndMeterId(areas[cme.Area], meters[cme.MPXN]) == 0)
                    .GroupBy(cme => new { cme.Area, cme.MPXN }).ToList();

                foreach(var meterEntity in newAreaToMeterEntities)
                {
                    //End date existing AreaToMeter mappings
                    mappingMethods.AreaToMeter_DeleteByMeterId(meters[meterEntity.Key.MPXN]);

                    //Insert into [Mapping].[AreaToMeter]
                    mappingMethods.AreaToMeter_Insert(createdByUserId, sourceId, areas[meterEntity.Key.Area], meters[meterEntity.Key.MPXN]);            
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAreaToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAreaToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
