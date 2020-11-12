using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace CommitLineLossFactorClassToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitLineLossFactorClassToMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitLineLossFactorClassToMeterDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Meter.Attribute _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
        // private readonly Enums.Information.LineLossFactorClass.Attribute _informationLineLossFactorClassAttributeEnums = new Enums.Information.LineLossFactorClass.Attribute();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitLineLossFactorClassToMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitLineLossFactorClassToMeterDataController(ILogger<CommitLineLossFactorClassToMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitLineLossFactorClassToMeterDataAPI, password);
            commitLineLossFactorClassToMeterDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitLineLossFactorClassToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitLineLossFactorClassToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitLineLossFactorClassToMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitLineLossFactorClassToMeterData/Commit")]
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
                    commitLineLossFactorClassToMeterDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitLineLossFactorClassToMeterDataAPI, commitLineLossFactorClassToMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId);

                //TODO: Build once LLF process is built

                // //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                // var meterDataRows = _tempCustomerDataUploadMethods.Meter_GetByProcessQueueGUID(processQueueGUID);
                // var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(meterDataRows);

                // if(!commitableDataRows.Any())
                // {
                //     //Nothing to commit so update Process Queue and exit
                //     _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId, false, null);
                //     return;
                // }

                // var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                // var lineLossFactorClassGroupIdLineLossFactorClassAttributeId = _informationMethods.LineLossFactorClassAttribute_GetLineLossFactorClassAttributeIdByLineLossFactorClassAttributeDescription(_informationLineLossFactorClassAttributeEnums.LineLossFactorClassGroupId);

                // foreach(var dataRow in commitableDataRows)
                // {
                //     //Get MeterId from [Customer].[MeterDetail] by MPXN
                //     var mpxn = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN);
                //     var meterId = _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn);

                //     //Get LineLossFactorClassId from [Information].[LineLossFactorClassDetail]
                //     var lineLossFactorClass = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.LineLossFactorClass);
                //     var lineLossFactorClassId = _informationMethods.LineLossFactorClassDetail_GetLineLossFactorClassIdByLineLossFactorClassAttributeIdAndLineLossFactorClassDetailDescription(lineLossFactorClassGroupIdLineLossFactorClassAttributeId, lineLossFactorClass);

                //     if(lineLossFactorClassId == 0)
                //     {
                //         //Create new LineLossFactorClassGUID
                //         var lineLossFactorClassGUID = Guid.NewGuid().ToString();

                //         //Insert into [Customer].[LineLossFactorClass]
                //         _informationMethods.LineLossFactorClass_Insert(createdByUserId, sourceId, lineLossFactorClassGUID);
                //         lineLossFactorClassId = _informationMethods.LineLossFactorClass_GetLineLossFactorClassIdByLineLossFactorClassGUID(lineLossFactorClassGUID);

                //         //Insert into [Customer].[LineLossFactorClassDetail]
                //         _informationMethods.LineLossFactorClassDetail_Insert(createdByUserId, sourceId, lineLossFactorClassId, lineLossFactorClassGroupIdLineLossFactorClassAttributeId, lineLossFactorClass);
                //     }

                //     //Insert into [Mapping].[LineLossFactorClassToMeter]
                //     _mappingMethods.LineLossFactorClassToMeter_Insert(createdByUserId, sourceId, lineLossFactorClassId, meterId);
                // }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}