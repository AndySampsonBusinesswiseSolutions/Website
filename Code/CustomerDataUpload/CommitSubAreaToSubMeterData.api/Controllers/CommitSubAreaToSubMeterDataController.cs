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

namespace CommitSubAreaToSubMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitSubAreaToSubMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitSubAreaToSubMeterDataController> _logger;
        private readonly Int64 commitSubAreaToSubMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitSubAreaToSubMeterDataController(ILogger<CommitSubAreaToSubMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitSubAreaToSubMeterDataAPI, password);
            commitSubAreaToSubMeterDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitSubAreaToSubMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitSubAreaToSubMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(commitSubAreaToSubMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitSubAreaToSubMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var informationMethods = new Methods.Information();
            var systemMethods = new Methods.System();

            //Get base variables
            var createdByUserId = new Methods.Administration.User().GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

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
                    commitSubAreaToSubMeterDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitSubAreaToSubMeterDataAPI, commitSubAreaToSubMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitSubAreaToSubMeterDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[SubMeter] where CanCommit = 1
                var subMeterEntities = new Methods.Temp.CustomerDataUpload.SubMeter().SubMeter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableSubMeterEntities = new Methods.Temp.CustomerDataUpload().GetCommitableEntities(subMeterEntities);

                if(!commitableSubMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubAreaToSubMeterDataAPIId, false, null);
                    return;
                }

                var mappingMethods = new Methods.Mapping();
                var customerMethods = new Methods.Customer();

                var subMeterIdentifierSubMeterAttributeId = customerMethods.SubMeterAttribute_GetSubMeterAttributeIdBySubMeterAttributeDescription(new Enums.CustomerSchema.SubMeter.Attribute().SubMeterIdentifier);

                var subAreas = commitableSubMeterEntities.Select(csme => csme.SubArea).Distinct()
                    .ToDictionary(sa => sa, sa => informationMethods.GetSubAreaId(sa, createdByUserId, sourceId));
                
                var subMeters = commitableSubMeterEntities.Select(csme => csme.SubMeterIdentifier).Distinct()
                    .ToDictionary(sm => sm, sm => customerMethods.SubMeterDetail_GetSubMeterDetailIdBySubMeterAttributeIdAndSubMeterDetailDescription(subMeterIdentifierSubMeterAttributeId, sm));

                foreach(var subMeterEntity in commitableSubMeterEntities)
                {
                    //Get SubAreaId from [Information].[SubArea]
                    var subAreaId = subAreas[subMeterEntity.SubArea];

                    //Get SubMeterId from [Customer].[SubMeterDetail] by SubMeterIdentifier
                    var subMeterId = subMeters[subMeterEntity.SubMeterIdentifier];

                    //Get existing SubAreaToSubMeter Id
                    var existingSubAreaToSubMeterId = mappingMethods.SubAreaToSubMeter_GetSubAreaToSubMeterIdBySubAreaIdAndSubMeterId(subAreaId, subMeterId);

                    if(existingSubAreaToSubMeterId == 0)
                    {
                        //Insert into [Mapping].[SubAreaToSubMeter]
                        mappingMethods.SubAreaToSubMeter_Insert(createdByUserId, sourceId, subAreaId, subMeterId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubAreaToSubMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitSubAreaToSubMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}