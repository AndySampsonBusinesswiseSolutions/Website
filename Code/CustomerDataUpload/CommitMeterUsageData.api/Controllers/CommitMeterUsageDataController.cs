﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using Newtonsoft.Json;

namespace CommitMeterUsageData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterUsageDataController : ControllerBase
    {
        private readonly ILogger<CommitMeterUsageDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
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
                    commitMeterUsageDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitMeterUsageDataAPI, commitMeterUsageDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[MeterUsage] where CanCommit = 1
                var meterDataRows = _tempCustomerMethods.Meter_GetByProcessQueueGUID(processQueueGUID);
                var meterCommitableDataRows = _tempCustomerMethods.GetCommitableRows(meterDataRows);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterUsageDataRows = _tempCustomerMethods.MeterUsage_GetByProcessQueueGUID(processQueueGUID);
                var meterUsageCommitableDataRows = _tempCustomerMethods.GetCommitableRows(meterUsageDataRows);

                if(!meterCommitableDataRows.Any() && !meterUsageCommitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterUsageDataAPIId, false, null);
                    return;
                }

                //Get list of mpxns from datasets
                var mpxnList = meterCommitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN))
                    .Union(meterUsageCommitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)))
                    .Distinct();

                if(!mpxnList.Any())
                {
                    //Nothing to work so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterUsageDataAPIId, false, null);
                    return;
                }

                foreach(var mpxn in mpxnList)
                {
                    //Add meter type to jsonObject
                    jsonObject.Add(_systemAPIRequiredDataKeyEnums.MeterType, "Meter");

                    //Add mpxn to jsonObject
                    jsonObject.Add(_systemAPIRequiredDataKeyEnums.MPXN, mpxn);

                    if(meterUsageCommitableDataRows.Any(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN) == mpxn))
                    {
                        //Get periodic usage
                        var periodicUsageDataRows = meterUsageCommitableDataRows.Where(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN) == mpxn);

                        //Add periodic usage to jsonObject
                        jsonObject.Add(_systemAPIRequiredDataKeyEnums.PeriodicUsage, JsonConvert.SerializeObject(periodicUsageDataRows));

                        //Launch CommitPeriodicUsageData API
                    }
                    else 
                    {
                        //Get EstimatedAnnualUsage
                        var estimatedAnnualUsage = meterUsageCommitableDataRows.First(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN) == mpxn)[_customerDataUploadValidationEntityEnums.AnnualUsage].ToString();

                        //Add EstimatedAnnualUsage to jsonObject
                        jsonObject.Add(_systemAPIRequiredDataKeyEnums.EstimatedAnnualUsage, estimatedAnnualUsage);

                        //Launch CommitEstimatedAnnualUsageData API
                    }
                }
                
                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterUsageDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterUsageDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

