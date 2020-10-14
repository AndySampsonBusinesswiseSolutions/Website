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

namespace CommitSubMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitSubMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<CommitSubMeterUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.System.Process.GUID _systemProcessGUIDEnums = new Enums.System.Process.GUID();
        private readonly Enums.Information.UsageType _informationUsageTypeEnums = new Enums.Information.UsageType();
        private readonly Enums.Information.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitSubMeterUsageDataAPIId;

        public CommitSubMeterUsageDataController(ILogger<CommitSubMeterUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitSubMeterUsageDataAPI, _systemAPIPasswordEnums.CommitSubMeterUsageDataAPI);
            commitSubMeterUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitSubMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("CommitSubMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitSubMeterUsageDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitSubMeterUsageData/Commit")]
        public void Commit([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
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

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitSubMeterUsageDataAPI, commitSubMeterUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitSubMeterUsageDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterUsageDataRows = _tempCustomerDataUploadMethods.SubMeterUsage_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var subMeterUsageCommitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(subMeterUsageDataRows);

                if(!subMeterUsageCommitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
                    return;
                }

                //Get list of subMeterIdentifiers from datasets
                var subMeterIdentifierList = subMeterUsageCommitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier))
                    .Distinct();

                //Get Routing.API URL
                var routingAPIId = _systemMethods.GetRoutingAPIId();

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
                    var periodicUsageDataRows = subMeterUsageCommitableDataRows.Where(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier) == subMeterIdentifier);

                    //Convert to Tuple
                    var periodicUsageTupleList = new List<Tuple<string, string, string>>();

                    foreach (DataRow r in periodicUsageDataRows)
                    {
                        var tup = Tuple.Create((string)r["Date"], (string)r["TimePeriod"], (string)r["Value"]);
                        periodicUsageTupleList.Add(tup);
                    }

                    //Convert to dictionary
                    var periodicUsageDictionary = new Dictionary<string, Dictionary<string, string>>();
                    foreach(var periodicUsageTuple in periodicUsageTupleList)
                    {
                        if(!periodicUsageDictionary.ContainsKey(periodicUsageTuple.Item1))
                        {
                            periodicUsageDictionary.Add(periodicUsageTuple.Item1, new Dictionary<string, string>());
                        }
                        
                        var date = periodicUsageDictionary[periodicUsageTuple.Item1];

                        if(!date.ContainsKey(periodicUsageTuple.Item2))
                        {
                            date.Add(periodicUsageTuple.Item2, periodicUsageTuple.Item3);
                        }
                    }

                    //Add periodic usage to newJsonObject
                    newJsonObject.Add(_systemAPIRequiredDataKeyEnums.PeriodicUsage, JsonConvert.SerializeObject(periodicUsageDictionary));

                    //Connect to Routing API and POST data
                    _systemMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.CommitSubMeterUsageDataAPI, newJsonObject);
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