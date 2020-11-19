using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace CommitMeterToSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterToSubMeterDataController> _logger;
        private readonly Int64 commitMeterToSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterToSubMeterDataController(ILogger<CommitMeterToSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToSubMeterDataAPI, password);
            commitMeterToSubMeterDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterToSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(commitMeterToSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToSubMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = new Methods.Information().GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitMeterToSubMeterDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitMeterToSubMeterDataAPI, commitMeterToSubMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterToSubMeterDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterEntities = new Methods.Temp.CustomerDataUpload.SubMeter().SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSubMeterEntities = new Methods.Temp.CustomerDataUpload().GetCommitableEntities(subMeterEntities);

                if(!commitableSubMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToSubMeterDataAPIId, false, null);
                    return;
                }

                var customerSubMeterAttributeEnums = new Enums.CustomerSchema.SubMeter.Attribute();
                var mappingMethods = new Methods.Mapping();
                var customerMethods = new Methods.Customer();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var subMeterIdentifierSubMeterAttributeId = customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(customerSubMeterAttributeEnums.SubMeterIdentifier);
                var subMeterSerialNumberSubMeterAttributeId = customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(customerSubMeterAttributeEnums.SerialNumber);

                var meters = commitableSubMeterEntities.Select(csme => csme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                var subMeters = commitableSubMeterEntities.Select(csme => csme.SubMeterIdentifier).Distinct()
                    .ToDictionary(s => s, s => customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, s));

                foreach(var subMeterEntity in commitableSubMeterEntities)
                {
                    //Get MeterId by MPXN
                    var meterId = meters[subMeterEntity.MPXN];

                    //Get SubMeterId from [Customer].[SubMeterDetail] by SubMeterIdentifier
                    var subMeterId = subMeters[subMeterEntity.SubMeterIdentifier];

                    //Get existing MeterToSubMeter Id
                    var existingMeterToSubMeterId = mappingMethods.MeterToSubMeter_GetMeterToSubMeterIdByMeterIdAndSubMeterId(meterId, subMeterId);

                    if(existingMeterToSubMeterId == 0)
                    {
                        //Insert into [Mapping].[MeterToSubMeter]
                        mappingMethods.MeterToSubMeter_Insert(createdByUserId, sourceId, meterId, subMeterId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToSubMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}