using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommitSubMeterDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "nZYGJvSe4Ej3dzFH";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitSubMeterDataAPI, password);
                var commitSubMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitSubMeterDataAPI);

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
                    commitSubMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitSubMeterDataAPI, commitSubMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitSubMeterDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterEntities = new Methods.TempSchema.CustomerDataUpload.SubMeter().SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSubMeterEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(subMeterEntities);

                if(!commitableSubMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterDataAPIId, false, null);
                    return;
                }

                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
                var customerSubMeterAttributeEnums = new Enums.CustomerSchema.SubMeter.Attribute();
                var customerMethods = new Methods.CustomerSchema();

                //For each column, get SubMeterAttributeId
                var attributes = new Dictionary<long, string>
                {
                    {customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(customerSubMeterAttributeEnums.SubMeterIdentifier), customerDataUploadValidationEntityEnums.SubMeterIdentifier},
                    {customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(customerSubMeterAttributeEnums.SerialNumber), customerDataUploadValidationEntityEnums.SerialNumber},
                };

                var subMeterIdList = new List<long>();

                foreach(var subMeterEntity in commitableSubMeterEntities)
                {
                    var detailDictionary = attributes.ToDictionary(
                        a => a.Key,
                        a => subMeterEntity.GetType().GetProperty(a.Value).GetValue(subMeterEntity).ToString()
                    );

                    //Get SubMeterId by MPXN
                    var subMeterId = customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(detailDictionary.First().Key, detailDictionary.First().Value);

                    if(subMeterId == 0)
                    {
                        subMeterId = customerMethods.InsertNewSubMeter(createdByUserId, sourceId);

                        //Insert into [Customer].[SubMeterDetail]
                        foreach(var detail in detailDictionary)
                        {
                            customerMethods.SubMeterDetail_Insert(createdByUserId, sourceId, subMeterId, detail.Key, detail.Value);
                        }
                    }
                    else
                    {
                        //Update [Customer].[SubMeterDetail]
                        foreach(var detail in detailDictionary)
                        {
                            var currentDetailEntity = customerMethods.SubMeterDetail_GetBySubMeterIdAndSubMeterAttributeId(subMeterId, detail.Key);

                            if(detail.Value != currentDetailEntity.SubMeterDetailDescription)
                            {
                                customerMethods.SubMeterDetail_DeleteBySubMeterDetailId(currentDetailEntity.SubMeterDetailId);
                                customerMethods.SubMeterDetail_Insert(createdByUserId, sourceId, subMeterId, detail.Key, detail.Value);
                            }
                        }
                    }

                    if(!subMeterIdList.Contains(subMeterId)) 
                    {
                        subMeterIdList.Add(subMeterId);
                    }
                }

                //Create SubMeter tables
                var supplyMethods = new Methods.SupplySchema();
                var meterType = "SubMeter";
                Parallel.ForEach(subMeterIdList, new ParallelOptions{MaxDegreeOfParallelism = 5}, subMeterId => {
                    supplyMethods.CreateMeterTables($"Supply.{meterType}{subMeterId}", subMeterId, meterType);
                });

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
