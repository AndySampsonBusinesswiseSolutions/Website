using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace CommitSubAreaToSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitSubAreaToSubMeterDataController : ControllerBase
    {
        private readonly ILogger<CommitSubAreaToSubMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitSubAreaToSubMeterDataAPIId;

        public CommitSubAreaToSubMeterDataController(ILogger<CommitSubAreaToSubMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitSubAreaToSubMeterDataAPI, _systemAPIPasswordEnums.CommitSubAreaToSubMeterDataAPI);
            commitSubAreaToSubMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitSubAreaToSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitSubAreaToSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitSubAreaToSubMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitSubAreaToSubMeterData/Commit")]
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
                    commitSubAreaToSubMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitSubAreaToSubMeterDataAPI, commitSubAreaToSubMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitSubAreaToSubMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterDataRows = _tempCustomerDataUploadMethods.SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(subMeterDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubAreaToSubMeterDataAPIId, false, null);
                    return;
                }

                var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);

                var subAreas = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubArea))
                    .Distinct()
                    .ToDictionary(sa => sa, sa => GetSubAreaId(sa, createdByUserId, sourceId));
                
                var subMeters = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier))
                    .Distinct()
                    .ToDictionary(sm => sm, sm => _customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, sm));

                foreach(var dataRow in commitableDataRows)
                {
                    //Get SubAreaId from [Information].[SubArea]
                    var subAreaId = subAreas[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.SubArea)];

                    //Get SubMeterId from [Customer].[SubMeterDetail] by SubMeterIdentifier
                    var subMeterId = subMeters[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.SubMeterIdentifier)];

                    //Insert into [Mapping].[SubAreaToSubMeter]
                    _mappingMethods.SubAreaToSubMeter_Insert(createdByUserId, sourceId, subAreaId, subMeterId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubAreaToSubMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubAreaToSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private long GetSubAreaId(string subArea, long createdByUserId, long sourceId)
        {
            var subAreaId = _informationMethods.SubArea_GetSubAreaIdBySubAreaDescription(subArea);

            if(subAreaId == 0)
            {
                _informationMethods.SubArea_Insert(createdByUserId, sourceId, subArea);
                subAreaId = _informationMethods.SubArea_GetSubAreaIdBySubAreaDescription(subArea);
            }

            return subAreaId;
        }
    }
}