using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace CommitMeterToProfileClassData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToProfileClassDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterToProfileClassDataController> _logger;
        private readonly Int64 commitMeterToProfileClassDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterToProfileClassDataController(ILogger<CommitMeterToProfileClassDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToProfileClassDataAPI, password);
            commitMeterToProfileClassDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterToProfileClassDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToProfileClassData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(commitMeterToProfileClassDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToProfileClassData/Commit")]
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
                    commitMeterToProfileClassDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitMeterToProfileClassDataAPI, commitMeterToProfileClassDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.Temp.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId, false, null);
                    return;
                }

                var mappingMethods = new Methods.Mapping();
                var customerMethods = new Methods.Customer();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var profileClassCodeProfileClassAttributeId = informationMethods.ProfileClassAttribute_GetProfileClassAttributeIdByProfileClassAttributeDescription(new Enums.InformationSchema.ProfileClass.Attribute().ProfileClassCode);

                var profileClasses = commitableMeterEntities.Where(cme => !string.IsNullOrWhiteSpace(cme.ProfileClass)).Select(cme => cme.ProfileClass).Distinct()
                    .ToDictionary(pc => pc, pc => informationMethods.ProfileClassDetail_GetProfileClassIdByProfileClassAttributeIdAndProfileClassDetailDescription(profileClassCodeProfileClassAttributeId, pc));
                
                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterEntity in commitableMeterEntities)
                {
                    if(string.IsNullOrWhiteSpace(meterEntity.ProfileClass))
                    {
                        continue;
                    }

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];
                    
                    //Get ProfileClassId from [Information].[ProfileClassDetail]
                    var profileClassId = profileClasses[meterEntity.ProfileClass];

                    //Does mapping exist between meter and profile class
                    var existingMeterProfileClassId = mappingMethods.MeterToProfileClass_GetProfileClassIdByMeterId(meterId);

                    if(existingMeterProfileClassId != profileClassId)
                    {
                        //Insert into [Mapping].[MeterToProfileClass]
                        mappingMethods.MeterToProfileClass_Insert(createdByUserId, sourceId, profileClassId, meterId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToProfileClassDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}