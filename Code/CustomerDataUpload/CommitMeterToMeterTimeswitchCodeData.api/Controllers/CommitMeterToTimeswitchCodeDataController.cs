﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace CommitMeterToMeterTimeswitchCodeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToMeterTimeswitchCodeDataController : ControllerBase
    {
        private readonly ILogger<CommitMeterToMeterTimeswitchCodeDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Information.MeterTimeswitchCode.Attribute _informationMeterTimeswitchCodeAttributeEnums = new Enums.Information.MeterTimeswitchCode.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitMeterToMeterTimeswitchCodeDataAPIId;

        public CommitMeterToMeterTimeswitchCodeDataController(ILogger<CommitMeterToMeterTimeswitchCodeDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitMeterToMeterTimeswitchCodeDataAPI, _systemAPIPasswordEnums.CommitMeterToMeterTimeswitchCodeDataAPI);
            commitMeterToMeterTimeswitchCodeDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitMeterToMeterTimeswitchCodeDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToMeterTimeswitchCodeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitMeterToMeterTimeswitchCodeDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToMeterTimeswitchCodeData/Commit")]
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
                    commitMeterToMeterTimeswitchCodeDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitMeterToMeterTimeswitchCodeDataAPI, commitMeterToMeterTimeswitchCodeDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterDataRows = _tempCustomerMethods.Meter_GetByProcessQueueGUID(processQueueGUID);
                var commitableDataRows = _tempCustomerMethods.GetCommitableRows(meterDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, false, null);
                    return;
                }

                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var meterTimeswitchCodeRangeStartMeterTimeswitchCodeAttributeId = _informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(_informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchRangeStart);
                var meterTimeswitchCodeRangeEndMeterTimeswitchCodeAttributeId = _informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(_informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchRangeEnd);

                foreach(var dataRow in commitableDataRows)
                {
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var mpxn = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN);
                    var meterId = _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn);

                    //Get MeterTimeswitchCodeId from [Information].[MeterTimeswitchCodeDetail]
                    var meterTimeswitchCode = Convert.ToInt64(dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MeterTimeswitchCode));
                    
                    var meterTimeswitchCodeRangeStartAttributeId = _informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(_informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchRangeStart);
                    var meterTimeswitchCodeRangeEndAttributeId = _informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(_informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchRangeEnd);

                    var meterTimeswitchCodeRangeStartDataTable = _informationMethods.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(meterTimeswitchCodeRangeStartAttributeId);
                    var meterTimeswitchCodeRangeEndDataTable = _informationMethods.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(meterTimeswitchCodeRangeEndAttributeId);

                    var validRangeStartDataRecords = meterTimeswitchCodeRangeStartDataTable.Rows.Cast<DataRow>().Where(r => r.Field<int>("MeterTimeswitchCodeDetailDescription") <= meterTimeswitchCode);
                    var validRangeEndDataRecords = meterTimeswitchCodeRangeEndDataTable.Rows.Cast<DataRow>().Where(r => r.Field<int>("MeterTimeswitchCodeDetailDescription") >= meterTimeswitchCode);

                    var meterTimeswitchCodeDataRows = validRangeStartDataRecords.Intersect(validRangeEndDataRecords).First();
                    var meterTimeswitchCodeId = meterTimeswitchCodeDataRows.Field<long>("MeterTimeswitchId");

                    //Insert into [Mapping].[MeterToMeterTimeswitchCode]
                    _mappingMethods.MeterToMeterTimeswitchCode_Insert(createdByUserId, sourceId, meterTimeswitchCodeId, meterId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}