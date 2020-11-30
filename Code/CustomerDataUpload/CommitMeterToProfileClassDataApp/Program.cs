using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitMeterToProfileClassDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "EwLC5zy5SjCuEmmj";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToProfileClassDataAPI, password);
                var commitMeterToProfileClassDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterToProfileClassDataAPI);

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
                    commitMeterToProfileClassDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitMeterToProfileClassDataAPI, commitMeterToProfileClassDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.TempSchema.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId, false, null);
                    return;
                }

                var mappingMethods = new Methods.MappingSchema();
                var customerMethods = new Methods.CustomerSchema();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var profileClassCodeProfileClassAttributeId = informationMethods.ProfileClassAttribute_GetProfileClassAttributeIdByProfileClassAttributeDescription(new Enums.InformationSchema.ProfileClass.Attribute().ProfileClassCode);

                var profileClasses = commitableMeterEntities.Where(cme => !string.IsNullOrWhiteSpace(cme.ProfileClass)).Select(cme => cme.ProfileClass).Distinct()
                    .ToDictionary(pc => pc, pc => informationMethods.ProfileClassDetail_GetProfileClassIdByProfileClassAttributeIdAndProfileClassDetailDescription(profileClassCodeProfileClassAttributeId, pc));
                
                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterEntity in commitableMeterEntities)
                {
                    if(string.IsNullOrWhiteSpace(meterEntity.ProfileClass))
                    {
                        continue;
                    }

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];
                    
                    //Get ProfileClassId from [Information].[ProfileClassDetail]
                    var profileClassId = profileClasses[meterEntity.ProfileClass];

                    //Does mapping exist between meter and profile class
                    var existingMeterProfileClassId = mappingMethods.MeterToProfileClass_GetProfileClassIdByMeterId(meterId);

                    if(existingMeterProfileClassId != profileClassId)
                    {
                        //Insert into [Mapping].[MeterToProfileClass]
                        mappingMethods.MeterToProfileClass_Insert(createdByUserId, sourceId, profileClassId, meterId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
