using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitLocalDistributionZoneToMeterDataApp
{
    class Program
    {
        private static readonly Methods.SystemSchema _systemMethods = new Methods.SystemSchema();

        static void Main(string[] args)
        {
            try
            {
                var password = "JT4paBW9eH4m6F35";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitLocalDistributionZoneToMeterDataAPI, password);
                var commitLocalDistributionZoneToMeterDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitLocalDistributionZoneToMeterDataAPI);

                var informationMethods = new Methods.InformationSchema();

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitLocalDistributionZoneToMeterDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitLocalDistributionZoneToMeterDataAPI, commitLocalDistributionZoneToMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId);
                
                var customerDataUploadProcessQueueGUID = _systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.TempSchema.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId, false, null);
                    return;
                }

                var mappingMethods = new Methods.MappingSchema();
                var customerMethods = new Methods.CustomerSchema();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var localDistributionZoneLocalDistributionZoneAttributeId = informationMethods.LocalDistributionZoneAttribute_GetLocalDistributionZoneAttributeIdByLocalDistributionZoneAttributeDescription(new Enums.InformationSchema.LocalDistributionZone.Attribute().LocalDistributionZone);

                var localDistributionZones = commitableMeterEntities.Select(cme => cme.LocalDistributionZone).Distinct()
                    .ToDictionary(ldz => ldz, ldz => informationMethods.GetLocalDistributionZoneId(ldz, createdByUserId, sourceId, localDistributionZoneLocalDistributionZoneAttributeId));
                
                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterEntity in commitableMeterEntities)
                {
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];

                    //Get LocalDistributionZoneId from [Information].[LocalDistributionZoneDetail]
                    var localDistributionZoneId = localDistributionZones[meterEntity.LocalDistributionZone];

                    //Get existing LocalDistributionZoneToMeter Id
                    var existingLocalDistributionZoneToMeterId = mappingMethods.LocalDistributionZoneToMeter_GetLocalDistributionZoneToMeterIdByLocalDistributionZoneIdAndMeterId(localDistributionZoneId, meterId);

                    if(existingLocalDistributionZoneToMeterId == 0)
                    {
                        //Insert into [Mapping].[LocalDistributionZoneToMeter]
                        mappingMethods.LocalDistributionZoneToMeter_Insert(createdByUserId, sourceId, localDistributionZoneId, meterId);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(1, 1, error);

                //Update Process Queue
                //_systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
