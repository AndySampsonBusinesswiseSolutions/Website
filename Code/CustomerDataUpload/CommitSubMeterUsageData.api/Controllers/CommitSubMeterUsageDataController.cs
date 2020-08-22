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

                //Get data from [Temp.CustomerDataUpload].[SubMeterUsage] where CanCommit = 1
                var subMeterDataRows = _tempCustomerDataUploadMethods.SubMeter_GetByProcessQueueGUID(processQueueGUID);
                var subMeterCommitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(subMeterDataRows);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterUsageDataRows = _tempCustomerDataUploadMethods.SubMeterUsage_GetByProcessQueueGUID(processQueueGUID);
                var subMeterUsageCommitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(subMeterUsageDataRows);

                if(!subMeterCommitableDataRows.Any() && !subMeterUsageCommitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
                    return;
                }

                //Get list of subMeterIdentifiers from datasets
                var subMeterIdentifierList = subMeterCommitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier))
                    .Union(subMeterUsageCommitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier)))
                    .Distinct();

                if(!subMeterIdentifierList.Any())
                {
                    //Nothing to work so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
                    return;
                }

                //Get Routing.API URL
                var routingAPIId = _systemMethods.GetRoutingAPIId();

                foreach(var subMeterIdentifier in subMeterIdentifierList)
                {
                    //Create new ProcessQueueGUID
                    var newProcessQueueGUID = Guid.NewGuid().ToString();

                    //Map current ProcessQueueGUID to new ProcessQueueGUID
                    _systemMethods.ProcessQueueProgression_Insert(createdByUserId, sourceId, processQueueGUID, newProcessQueueGUID);
                    _systemMethods.SetProcessQueueGUIDInJObject(jsonObject, newProcessQueueGUID);

                    //Add subMeter type to jsonObject
                    jsonObject.Add(_systemAPIRequiredDataKeyEnums.MeterType, "SubMeter");

                    //Add subMeterIdentifier to jsonObject
                    jsonObject.Add(_systemAPIRequiredDataKeyEnums.MPXN, subMeterIdentifier);

                    if(subMeterUsageCommitableDataRows.Any(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN) == subMeterIdentifier))
                    {
                        //Update Process GUID to CommitPeriodicUsage Process GUID
                        _systemMethods.SetProcessGUIDInJObject(jsonObject, _systemProcessGUIDEnums.CommitPeriodicUsage);

                        //Get periodic usage
                        var periodicUsageDataRows = subMeterUsageCommitableDataRows.Where(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN) == subMeterIdentifier);

                        //Add periodic usage to jsonObject
                        jsonObject.Add(_systemAPIRequiredDataKeyEnums.PeriodicUsage, JsonConvert.SerializeObject(periodicUsageDataRows));
                    }
                    else 
                    {
                        //Update Process GUID to CommitEstimatedAnnualUsage Process GUID
                        _systemMethods.SetProcessGUIDInJObject(jsonObject, _systemProcessGUIDEnums.CommitEstimatedAnnualUsage);

                        //Get EstimatedAnnualUsage
                        var estimatedAnnualUsage = subMeterUsageCommitableDataRows.First(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN) == subMeterIdentifier)[_customerDataUploadValidationEntityEnums.AnnualUsage].ToString();

                        //Add EstimatedAnnualUsage to jsonObject
                        jsonObject.Add(_systemAPIRequiredDataKeyEnums.EstimatedAnnualUsage, estimatedAnnualUsage);
                    }

                    //Connect to Routing API and POST data
                    _systemMethods.PostAsJsonAsync(routingAPIId, _systemAPIGUIDEnums.CommitSubMeterUsageDataAPI, jsonObject);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitSubMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitSubMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

