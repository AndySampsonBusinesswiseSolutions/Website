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

namespace CommitMeterToProfileClassData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToProfileClassDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterToProfileClassDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Meter.Attribute _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
        private readonly Enums.InformationSchema.ProfileClass.Attribute _informationProfileClassAttributeEnums = new Enums.InformationSchema.ProfileClass.Attribute();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitMeterToProfileClassDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterToProfileClassDataController(ILogger<CommitMeterToProfileClassDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToProfileClassDataAPI, password);
            commitMeterToProfileClassDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitMeterToProfileClassDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToProfileClassData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitMeterToProfileClassDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToProfileClassData/Commit")]
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
                    commitMeterToProfileClassDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitMeterToProfileClassDataAPI, commitMeterToProfileClassDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId, false, null);
                    return;
                }

                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var profileClassCodeProfileClassAttributeId = _informationMethods.ProfileClassAttribute_GetProfileClassAttributeIdByProfileClassAttributeDescription(_informationProfileClassAttributeEnums.ProfileClassCode);

                var profileClasses = commitableMeterEntities.Select(cme => cme.ProfileClass).Distinct()
                    .ToDictionary(pc => pc, pc => GetProfileClassId(pc, profileClassCodeProfileClassAttributeId));
                
                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterEntity in commitableMeterEntities)
                {
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];
                    
                    //Get ProfileClassId from [Information].[ProfileClassDetail]
                    var profileClassId = profileClasses[meterEntity.ProfileClass];

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