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
        private readonly Enums.Information.Granularity.Description _informationGranularityDescriptionEnums = new Enums.Information.Granularity.Description();
        private readonly Enums.Information.UsageType _informationUsageTypeEnums = new Enums.Information.UsageType();
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

                //Get data from [Temp.CustomerDataUpload].[SubMeterUsage] where CanCommit = 1
                var subMeterDataRows = _tempCustomerDataUploadMethods.SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var subMeterCommitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(subMeterDataRows);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterUsageDataRows = _tempCustomerDataUploadMethods.SubMeterUsage_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var subMeterUsageCommitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(subMeterUsageDataRows);

                if(!subMeterCommitableDataRows.Any() && !subMeterUsageCommitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
                    return;
                }

                //Get list of subMeterIdentifiers from datasets
                var subMeterIdentifierList = subMeterCommitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier))
                    .Union(subMeterUsageCommitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier)))
                    .Distinct();

                if(!subMeterIdentifierList.Any())
                {
                    //Nothing to work so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
                    return;
                }

                //Get Routing.API URL
                var routingAPIId = _systemMethods.GetRoutingAPIId();

                foreach(var subMeterIdentifier in subMeterIdentifierList)
                {
                    //Clone jsonObject
                    var newJsonObject = (JObject)jsonObject.DeepClone();

                    //Create new ProcessQueueGUID
                    var newProcessQueueGUID = Guid.NewGuid().ToString();

                    //Map current ProcessQueueGUID to new ProcessQueueGUID
                    _systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                    _systemMethods.SetProcessQueueGUIDInJObject(newJsonObject, newProcessQueueGUID);

                    //Add subMeter type to newJsonObject
                    newJsonObject.Add(_systemAPIRequiredDataKeyEnums.MeterType, "SubMeter");

                    //Add subMeterIdentifier to newJsonObject
                    newJsonObject.Add(_systemAPIRequiredDataKeyEnums.MPXN, subMeterIdentifier);

                    if(subMeterUsageCommitableDataRows.Any(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier) == subMeterIdentifier))
                    {
                        //Update Process GUID to CommitPeriodicUsage Process GUID
                        _systemMethods.SetProcessGUIDInJObject(newJsonObject, _systemProcessGUIDEnums.CommitPeriodicUsage);

                        //Get periodic usage
                        var periodicUsageDataRows = subMeterUsageCommitableDataRows.Where(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier) == subMeterIdentifier);

                        //Convert to dictionary
                        var periodicUsageDictionary = new Dictionary<string, Dictionary<string, string>>();
                        foreach(var periodicUsageDate in periodicUsageDataRows)
                        {
                            if(!periodicUsageDictionary.ContainsKey(periodicUsageDate.Field<string>("Date")))
                            {
                                periodicUsageDictionary.Add(periodicUsageDate.Field<string>("Date"), new Dictionary<string, string>());
                            }
                            
                            var date = periodicUsageDictionary[periodicUsageDate.Field<string>("Date")];

                            if(!date.ContainsKey(periodicUsageDate.Field<string>("TimePeriod")))
                            {
                                date.Add(periodicUsageDate.Field<string>("TimePeriod"), periodicUsageDate.Field<string>("Value"));
                            }
                        }

                        //Add granularity to newJsonObject
                        newJsonObject.Add(_systemAPIRequiredDataKeyEnums.Granularity, _informationGranularityDescriptionEnums.HalfHour);

                        //Add usage type to newJsonObject
                        newJsonObject.Add(_systemAPIRequiredDataKeyEnums.UsageType, _informationUsageTypeEnums.CustomerEstimated);

                        //Add periodic usage to newJsonObject
                        newJsonObject.Add(_systemAPIRequiredDataKeyEnums.PeriodicUsage, JsonConvert.SerializeObject(periodicUsageDictionary));
                    }
                    else 
                    {
                        //Update Process GUID to CommitEstimatedAnnualUsage Process GUID
                        _systemMethods.SetProcessGUIDInJObject(newJsonObject, _systemProcessGUIDEnums.CommitEstimatedAnnualUsage);

                        //Get EstimatedAnnualUsage
                        var estimatedAnnualUsage = subMeterUsageCommitableDataRows.First(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier) == subMeterIdentifier)[_customerDataUploadValidationEntityEnums.AnnualUsage].ToString();

                        //Add EstimatedAnnualUsage to newJsonObject
                        newJsonObject.Add(_systemAPIRequiredDataKeyEnums.EstimatedAnnualUsage, estimatedAnnualUsage);
                    }

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

