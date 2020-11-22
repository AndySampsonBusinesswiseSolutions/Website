using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CommitMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterUsageDataController> _logger;
        private readonly Int64 commitMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterUsageDataController(ILogger<CommitMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterUsageDataAPI, password);
            commitMeterUsageDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.SystemSchema.API().PostAsJsonAsync(commitMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterUsageData/Commit")]
        public void Commit([FromBody] object data)
        {
            var systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
            var informationMethods = new Methods.InformationSchema();
            var systemAPIMethods = new Methods.SystemSchema.API();
            var systemMethods = new Methods.SystemSchema();

            //Get base variables
            var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    commitMeterUsageDataAPIId);

                if (!systemAPIMethods.PrerequisiteAPIsAreSuccessful(systemAPIGUIDEnums.CommitMeterUsageDataAPI, commitMeterUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterUsageDataAPIId);

                var tempCustomerDataUploadMethods = new Methods.TempSchema.CustomerDataUpload();

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.TempSchema.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = tempCustomerDataUploadMethods.GetCommitableEntities(meterEntities);

                //Get data from [Temp.CustomerDataUpload].[MeterUsage] where CanCommit = 1
                var meterUsageEntities = new Methods.TempSchema.CustomerDataUpload.MeterUsage().MeterUsage_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterUsageEntities = tempCustomerDataUploadMethods.GetCommitableEntities(meterUsageEntities);

                if (!commitableMeterEntities.Any() && !commitableMeterUsageEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterUsageDataAPIId, false, null);
                    return;
                }

                var customerMethods = new Methods.CustomerSchema();

                //Get list of mpxns from datasets where Meter Id is not 0
                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var mpxnList = commitableMeterEntities.Select(cme => cme.MPXN)
                    .Union(commitableMeterUsageEntities.Select(cmue => cmue.MPXN)).Distinct()
                    .Where(m => customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m) > 0);

                if (!mpxnList.Any())
                {
                    //Nothing to work so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterUsageDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();
                var informationUsageTypeEnums = new Enums.InformationSchema.UsageType();
                var systemProcessGUIDEnums = new Enums.SystemSchema.Process.GUID();
                var systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
                var informationGranularityAttributeEnums = new Enums.InformationSchema.Granularity.Attribute();

                //Get Routing.API URL
                var routingAPIId = systemAPIMethods.GetRoutingAPIId();
                var commitEstimatedAnnualUsageAPIId = systemAPIMethods.API_GetAPIIdByAPIGUID(systemAPIGUIDEnums.CommitEstimatedAnnualUsageAPI);

                //Update Process GUID to CommitEstimatedAnnualUsage Process GUID
                systemMethods.SetProcessGUIDInJObject(jsonObject, systemProcessGUIDEnums.CommitEstimatedAnnualUsage);

                //Add meter type to newJsonObject
                jsonObject.Add(systemAPIRequiredDataKeyEnums.MeterType, "Meter");

                //Add granularity to newJsonObject
                var granularityDictionary = new Dictionary<bool, string>
                {
                    {true, informationMethods.GetDefaultGranularityDescriptionByCommodity(informationGranularityAttributeEnums.IsElectricityDefault)},
                    {false, informationMethods.GetDefaultGranularityDescriptionByCommodity(informationGranularityAttributeEnums.IsGasDefault)}
                };

                foreach (var mpxn in mpxnList)
                {
                    //Clone jsonObject
                    var newJsonObject = (JObject)jsonObject.DeepClone();

                    //Create new ProcessQueueGUID
                    var newProcessQueueGUID = Guid.NewGuid().ToString();

                    //Map current ProcessQueueGUID to new ProcessQueueGUID
                    systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                    systemMethods.SetProcessQueueGUIDInJObject(newJsonObject, newProcessQueueGUID);

                    //Update Process GUID to CommitPeriodicUsage Process GUID
                    systemMethods.SetProcessGUIDInJObject(newJsonObject, systemProcessGUIDEnums.CommitEstimatedAnnualUsage);

                    //Add mpxn to newJsonObject
                    newJsonObject.Add(systemAPIRequiredDataKeyEnums.MPXN, mpxn);

                    //Get EstimatedAnnualUsage
                    var estimatedAnnualUsage = commitableMeterEntities.First(cme => cme.MPXN == mpxn).AnnualUsage;

                    //Add EstimatedAnnualUsage to newJsonObject
                    newJsonObject.Add(systemAPIRequiredDataKeyEnums.EstimatedAnnualUsage, estimatedAnnualUsage);

                    var hasPeriodicUsage = commitableMeterUsageEntities.Any(cmue => cmue.MPXN == mpxn);

                    //Add HasPeriodicUsage to newJsonObject
                    newJsonObject.Add(systemAPIRequiredDataKeyEnums.HasPeriodicUsage, hasPeriodicUsage);

                    //Call CommitEstimatedAnnualUsage API and wait for response
                    var commitEstimateUsageAPIId = systemAPIMethods.API_GetAPIIdByAPIGUID(systemAPIGUIDEnums.CommitEstimatedAnnualUsageAPI);
                    var commitEstimateUsageAPI = systemAPIMethods.PostAsJsonAsync(commitEstimateUsageAPIId, systemAPIGUIDEnums.CommitMeterUsageDataAPI, hostEnvironment, newJsonObject);
                    var commitEstimateUsageResult = commitEstimateUsageAPI.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                    if (hasPeriodicUsage)
                    {
                        //Create new ProcessQueueGUID
                        newProcessQueueGUID = Guid.NewGuid().ToString();

                        //Map current ProcessQueueGUID to new ProcessQueueGUID
                        systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                        systemMethods.SetProcessQueueGUIDInJObject(newJsonObject, newProcessQueueGUID);

                        //Update Process GUID to CommitPeriodicUsage Process GUID
                        systemMethods.SetProcessGUIDInJObject(newJsonObject, systemProcessGUIDEnums.CommitPeriodicUsage);

                        //Get periodic usage
                        var periodicUsageEntities = commitableMeterUsageEntities.Where(cmue => cmue.MPXN == mpxn);

                        //Convert to dictionary
                        var periodicUsageDictionary = new Dictionary<string, Dictionary<string, string>>();
                        foreach (var periodicUsage in periodicUsageEntities)
                        {
                            if (!periodicUsageDictionary.ContainsKey(periodicUsage.Date))
                            {
                                periodicUsageDictionary.Add(periodicUsage.Date, new Dictionary<string, string>());
                            }

                            var date = periodicUsageDictionary[periodicUsage.Date];

                            if (!date.ContainsKey(periodicUsage.TimePeriod))
                            {
                                date.Add(periodicUsage.TimePeriod, periodicUsage.Value);
                            }
                        }

                        //Add usage type to newJsonObject
                        newJsonObject.Add(systemAPIRequiredDataKeyEnums.UsageType, informationUsageTypeEnums.CustomerEstimated);

                        //Add granularity description to newJsonObject
                        newJsonObject.Add(systemAPIRequiredDataKeyEnums.Granularity, granularityDictionary[methods.IsValidMPAN(mpxn)]);

                        //Add periodic usage to newJsonObject
                        newJsonObject.Add(systemAPIRequiredDataKeyEnums.PeriodicUsage, JsonConvert.SerializeObject(periodicUsageDictionary));

                        //Connect to Routing API and POST data
                        systemAPIMethods.PostAsJsonAsync(routingAPIId, systemAPIGUIDEnums.CommitMeterUsageDataAPI, hostEnvironment, newJsonObject);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterUsageDataAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}