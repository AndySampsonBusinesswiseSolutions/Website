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

namespace CommitSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitSubMeterUsageDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitSubMeterUsageDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.SystemSchema.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
        private readonly Enums.SystemSchema.Process.GUID _systemProcessGUIDEnums = new Enums.SystemSchema.Process.GUID();
        private readonly Enums.InformationSchema.UsageType _informationUsageTypeEnums = new Enums.InformationSchema.UsageType();
        private readonly Enums.InformationSchema.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.InformationSchema.Granularity.Attribute();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitSubMeterUsageDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitSubMeterUsageDataController(ILogger<CommitSubMeterUsageDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitSubMeterUsageDataAPI, password);
            commitSubMeterUsageDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("CommitSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitSubMeterUsageDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitSubMeterUsageData/Commit")]
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
                    commitSubMeterUsageDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitSubMeterUsageDataAPI, commitSubMeterUsageDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitSubMeterUsageDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterEntities = new Methods.Temp.CustomerDataUpload.SubMeter().SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSubMeterEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(subMeterEntities);

                //Get data from [Temp.CustomerDataUpload].[SubMeterUsage] where CanCommit = 1
                var subMeterUsageEntities =  new Methods.Temp.CustomerDataUpload.SubMeterUsage().SubMeterUsage_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSubMeterUsageEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(subMeterUsageEntities);

                if(!commitableSubMeterEntities.Any() && !commitableSubMeterUsageEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
                    return;
                }

                var customerMethods = new Methods.Customer();

                //Get list of subMeterIdentifiers from datasets where SubMeter Id is not 0
                var subMeterIdentifierSubMeterAttributeId = customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(new Enums.CustomerSchema.SubMeter.Attribute().SubMeterIdentifier);
                var subMeterIdentifierList = commitableSubMeterEntities.Select(smue => smue.SubMeterIdentifier)
                    .Union(commitableSubMeterUsageEntities.Select(smue => smue.SubMeterIdentifier)).Distinct()
                    .Where(m => customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, m) > 0);

                if (!subMeterIdentifierList.Any())
                {
                    //Nothing to work so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
                    return;
                }

                //Get Routing.API URL
                var routingAPIId = _systemAPIMethods.GetRoutingAPIId();

                //Add subMeter type to jsonObject
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.MeterType, "SubMeter");

                //Update Process GUID to CommitPeriodicUsage Process GUID
                _systemMethods.SetProcessGUIDInJObject(jsonObject, _systemProcessGUIDEnums.CommitPeriodicUsage);

                //Add granularity to newJsonObject
                var granularityDefaultGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.IsElectricityDefault);
                var granularityId = _informationMethods.GranularityDetail_GetGranularityIdByGranularityAttributeId(granularityDefaultGranularityAttributeId);
                var granularityDescriptionGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityDescription);
                var granularityDescription = _informationMethods.GranularityDetail_GetGranularityDetailDescriptionByGranularityIdAndGranularityAttributeId(granularityId, granularityDescriptionGranularityAttributeId);
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.Granularity, granularityDescription);

                //Add usage type to newJsonObject
                jsonObject.Add(_systemAPIRequiredDataKeyEnums.UsageType, _informationUsageTypeEnums.CustomerEstimated);

                foreach(var subMeterIdentifier in subMeterIdentifierList)
                {
                    //Clone jsonObject
                    var newJsonObject = (JObject)jsonObject.DeepClone();

                    //Create new ProcessQueueGUID
                    var newProcessQueueGUID = Guid.NewGuid().ToString();

                    //Map current ProcessQueueGUID to new ProcessQueueGUID
                    _systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                    _systemMethods.SetProcessQueueGUIDInJObject(newJsonObject, newProcessQueueGUID);

                    //Add subMeterIdentifier to newJsonObject
                    newJsonObject.Add(_systemAPIRequiredDataKeyEnums.SubMeterIdentifier, subMeterIdentifier);

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
                    newJsonObject.Add(_systemAPIRequiredDataKeyEnums.PeriodicUsage, JsonConvert.SerializeObject(periodicUsageDictionary));

                    //Connect to Routing API and POST data
                    _systemAPIMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.CommitSubMeterUsageDataAPI, hostEnvironment, newJsonObject);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}