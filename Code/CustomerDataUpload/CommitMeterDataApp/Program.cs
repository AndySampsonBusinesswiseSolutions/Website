using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommitMeterDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "jC7jNZZnz63nNysc";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterDataAPI, password);
                var commitMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterDataAPI);

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
                    commitMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitMeterDataAPI, commitMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.TempSchema.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterDataAPIId, false, null);
                    return;
                }

                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
                var customerMethods = new Methods.CustomerSchema();

                //For each column, get MeterAttributeId
                var attributes = new Dictionary<long, string>
                {
                    {customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(customerMeterAttributeEnums.MeterIdentifier), customerDataUploadValidationEntityEnums.MPXN},
                    {customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(customerMeterAttributeEnums.SupplyCapacity), customerDataUploadValidationEntityEnums.Capacity},
                    {customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(customerMeterAttributeEnums.StandardOfftakeQuantity), customerDataUploadValidationEntityEnums.StandardOfftakeQuantity},
                    {customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(customerMeterAttributeEnums.AnnualUsage), customerDataUploadValidationEntityEnums.AnnualUsage},
                    {customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(customerMeterAttributeEnums.MeterSerialNumber), customerDataUploadValidationEntityEnums.MeterSerialNumber},
                    {customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(customerMeterAttributeEnums.ImportExport), customerDataUploadValidationEntityEnums.ImportExport},
                };

                var meterIdList = new List<long>();

                foreach(var meterEntity in commitableMeterEntities)
                {
                    var detailDictionary = attributes.ToDictionary(
                        a => a.Key,
                        a => meterEntity.GetType().GetProperty(a.Value).GetValue(meterEntity).ToString()
                    );

                    //Get MeterId by MPXN
                    var meterId = customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(detailDictionary.First().Key, detailDictionary.First().Value).FirstOrDefault();

                    if(meterId == 0)
                    {
                        meterId = customerMethods.InsertNewMeter(createdByUserId, sourceId);

                        //Insert into [Customer].[MeterDetail]
                        foreach(var detail in detailDictionary)
                        {
                            customerMethods.MeterDetail_Insert(createdByUserId, sourceId, meterId, detail.Key, detail.Value);
                        }
                    }
                    else
                    {
                        //Update [Customer].[MeterDetail]
                        foreach(var detail in detailDictionary)
                        {
                            var currentDetailEntity = customerMethods.MeterDetail_GetByMeterIdAndMeterAttributeId(meterId, detail.Key);

                            if(detail.Value != currentDetailEntity.MeterDetailDescription)
                            {
                                customerMethods.MeterDetail_DeleteByMeterDetailId(currentDetailEntity.MeterDetailId);
                                customerMethods.MeterDetail_Insert(createdByUserId, sourceId, meterId, detail.Key, detail.Value);
                            }
                        }
                    }

                    if(!meterIdList.Contains(meterId)) 
                    {
                        meterIdList.Add(meterId);
                    }
                }

                //Create Meter tables
                var supplyMethods = new Methods.SupplySchema();
                var meterType = "Meter";
                Parallel.ForEach(meterIdList, new ParallelOptions{MaxDegreeOfParallelism = 5}, meterId => {
                    supplyMethods.CreateMeterTables($"Supply.{meterType}{meterId}", meterId, meterType);
                });

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
