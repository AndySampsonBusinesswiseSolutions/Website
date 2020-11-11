using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace CommitMeterToMeterTimeswitchCodeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToMeterTimeswitchCodeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterToMeterTimeswitchCodeDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Meter.Attribute _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
        private readonly Enums.InformationSchema.MeterTimeswitchCode.Attribute _informationMeterTimeswitchCodeAttributeEnums = new Enums.InformationSchema.MeterTimeswitchCode.Attribute();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitMeterToMeterTimeswitchCodeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterToMeterTimeswitchCodeDataController(ILogger<CommitMeterToMeterTimeswitchCodeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToMeterTimeswitchCodeDataAPI, password);
            commitMeterToMeterTimeswitchCodeDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitMeterToMeterTimeswitchCodeDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToMeterTimeswitchCodeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitMeterToMeterTimeswitchCodeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToMeterTimeswitchCodeData/Commit")]
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
                    commitMeterToMeterTimeswitchCodeDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitMeterToMeterTimeswitchCodeDataAPI, commitMeterToMeterTimeswitchCodeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, false, null);
                    return;
                }

                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var meterTimeswitchCodeRangeStartMeterTimeswitchCodeAttributeId = _informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(_informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchCodeRangeStart);
                var meterTimeswitchCodeRangeEndMeterTimeswitchCodeAttributeId = _informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(_informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchCodeRangeEnd);

                var meterTimeswitchCodeRangeStartDataTable = _informationMethods.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(meterTimeswitchCodeRangeStartMeterTimeswitchCodeAttributeId);
                var meterTimeswitchCodeRangeEndDataTable = _informationMethods.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(meterTimeswitchCodeRangeEndMeterTimeswitchCodeAttributeId);

                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterEntity in commitableMeterEntities)
                {
                    //Get MeterTimeswitchCodeId from [Information].[MeterTimeswitchCodeDetail]
                    if(string.IsNullOrWhiteSpace(meterEntity.MeterTimeswitchCode))
                    {
                        continue;
                    }

                    var meterTimeswitchCodeValue = Convert.ToInt64(meterEntity.MeterTimeswitchCode);

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];
                    
                    var validRangeStartDataRecords = meterTimeswitchCodeRangeStartDataTable.Rows.Cast<DataRow>().Where(r => Convert.ToInt64(r.Field<string>("MeterTimeswitchCodeDetailDescription")) <= meterTimeswitchCodeValue);
                    var validRangeEndDataRecords = meterTimeswitchCodeRangeEndDataTable.Rows.Cast<DataRow>().Where(r => Convert.ToInt64(r.Field<string>("MeterTimeswitchCodeDetailDescription")) >= meterTimeswitchCodeValue);

                    //Get MeterTimeswitchIds
                    var validRangeStartMeterTimeswitchIdList = validRangeStartDataRecords.Select(r => r.Field<long>("MeterTimeswitchCodeId"));
                    var validRangeEndMeterTimeswitchIdList = validRangeEndDataRecords.Select(r => r.Field<long>("MeterTimeswitchCodeId"));

                    var meterTimeswitchCodeId = validRangeStartMeterTimeswitchIdList.Intersect(validRangeEndMeterTimeswitchIdList).FirstOrDefault();

                    //Insert into [Mapping].[MeterToMeterTimeswitchCode]
                    _mappingMethods.MeterToMeterTimeswitchCode_Insert(createdByUserId, sourceId, meterTimeswitchCodeId, meterId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}