using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
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
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.InformationSchema.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.InformationSchema.Granularity.Attribute();
        private readonly Enums.CustomerSchema.Meter.Attribute _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
        private readonly Enums.SystemSchema.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
        private readonly Enums.SystemSchema.Process.GUID _systemProcessGUIDEnums = new Enums.SystemSchema.Process.GUID();
        private readonly Enums.InformationSchema.UsageType _informationUsageTypeEnums = new Enums.InformationSchema.UsageType();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterUsageDataController(ILogger<CommitMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterUsageDataAPI, password);
            commitMeterUsageDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterUsageData/Commit")]
        public void Commit([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            var customerDataUploadProcessQueueGUID = _systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    commitMeterUsageDataAPIId);

                if (!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitMeterUsageDataAPI, commitMeterUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterUsageDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(meterEntities);

                //Get data from [Temp.CustomerDataUpload].[MeterUsage] where CanCommit = 1
                var meterUsageEntities = new Methods.Temp.CustomerDataUpload.MeterUsage().MeterUsage_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterUsageEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(meterUsageEntities);

                if (!commitableMeterEntities.Any() && !commitableMeterUsageEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterUsageDataAPIId, false, null);
                    return;
                }

                var customerMethods = new Methods.Customer();

                //Get list of mpxns from datasets where Meter Id is not 0
                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var mpxnList = commitableMeterEntities.Select(cme => cme.MPXN)
                    .Union(commitableMeterUsageEntities.Select(cmue => cmue.MPXN)).Distinct()
                    .Where(m => customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m) > 0);

                if (!mpxnList.Any())
                {
                    //Nothing to work so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterUsageDataAPIId, false, null);
                    return;
                }

                var methods = new Methods();

                //Get Routing.API URL
                var routingAPIId = _systemAPIMethods.GetRoutingAPIId();
                var commitEstimatedAnnualUsageAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitEstimatedAnnualUsageAPI);

                //Update Process GUID to CommitEstimatedAnnualUsage Process GUID
                _systemMethods.SetProcessGUIDInJObject(jsonObject, _systemProcessGUIDEnums.CommitEstimatedAnnualUsage);

                //Add meter type to newJsonObject
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.MeterType, "Meter");

                //Add granularity to newJsonObject
                var granularityDictionary = new Dictionary<bool, string>
                {
                    {true, _informationMethods.GetDefaultGranularityDescriptionByCommodity(_informationGranularityAttributeEnums.IsElectricityDefault)},
                    {false, _informationMethods.GetDefaultGranularityDescriptionByCommodity(_informationGranularityAttributeEnums.IsGasDefault)}
                };

                foreach (var mpxn in mpxnList)
                {
                    //Clone jsonObject
                    var newJsonObject = (JObject)jsonObject.DeepClone();

                    //Create new ProcessQueueGUID
                    var newProcessQueueGUID = Guid.NewGuid().ToString();

                    //Map current ProcessQueueGUID to new ProcessQueueGUID
                    _systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                    _systemMethods.SetProcessQueueGUIDInJObject(newJsonObject, newProcessQueueGUID);

                    //Update Process GUID to CommitPeriodicUsage Process GUID
                    _systemMethods.SetProcessGUIDInJObject(newJsonObject, _systemProcessGUIDEnums.CommitEstimatedAnnualUsage);

                    //Add mpxn to newJsonObject
                    newJsonObject.Add(_systemAPIRequiredDataKeyEnums.MPXN, mpxn);

                    //Get EstimatedAnnualUsage
                    var estimatedAnnualUsage = commitableMeterEntities.First(cme => cme.MPXN == mpxn).AnnualUsage;

                    //Add EstimatedAnnualUsage to newJsonObject
                    newJsonObject.Add(_systemAPIRequiredDataKeyEnums.EstimatedAnnualUsage, estimatedAnnualUsage);

                    var hasPeriodicUsage = commitableMeterUsageEntities.Any(cmue => cmue.MPXN == mpxn);

                    //Add HasPeriodicUsage to newJsonObject
                    newJsonObject.Add(_systemAPIRequiredDataKeyEnums.HasPeriodicUsage, hasPeriodicUsage);

                    //Call CommitEstimatedAnnualUsage API and wait for response
                    var commitEstimateUsageAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitEstimatedAnnualUsageAPI);
                    var commitEstimateUsageAPI = _systemAPIMethods.PostAsJsonAsync(commitEstimateUsageAPIId, _systemAPIGUIDEnums.CommitMeterUsageDataAPI, hostEnvironment, newJsonObject);
                    var commitEstimateUsageResult = commitEstimateUsageAPI.GetAwaiter().GetResult().Content.ReadAsStringAsync();

                    if (hasPeriodicUsage)
                    {
                        //Create new ProcessQueueGUID
                        newProcessQueueGUID = Guid.NewGuid().ToString();

                        //Map current ProcessQueueGUID to new ProcessQueueGUID
                        _systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                        _systemMethods.SetProcessQueueGUIDInJObject(newJsonObject, newProcessQueueGUID);

                        //Update Process GUID to CommitPeriodicUsage Process GUID
                        _systemMethods.SetProcessGUIDInJObject(newJsonObject, _systemProcessGUIDEnums.CommitPeriodicUsage);

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
                        newJsonObject.Add(_systemAPIRequiredDataKeyEnums.UsageType, _informationUsageTypeEnums.CustomerEstimated);

                        //Add granularity description to newJsonObject
                        newJsonObject.Add(_systemAPIRequiredDataKeyEnums.Granularity, granularityDictionary[methods.IsValidMPAN(mpxn)]);

                        //Add periodic usage to newJsonObject
                        newJsonObject.Add(_systemAPIRequiredDataKeyEnums.PeriodicUsage, JsonConvert.SerializeObject(periodicUsageDictionary));

                        //Connect to Routing API and POST data
                        _systemAPIMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.CommitMeterUsageDataAPI, hostEnvironment, newJsonObject);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterUsageDataAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}