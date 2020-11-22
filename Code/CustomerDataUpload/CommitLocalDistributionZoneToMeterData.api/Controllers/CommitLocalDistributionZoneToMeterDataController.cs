using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

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
        private readonly Int64 commitLocalDistributionZoneToMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitLocalDistributionZoneToMeterDataController(ILogger<CommitLocalDistributionZoneToMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitLocalDistributionZoneToMeterDataAPI, password);
            commitLocalDistributionZoneToMeterDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitLocalDistributionZoneToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitLocalDistributionZoneToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(commitLocalDistributionZoneToMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitLocalDistributionZoneToMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var informationMethods = new Methods.Information();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

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
                    commitLocalDistributionZoneToMeterDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitLocalDistributionZoneToMeterDataAPI, commitLocalDistributionZoneToMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId);
                
                var customerDataUploadProcessQueueGUID = _systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.Temp.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId, false, null);
                    return;
                }

                var mappingMethods = new Methods.Mapping();
                var customerMethods = new Methods.Customer();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var localDistributionZoneLocalDistributionZoneAttributeId = informationMethods.LocalDistributionZoneAttribute_GetLocalDistributionZoneAttributeIdByLocalDistributionZoneAttributeDescription(new Enums.InformationSchema.LocalDistributionZone.Attribute().LocalDistributionZone);

                var localDistributionZones = commitableMeterEntities.Select(cme => cme.LocalDistributionZone).Distinct()
                    .ToDictionary(ldz => ldz, ldz => informationMethods.GetLocalDistributionZoneId(ldz, createdByUserId, sourceId, localDistributionZoneLocalDistributionZoneAttributeId));
                
                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterEntity in commitableMeterEntities)
                {
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];

                    //Get LocalDistributionZoneId from [Information].[LocalDistributionZoneDetail]
                    var localDistributionZoneId = localDistributionZones[meterEntity.LocalDistributionZone];

                    //Get existing LocalDistributionZoneToMeter Id
                    var existingLocalDistributionZoneToMeterId = mappingMethods.LocalDistributionZoneToMeter_GetLocalDistributionZoneToMeterIdByLocalDistributionZoneIdAndMeterId(localDistributionZoneId, meterId);

                    if(existingLocalDistributionZoneToMeterId == 0)
                    {
                        //Insert into [Mapping].[LocalDistributionZoneToMeter]
                        mappingMethods.LocalDistributionZoneToMeter_Insert(createdByUserId, sourceId, localDistributionZoneId, meterId);
                    }
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
    }
}