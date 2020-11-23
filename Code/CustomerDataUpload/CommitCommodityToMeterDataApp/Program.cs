using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitCommodityToMeterDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "dJPDrGp8DPndzw9w";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitCommodityToMeterDataAPI, password);
                var commitCommodityToMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitCommodityToMeterDataAPI);

                var methods = new Methods();
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
                    commitCommodityToMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitCommodityToMeterDataAPI, commitCommodityToMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitCommodityToMeterDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.TempSchema.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCommodityToMeterDataAPIId, false, null);
                    return;
                }

                var customerMethods = new Methods.CustomerSchema();
                var mappingMethods = new Methods.MappingSchema();
                var informationCommodityEnums = new Enums.InformationSchema.Commodity();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);

                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                var meterCommodity = meters.ToDictionary(m => m.Key, m => methods.IsValidMPAN(m.Key) ? informationCommodityEnums.Electricity : informationCommodityEnums.Gas);

                var commodities = meterCommodity.Select(m => m.Value).Distinct()
                    .ToDictionary(c => c, c => informationMethods.Commodity_GetCommodityIdByCommodityDescription(c));

                foreach(var meterEntity in commitableMeterEntities)
                {
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];

                    //Get CommodityId from [Information].[Commodity]
                    var commodity = meterCommodity[meterEntity.MPXN];
                    var commodityId = commodities[commodity];

                    var commodityToMeterId = mappingMethods.CommodityToMeter_GetCommodityToMeterIdByCommodityIdAndMeterId(commodityId, meterId);

                    if(commodityToMeterId == 0)
                    {
                        //Insert into [Mapping].[CommodityToMeter]
                        mappingMethods.CommodityToMeter_Insert(createdByUserId, sourceId, commodityId, meterId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCommodityToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitCommodityToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
