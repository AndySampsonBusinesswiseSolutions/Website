using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace CommitLineLossFactorClassToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitLineLossFactorClassToMeterDataController : ControllerBase
    {
        private readonly ILogger<CommitLineLossFactorClassToMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        // private readonly Enums.Information.LineLossFactorClass.Attribute _informationLineLossFactorClassAttributeEnums = new Enums.Information.LineLossFactorClass.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitLineLossFactorClassToMeterDataAPIId;

        public CommitLineLossFactorClassToMeterDataController(ILogger<CommitLineLossFactorClassToMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitLineLossFactorClassToMeterDataAPI, _systemAPIPasswordEnums.CommitLineLossFactorClassToMeterDataAPI);
            commitLineLossFactorClassToMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitLineLossFactorClassToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitLineLossFactorClassToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitLineLossFactorClassToMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitLineLossFactorClassToMeterData/Commit")]
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
                    commitLineLossFactorClassToMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitLineLossFactorClassToMeterDataAPI, commitLineLossFactorClassToMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //TODO: Build once LLF process is built

                // //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                // var meterDataRows = _tempCustomerDataUploadMethods.Meter_GetByProcessQueueGUID(processQueueGUID);
                // var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(meterDataRows);

                // if(!commitableDataRows.Any())
                // {
                //     //Nothing to commit so update Process Queue and exit
                //     _systemMethods.ProcessQueue_Update(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId, false, null);
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
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitLineLossFactorClassToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}