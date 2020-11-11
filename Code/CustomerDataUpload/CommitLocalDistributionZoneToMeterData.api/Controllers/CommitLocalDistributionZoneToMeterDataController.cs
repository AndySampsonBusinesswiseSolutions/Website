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

namespace CommitLocalDistributionZoneToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitLocalDistributionZoneToMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitLocalDistributionZoneToMeterDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Meter.Attribute _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
        private readonly Enums.InformationSchema.LocalDistributionZone.Attribute _informationLocalDistributionZoneAttributeEnums = new Enums.InformationSchema.LocalDistributionZone.Attribute();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitLocalDistributionZoneToMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitLocalDistributionZoneToMeterDataController(ILogger<CommitLocalDistributionZoneToMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitLocalDistributionZoneToMeterDataAPI, password);
            commitLocalDistributionZoneToMeterDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitLocalDistributionZoneToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitLocalDistributionZoneToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitLocalDistributionZoneToMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitLocalDistributionZoneToMeterData/Commit")]
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
                    commitLocalDistributionZoneToMeterDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitLocalDistributionZoneToMeterDataAPI, commitLocalDistributionZoneToMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId, false, null);
                    return;
                }

                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var localDistributionZoneLocalDistributionZoneAttributeId = _informationMethods.LocalDistributionZoneAttribute_GetLocalDistributionZoneAttributeIdByLocalDistributionZoneAttributeDescription(_informationLocalDistributionZoneAttributeEnums.LocalDistributionZone);

                var localDistributionZones = commitableMeterEntities.Select(cme => cme.LocalDistributionZone).Distinct()
                    .ToDictionary(ldz => ldz, ldz => GetLocalDistributionZoneId(ldz, createdByUserId, sourceId, localDistributionZoneLocalDistributionZoneAttributeId));
                
                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterEntity in commitableMeterEntities)
                {
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];

                    //Get LocalDistributionZoneId from [Information].[LocalDistributionZoneDetail]
                    var localDistributionZoneId = localDistributionZones[meterEntity.LocalDistributionZone];

                    //Insert into [Mapping].[LocalDistributionZoneToMeter]
                    _mappingMethods.LocalDistributionZoneToMeter_Insert(createdByUserId, sourceId, localDistributionZoneId, meterId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private long GetLocalDistributionZoneId(string localDistributionZone, long createdByUserId, long sourceId, long localDistributionZoneGroupIdLocalDistributionZoneAttributeId)
        {
            var localDistributionZoneId = _informationMethods.LocalDistributionZoneDetail_GetLocalDistributionZoneIdByLocalDistributionZoneAttributeIdAndLocalDistributionZoneDetailDescription(localDistributionZoneGroupIdLocalDistributionZoneAttributeId, localDistributionZone);

            if(localDistributionZoneId == 0)
            {
                localDistributionZoneId = _informationMethods.InsertNewLocalDistributionZone(createdByUserId, sourceId);

                //Insert into [Customer].[LocalDistributionZoneDetail]
                _informationMethods.LocalDistributionZoneDetail_Insert(createdByUserId, sourceId, localDistributionZoneId, localDistributionZoneGroupIdLocalDistributionZoneAttributeId, localDistributionZone);
            }

            return localDistributionZoneId;
        }
    }
}