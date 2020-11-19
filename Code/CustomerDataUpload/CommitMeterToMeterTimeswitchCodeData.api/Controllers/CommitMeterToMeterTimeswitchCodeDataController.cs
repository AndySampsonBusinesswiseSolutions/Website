using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace CommitMeterToMeterTimeswitchCodeData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterToMeterTimeswitchCodeDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterToMeterTimeswitchCodeDataController> _logger;
        private readonly Int64 commitMeterToMeterTimeswitchCodeDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterToMeterTimeswitchCodeDataController(ILogger<CommitMeterToMeterTimeswitchCodeDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitMeterToMeterTimeswitchCodeDataAPI, password);
            commitMeterToMeterTimeswitchCodeDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitMeterToMeterTimeswitchCodeDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterToMeterTimeswitchCodeData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(commitMeterToMeterTimeswitchCodeDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterToMeterTimeswitchCodeData/Commit")]
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
                    commitMeterToMeterTimeswitchCodeDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitMeterToMeterTimeswitchCodeDataAPI, commitMeterToMeterTimeswitchCodeDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId);

                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.Temp.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, false, null);
                    return;
                }

                var informationMeterTimeswitchCodeAttributeEnums = new Enums.InformationSchema.MeterTimeswitchCode.Attribute();
                var mappingMethods = new Methods.Mapping();
                var customerMethods = new Methods.Customer();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var meterTimeswitchCodeRangeStartMeterTimeswitchCodeAttributeId = informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchCodeRangeStart);
                var meterTimeswitchCodeRangeEndMeterTimeswitchCodeAttributeId = informationMethods.MeterTimeswitchCodeAttribute_GetMeterTimeswitchCodeAttributeIdByMeterTimeswitchCodeAttributeDescription(informationMeterTimeswitchCodeAttributeEnums.MeterTimeswitchCodeRangeEnd);

                var meterTimeswitchCodeRangeStartEntities = informationMethods.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(meterTimeswitchCodeRangeStartMeterTimeswitchCodeAttributeId);
                var meterTimeswitchCodeRangeEndEntities = informationMethods.MeterTimeswitchCodeDetail_GetByMeterTimeswitchCodeAttributeId(meterTimeswitchCodeRangeEndMeterTimeswitchCodeAttributeId);

                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterEntity in commitableMeterEntities)
                {
                    //Get MeterTimeswitchCodeId from [Information].[MeterTimeswitchCodeDetail]
                    if(string.IsNullOrWhiteSpace(meterEntity.MeterTimeswitchCode))
                    {
                        continue;
                    }

                    var meterTimeswitchCodeValue = Convert.ToInt64(meterEntity.MeterTimeswitchCode);

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];
                    
                    var validRangeStartEntities = meterTimeswitchCodeRangeStartEntities.Where(mtcrse => Convert.ToInt64(mtcrse.MeterTimeswitchCodeDetailDescription) <= meterTimeswitchCodeValue);
                    var validRangeEndEntities = meterTimeswitchCodeRangeEndEntities.Where(mtcree => Convert.ToInt64(mtcree.MeterTimeswitchCodeDetailDescription) >= meterTimeswitchCodeValue);

                    //Get MeterTimeswitchIds
                    var validRangeStartMeterTimeswitchIdList = validRangeStartEntities.Select(r => r.MeterTimeswitchCodeId);
                    var validRangeEndMeterTimeswitchIdList = validRangeEndEntities.Select(r => r.MeterTimeswitchCodeId);

                    var meterTimeswitchCodeId = validRangeStartMeterTimeswitchIdList.Intersect(validRangeEndMeterTimeswitchIdList).FirstOrDefault();

                    //Get existing MeterToMeterTimeswitchCode Id
                    var existingMeterToMeterTimeswitchCodeId = mappingMethods.MeterToMeterTimeswitchCode_GetMeterToMeterTimeswitchCodeIdByMeterIdAndMeterTimeswitchCodeId(meterId, meterTimeswitchCodeId);

                    if(existingMeterToMeterTimeswitchCodeId == 0)
                    {
                        //Insert into [Mapping].[MeterToMeterTimeswitchCode]
                        mappingMethods.MeterToMeterTimeswitchCode_Insert(createdByUserId, sourceId, meterTimeswitchCodeId, meterId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterToMeterTimeswitchCodeDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}