using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitGridSupplyPointToMeterDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "TLbyfyMNz2NTGMzb";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitGridSupplyPointToMeterDataAPI, password);
                var commitGridSupplyPointToMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitGridSupplyPointToMeterDataAPI);

                var informationMethods = new Methods.InformationSchema();
                var systemMethods = new Methods.SystemSchema();

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
                    commitGridSupplyPointToMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitGridSupplyPointToMeterDataAPI, commitGridSupplyPointToMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitGridSupplyPointToMeterDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.TempSchema.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitGridSupplyPointToMeterDataAPIId, false, null);
                    return;
                }

                var mappingMethods = new Methods.MappingSchema();
                var customerMethods = new Methods.CustomerSchema();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var gridSupplyPointGroupIdGridSupplyPointAttributeId = informationMethods.GridSupplyPointAttribute_GetGridSupplyPointAttributeIdByGridSupplyPointAttributeDescription(new Enums.InformationSchema.GridSupplyPoint.Attribute().GridSupplyPointGroupId);

                var gridSupplyPoints = commitableMeterEntities.Where(cme => !string.IsNullOrWhiteSpace(cme.GridSupplyPoint)).Select(cme => cme.GridSupplyPoint).Distinct()
                    .ToDictionary(gsp => gsp, gsp => informationMethods.GetGridSupplyPointId(gsp, createdByUserId, sourceId, gridSupplyPointGroupIdGridSupplyPointAttributeId));
                
                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterEntity in commitableMeterEntities)
                {
                    if(string.IsNullOrWhiteSpace(meterEntity.GridSupplyPoint))
                    {
                        continue;
                    }
                    
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];

                    //Get GridSupplyPointId from [Information].[GridSupplyPointDetail]
                    var gridSupplyPointId = gridSupplyPoints[meterEntity.GridSupplyPoint];

                    //Get existing GridSupplyPointToMeter Id
                    var existingGridSupplyPointToMeterId = mappingMethods.GridSupplyPointToMeter_GetGridSupplyPointToMeterIdByGridSupplyPointIdAndMeterId(gridSupplyPointId, meterId);

                    if(existingGridSupplyPointToMeterId == 0)
                    {
                        //Insert into [Mapping].[GridSupplyPointToMeter]
                        mappingMethods.GridSupplyPointToMeter_Insert(createdByUserId, sourceId, gridSupplyPointId, meterId);    
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitGridSupplyPointToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitGridSupplyPointToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
