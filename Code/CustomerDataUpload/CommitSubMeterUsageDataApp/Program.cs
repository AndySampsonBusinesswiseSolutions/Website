using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CommitSubMeterUsageDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "BahJpvV8hyTwC3Bt";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitSubMeterUsageDataAPI, password);
                var commitSubMeterUsageDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitSubMeterUsageDataAPI);

                var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
                var informationMethods = new Methods.InformationSchema();
                var systemAPIMethods = new Methods.SystemSchema.API();
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
                    commitSubMeterUsageDataAPIId);

                if(!systemAPIMethods.PrerequisiteAPIsAreSuccessful(systemAPIGUIDEnums.CommitSubMeterUsageDataAPI, commitSubMeterUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitSubMeterUsageDataAPIId);
            
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var tempCustomerDataUploadMethods = new Methods.TempSchema.CustomerDataUpload();
                var subMeterEntities = new Methods.TempSchema.CustomerDataUpload.SubMeter().SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSubMeterEntities = tempCustomerDataUploadMethods.GetCommitableEntities(subMeterEntities);

                //Get data from [Temp.CustomerDataUpload].[SubMeterUsage] where CanCommit = 1
                var subMeterUsageEntities =  new Methods.TempSchema.CustomerDataUpload.SubMeterUsage().SubMeterUsage_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSubMeterUsageEntities = tempCustomerDataUploadMethods.GetCommitableEntities(subMeterUsageEntities);

                if(!commitableSubMeterEntities.Any() && !commitableSubMeterUsageEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
                    return;
                }

                var customerMethods = new Methods.CustomerSchema();
                var systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();

                //Get list of subMeterIdentifiers from datasets where SubMeter Id is not 0
                var subMeterIdentifierSubMeterAttributeId = customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(new Enums.CustomerSchema.SubMeter.Attribute().SubMeterIdentifier);
                var subMeterIdentifierList = commitableSubMeterEntities.Select(smue => smue.SubMeterIdentifier)
                    .Union(commitableSubMeterUsageEntities.Select(smue => smue.SubMeterIdentifier)).Distinct()
                    .Where(m => customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, m) > 0);

                if (!subMeterIdentifierList.Any())
                {
                    //Nothing to work so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
                    return;
                }

                var informationGranularityAttributeEnums = new Enums.InformationSchema.Granularity.Attribute();

                //Get Routing.API URL
                var routingAPIId = systemAPIMethods.GetRoutingAPIId();

                //Add subMeter type to jsonObject
                jsonObject.Add(systemAPIRequiredDataKeyEnums.MeterType, "SubMeter");

                //Update Process GUID to CommitPeriodicUsage Process GUID
                systemMethods.SetProcessGUIDInJObject(jsonObject, new Enums.SystemSchema.Process.GUID().CommitPeriodicUsage);

                //Add granularity to newJsonObject
                var granularityDefaultGranularityAttributeId = informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(informationGranularityAttributeEnums.IsElectricityDefault);
                var granularityId = informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeId(granularityDefaultGranularityAttributeId);
                var granularityDescriptionGranularityAttributeId = informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(informationGranularityAttributeEnums.GranularityDescription);
                var granularityDescription = informationMethods.GranularityDetail_GetGranularityDetailDescriptionByGranularityIdAndGranularityAttributeId(granularityId, granularityDescriptionGranularityAttributeId);
                jsonObject.Add(systemAPIRequiredDataKeyEnums.Granularity, granularityDescription);

                //Add usage type to newJsonObject
                jsonObject.Add(systemAPIRequiredDataKeyEnums.UsageType, new Enums.InformationSchema.UsageType().CustomerEstimated);

                foreach(var subMeterIdentifier in subMeterIdentifierList)
                {
                    //Clone jsonObject
                    var newJsonObject = (JObject)jsonObject.DeepClone();

                    //Create new ProcessQueueGUID
                    var newProcessQueueGUID = Guid.NewGuid().ToString();

                    //Map current ProcessQueueGUID to new ProcessQueueGUID
                    systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                    systemMethods.SetProcessQueueGUIDInJObject(newJsonObject, newProcessQueueGUID);

                    //Add subMeterIdentifier to newJsonObject
                    newJsonObject.Add(systemAPIRequiredDataKeyEnums.SubMeterIdentifier, subMeterIdentifier);

                    //Get periodic usage
                    var periodicUsageEntities = commitableSubMeterUsageEntities.Where(smue => smue.SubMeterIdentifier == subMeterIdentifier).ToList();

                    //Convert to dictionary
                    var periodicUsageDictionary = new Dictionary<string, Dictionary<string, string>>();
                    foreach(var periodicUsageEntity in periodicUsageEntities)
                    {
                        if(!periodicUsageDictionary.ContainsKey(periodicUsageEntity.Date))
                        {
                            periodicUsageDictionary.Add(periodicUsageEntity.Date, new Dictionary<string, string>());
                        }
                        
                        var date = periodicUsageDictionary[periodicUsageEntity.Date];

                        if(!date.ContainsKey(periodicUsageEntity.TimePeriod))
                        {
                            date.Add(periodicUsageEntity.TimePeriod, periodicUsageEntity.Value);
                        }
                    }

                    //Add periodic usage to newJsonObject
                    newJsonObject.Add(systemAPIRequiredDataKeyEnums.PeriodicUsage, JsonConvert.SerializeObject(periodicUsageDictionary));

                    //Connect to Routing API and POST data
                    systemAPIMethods.PostAsJsonAsync(routingAPIId, systemAPIGUIDEnums.CommitSubMeterUsageDataAPI, hostEnvironment, newJsonObject);
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
