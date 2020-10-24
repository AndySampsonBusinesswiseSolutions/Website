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

namespace CommitMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<CommitMeterUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Information.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.System.Process.GUID _systemProcessGUIDEnums = new Enums.System.Process.GUID();
        private readonly Enums.Information.UsageType _informationUsageTypeEnums = new Enums.Information.UsageType();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitMeterUsageDataAPIId;

        public CommitMeterUsageDataController(ILogger<CommitMeterUsageDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitMeterUsageDataAPI, _systemAPIPasswordEnums.CommitMeterUsageDataAPI);
            commitMeterUsageDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitMeterUsageDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterUsageData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitMeterUsageDataAPIId, JObject.Parse(data.ToString()));

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

                if (!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitMeterUsageDataAPI, commitMeterUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterUsageDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterDataRows = _tempCustomerDataUploadMethods.Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var meterCommitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(meterDataRows);

                //Get data from [Temp.CustomerDataUpload].[MeterUsage] where CanCommit = 1
                var meterUsageDataRows = _tempCustomerDataUploadMethods.MeterUsage_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var meterUsageCommitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(meterUsageDataRows);

                if (!meterCommitableDataRows.Any() && !meterUsageCommitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterUsageDataAPIId, false, null);
                    return;
                }

                //Get list of mpxns from datasets
                var mpxnList = meterCommitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN))
                    .Union(meterUsageCommitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)))
                    .Distinct();

                if (!mpxnList.Any())
                {
                    //Nothing to work so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterUsageDataAPIId, false, null);
                    return;
                }

                //Get Routing.API URL
                var routingAPIId = _systemMethods.GetRoutingAPIId();
                var commitEstimatedAnnualUsageAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitEstimatedAnnualUsageAPI);

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
                    var estimatedAnnualUsage = meterCommitableDataRows.First(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN) == mpxn)[_customerDataUploadValidationEntityEnums.AnnualUsage].ToString();

                    //Add EstimatedAnnualUsage to newJsonObject
                    newJsonObject.Add(_systemAPIRequiredDataKeyEnums.EstimatedAnnualUsage, estimatedAnnualUsage);

                    var hasPeriodicUsage = meterUsageCommitableDataRows.Any(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN) == mpxn);

                    //Add HasPeriodicUsage to newJsonObject
                    newJsonObject.Add(_systemAPIRequiredDataKeyEnums.HasPeriodicUsage, hasPeriodicUsage);

                    //Connect to Routing API and POST data
                    _systemMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.CommitMeterUsageDataAPI, newJsonObject);

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
                        var periodicUsageDataRows = meterUsageCommitableDataRows.Where(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN) == mpxn);

                        //Convert to Tuple
                        var periodicUsageTupleList = new List<Tuple<string, string, string>>();

                        foreach (DataRow r in periodicUsageDataRows)
                        {
                            var tup = Tuple.Create((string)r["Date"], (string)r["TimePeriod"], (string)r["Value"]);
                            periodicUsageTupleList.Add(tup);
                        }

                        //Convert to dictionary
                        var periodicUsageDictionary = new Dictionary<string, Dictionary<string, string>>();
                        foreach (var periodicUsageTuple in periodicUsageTupleList)
                        {
                            if (!periodicUsageDictionary.ContainsKey(periodicUsageTuple.Item1))
                            {
                                periodicUsageDictionary.Add(periodicUsageTuple.Item1, new Dictionary<string, string>());
                            }

                            var date = periodicUsageDictionary[periodicUsageTuple.Item1];

                            if (!date.ContainsKey(periodicUsageTuple.Item2))
                            {
                                date.Add(periodicUsageTuple.Item2, periodicUsageTuple.Item3);
                            }
                        }

                        //Add usage type to newJsonObject
                        newJsonObject.Add(_systemAPIRequiredDataKeyEnums.UsageType, _informationUsageTypeEnums.CustomerEstimated);

                        //Add granularity description to newJsonObject
                        newJsonObject.Add(_systemAPIRequiredDataKeyEnums.Granularity, granularityDictionary[_methods.IsValidMPAN(mpxn)]);

                        //Add periodic usage to newJsonObject
                        newJsonObject.Add(_systemAPIRequiredDataKeyEnums.PeriodicUsage, JsonConvert.SerializeObject(periodicUsageDictionary));

                        //Connect to Routing API and POST data
                        _systemMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.CommitMeterUsageDataAPI, newJsonObject);
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