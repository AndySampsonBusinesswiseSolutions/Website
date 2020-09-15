using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace CommitMeterToProfileClassData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToProfileClassDataController : ControllerBase
    {
        private readonly ILogger<CommitMeterToProfileClassDataController> _logger;
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
        private readonly Enums.Information.ProfileClass.Attribute _informationProfileClassAttributeEnums = new Enums.Information.ProfileClass.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitMeterToProfileClassDataAPIId;

        public CommitMeterToProfileClassDataController(ILogger<CommitMeterToProfileClassDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitMeterToProfileClassDataAPI, _systemAPIPasswordEnums.CommitMeterToProfileClassDataAPI);
            commitMeterToProfileClassDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitMeterToProfileClassDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToProfileClassData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitMeterToProfileClassDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToProfileClassData/Commit")]
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
                    commitMeterToProfileClassDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitMeterToProfileClassDataAPI, commitMeterToProfileClassDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterDataRows = _tempCustomerDataUploadMethods.Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(meterDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId, false, null);
                    return;
                }

                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var profileClassCodeProfileClassAttributeId = _informationMethods.ProfileClassAttribute_GetProfileClassAttributeIdByProfileClassAttributeDescription(_informationProfileClassAttributeEnums.ProfileClassCode);

                var profileClasses = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.ProfileClass).PadLeft(2, '0'))
                    .Distinct()
                    .ToDictionary(pc => pc, pc => GetProfileClassId(pc, profileClassCodeProfileClassAttributeId));
                
                var meters = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN))
                    .Distinct()
                    .ToDictionary(m => m, m => _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var dataRow in commitableDataRows)
                {
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)];
                    
                    //Get ProfileClassId from [Information].[ProfileClassDetail]
                    var profileClassId = profileClasses[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ProfileClass).PadLeft(2, '0')];

                    //Does mapping exist between meter and profile class
                    var existingMeterProfileClassId = _mappingMethods.MeterToProfileClass_GetProfileClassIdByMeterId(meterId);

                    if(existingMeterProfileClassId != profileClassId)
                    {
                        //Insert into [Mapping].[MeterToProfileClass]
                        _mappingMethods.MeterToProfileClass_Insert(createdByUserId, sourceId, profileClassId, meterId);
                    }
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private long GetProfileClassId(string profileClass, long profileClassGroupIdProfileClassAttributeId)
        {
            return _informationMethods.ProfileClassDetail_GetProfileClassIdByProfileClassAttributeIdAndProfileClassDetailDescription(profileClassGroupIdProfileClassAttributeId, profileClass);
        }
    }
}