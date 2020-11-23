using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitMeterToMeterTimeswitchCodeDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "7wSWYFFtFGr6qETP";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToMeterTimeswitchCodeDataAPI, password);
                var commitMeterToMeterTimeswitchCodeDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterToMeterTimeswitchCodeDataAPI);

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
                    commitMeterToMeterTimeswitchCodeDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitMeterToMeterTimeswitchCodeDataAPI, commitMeterToMeterTimeswitchCodeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId);

                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.TempSchema.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, false, null);
                    return;
                }

                var informationMeterTimeswitchCodeAttributeEnums = new Enums.InformationSchema.MeterTimeswitchCode.Attribute();
                var mappingMethods = new Methods.MappingSchema();
                var customerMethods = new Methods.CustomerSchema();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var meterTimeswitchCodeRangeStartMeterTimeswitchCodeAttributeId = informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchCodeRangeStart);
                var meterTimeswitchCodeRangeEndMeterTimeswitchCodeAttributeId = informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchCodeRangeEnd);

                var meterTimeswitchCodeRangeStartEntities = informationMethods.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(meterTimeswitchCodeRangeStartMeterTimeswitchCodeAttributeId);
                var meterTimeswitchCodeRangeEndEntities = informationMethods.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(meterTimeswitchCodeRangeEndMeterTimeswitchCodeAttributeId);

                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterEntity in commitableMeterEntities)
                {
                    //Get MeterTimeswitchCodeId from [Information].[MeterTimeswitchCodeDetail]
                    if(string.IsNullOrWhiteSpace(meterEntity.MeterTimeswitchCode))
                    {
                        continue;
                    }

                    var meterTimeswitchCodeValue = Convert.ToInt64(meterEntity.MeterTimeswitchCode);

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];
                    
                    var validRangeStartEntities = meterTimeswitchCodeRangeStartEntities.Where(mtcrse => Convert.ToInt64(mtcrse.MeterTimeswitchCodeDetailDescription) <= meterTimeswitchCodeValue);
                    var validRangeEndEntities = meterTimeswitchCodeRangeEndEntities.Where(mtcree => Convert.ToInt64(mtcree.MeterTimeswitchCodeDetailDescription) >= meterTimeswitchCodeValue);

                    //Get MeterTimeswitchIds
                    var validRangeStartMeterTimeswitchIdList = validRangeStartEntities.Select(r => r.MeterTimeswitchCodeId);
                    var validRangeEndMeterTimeswitchIdList = validRangeEndEntities.Select(r => r.MeterTimeswitchCodeId);

                    var meterTimeswitchCodeId = validRangeStartMeterTimeswitchIdList.Intersect(validRangeEndMeterTimeswitchIdList).FirstOrDefault();

                    //Get existing MeterToMeterTimeswitchCode Id
                    var existingMeterToMeterTimeswitchCodeId = mappingMethods.MeterToMeterTimeswitchCode_GetMeterToMeterTimeswitchCodeIdByMeterIdAndMeterTimeswitchCodeId(meterId, meterTimeswitchCodeId);

                    if(existingMeterToMeterTimeswitchCodeId == 0)
                    {
                        //Insert into [Mapping].[MeterToMeterTimeswitchCode]
                        mappingMethods.MeterToMeterTimeswitchCode_Insert(createdByUserId, sourceId, meterTimeswitchCodeId, meterId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
