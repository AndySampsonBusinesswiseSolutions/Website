using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace CommitAssetToSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitAssetToSubMeterDataController : ControllerBase
    {
        private readonly ILogger<CommitAssetToSubMeterDataController> _logger;
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
        private readonly Enums.Customer.SubMeter.Attribute _customerSubMeterAttributeEnums = new Enums.Customer.SubMeter.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitAssetToSubMeterDataAPIId;

        public CommitAssetToSubMeterDataController(ILogger<CommitAssetToSubMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitAssetToSubMeterDataAPI, _systemAPIPasswordEnums.CommitAssetToSubMeterDataAPI);
            commitAssetToSubMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitAssetToSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitAssetToSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitAssetToSubMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitAssetToSubMeterData/Commit")]
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
                    commitAssetToSubMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitAssetToSubMeterDataAPI, commitAssetToSubMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterDataRows = _tempCustomerMethods.SubMeter_GetByProcessQueueGUID(processQueueGUID);
                var commitableDataRows = _tempCustomerMethods.GetCommitableRows(subMeterDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitAssetToSubMeterDataAPIId, false, null);
                    return;
                }

                var subMeterIdentifierSubMeterAttributeId = _customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(_customerSubMeterAttributeEnums.SubMeterIdentifier);

                foreach(var dataRow in commitableDataRows)
                {
                    //Get SubAreaId from [Information].[SubArea]
                    var subArea = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.SubArea);
                    var subAreaId = _informationMethods.SubArea_GetSubAreaIdBySubAreaDescription(subArea);

                    if(subAreaId == 0)
                    {
                        _informationMethods.SubArea_Insert(createdByUserId, sourceId, subArea);
                        subAreaId = _informationMethods.SubArea_GetSubAreaIdBySubAreaDescription(subArea);
                    }

                    //Get SubMeterId from [Customer].[SubMeterDetail] by MPXN
                    var mpxn = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN);
                    var meterId = _customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, mpxn);

                    //Insert into [Mapping].[SubAreaToSubMeter]
                    _mappingMethods.SubAreaToSubMeter_Insert(createdByUserId, sourceId, subAreaId, meterId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitAssetToSubMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitAssetToSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

