using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitMeterExemptionDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "hzRHNnabT4hc6Mzf";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterExemptionDataAPI, password);
                var commitMeterExemptionDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterExemptionDataAPI);

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
                    commitMeterExemptionDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitMeterExemptionDataAPI, commitMeterExemptionDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterExemptionDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[MeterExemption] where CanCommit = 1
                var meterExemptionEntities = new Methods.TempSchema.CustomerDataUpload.MeterExemption().MeterExemption_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterExemptionEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(meterExemptionEntities);

                if(!commitableMeterExemptionEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterExemptionDataAPIId, false, null);
                    return;
                }

                var informationMeterExemptionAttributeEnums = new Enums.InformationSchema.MeterExemption.Attribute();
                var customerMeterExemptionAttributeEnums = new Enums.CustomerSchema.MeterExemption.Attribute();
                var mappingMethods = new Methods.MappingSchema();
                var customerMethods = new Methods.CustomerSchema();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var meterExemptionProductMeterExemptionAttributeId = informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(informationMeterExemptionAttributeEnums.MeterExemptionProduct);
                var meterExemptionProportionMeterExemptionAttributeId = informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(informationMeterExemptionAttributeEnums.MeterExemptionProportion);
                var useDefaultValueMeterExemptionAttributeId = informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(informationMeterExemptionAttributeEnums.UseDefaultValue);

                var dateFromMeterExemptionAttributeId = customerMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(customerMeterExemptionAttributeEnums.DateFrom);
                var dateToMeterExemptionAttributeId = customerMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(customerMeterExemptionAttributeEnums.DateTo);
                var exemptionProportionMeterExemptionAttributeId = customerMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(customerMeterExemptionAttributeEnums.ExemptionProportion);

                var meterExemptions = commitableMeterExemptionEntities.Select(cmee => cmee.ExemptionProduct).Distinct()
                    .ToDictionary(me => me, me => informationMethods.MeterExemptionDetail_GetMeterExemptionIdByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(meterExemptionProductMeterExemptionAttributeId, me));
                
                var meters = commitableMeterExemptionEntities.Select(cmee => cmee.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterExemptionEntity in commitableMeterExemptionEntities)
                {
                    //Get MeterExemptionId from [Information].[MeterExemption]
                    var meterExemptionId = meterExemptions[meterExemptionEntity.ExemptionProduct];

                    //Check if a default value exists
                    var useDefaultValue = informationMethods.MeterExemptionDetail_GetMeterExemptionDetailDescriptionByMeterExemptionIdAndMeterExemptionAttributeId(meterExemptionId, useDefaultValueMeterExemptionAttributeId);

                    //Get ExemptionId from [Customer].[MeterExemptionDetail] by DateFrom, DateTo and ExemptionProportion
                    var exemptionProportion = string.IsNullOrWhiteSpace(useDefaultValue)
                        ? (Convert.ToInt64(meterExemptionEntity.ExemptionProportion.Replace("%", string.Empty))/100M).ToString()
                        : informationMethods.MeterExemptionDetail_GetMeterExemptionDetailDescriptionByMeterExemptionIdAndMeterExemptionAttributeId(meterExemptionId, meterExemptionProportionMeterExemptionAttributeId);

                    var customerMeterExemptionId = 0L;
                    var dateFromMeterExemptionIdList = customerMethods.MeterExemptionDetail_GetMeterExemptionIdListByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(dateFromMeterExemptionAttributeId, meterExemptionEntity.DateFrom);
                    if(dateFromMeterExemptionIdList.Any())
                    {
                        var dateToMeterExemptionIdList = customerMethods.MeterExemptionDetail_GetMeterExemptionIdListByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(dateToMeterExemptionAttributeId, meterExemptionEntity.DateTo);
                        if(dateToMeterExemptionIdList.Any())
                        {
                            var exemptionProportionMeterExemptionIdList = customerMethods.MeterExemptionDetail_GetMeterExemptionIdListByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(exemptionProportionMeterExemptionAttributeId, exemptionProportion);
                            if(exemptionProportionMeterExemptionIdList.Any())
                            {
                                customerMeterExemptionId = dateFromMeterExemptionIdList.Intersect(dateToMeterExemptionIdList).Intersect(exemptionProportionMeterExemptionIdList).FirstOrDefault();
                            }
                        }
                    }

                    if(customerMeterExemptionId == 0)
                    {
                        customerMeterExemptionId = customerMethods.InsertNewMeterExemption(createdByUserId, sourceId);

                        //Insert into [Customer].[MeterExemptionDetail]
                        customerMethods.MeterExemptionDetail_Insert(createdByUserId, sourceId, customerMeterExemptionId, dateFromMeterExemptionAttributeId, meterExemptionEntity.DateFrom);
                        customerMethods.MeterExemptionDetail_Insert(createdByUserId, sourceId, customerMeterExemptionId, dateToMeterExemptionAttributeId, meterExemptionEntity.DateTo);
                        customerMethods.MeterExemptionDetail_Insert(createdByUserId, sourceId, customerMeterExemptionId, exemptionProportionMeterExemptionAttributeId, exemptionProportion);
                    }

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterExemptionEntity.MPXN];

                    //Get existing MeterToMeterExemption Id
                    var existingMeterToMeterExemptionId = mappingMethods.MeterToMeterExemption_GetMeterToMeterExemptionIdByMeterIdAndMeterExemptionId(meterId, customerMeterExemptionId);

                    if(existingMeterToMeterExemptionId == 0)
                    {
                        //Insert into [Mapping].[MeterToMeterExemption]
                        mappingMethods.MeterToMeterExemption_Insert(createdByUserId, sourceId, meterId, customerMeterExemptionId);
                        existingMeterToMeterExemptionId = mappingMethods.MeterToMeterExemption_GetMeterToMeterExemptionIdByMeterIdAndMeterExemptionId(meterId, customerMeterExemptionId);
                    }

                    //Get existing MeterExemptionToMeterExemptionProduct Id
                    var existingMeterExemptionToMeterExemptionProductId = mappingMethods.MeterExemptionToMeterExemptionProduct_GetMeterExemptionToMeterExemptionProductIdByMeterExemptionIdAndMeterExemptionProductId(customerMeterExemptionId, meterExemptionId);

                    if(existingMeterExemptionToMeterExemptionProductId == 0)
                    {
                        //Insert into [Mapping].[MeterExemptionToMeterExemptionProduct]
                        mappingMethods.MeterExemptionToMeterExemptionProduct_Insert(createdByUserId, sourceId, customerMeterExemptionId, meterExemptionId);
                    }

                    //Get existing MeterToMeterExemptionToMeterExemptionProduct Id
                    var existingMeterToMeterExemptionToMeterExemptionProductId = mappingMethods.MeterToMeterExemptionToMeterExemptionProduct_GetMeterToMeterExemptionToMeterExemptionProductIdByMeterToMeterExemptionIdAndMeterExemptionProductId(existingMeterToMeterExemptionId, meterExemptionId);

                    if(existingMeterToMeterExemptionToMeterExemptionProductId == 0)
                    {
                        //Insert into [Mapping].[MeterToMeterExemptionToMeterExemptionProduct]
                        mappingMethods.MeterToMeterExemptionToMeterExemptionProduct_Insert(createdByUserId, sourceId, existingMeterToMeterExemptionId, meterExemptionId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterExemptionDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterExemptionDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
