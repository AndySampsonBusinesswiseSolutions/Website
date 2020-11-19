using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitAreaToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitAreaToMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitAreaToMeterDataController> _logger;
        private readonly Entity.System.API.CommitAreaToMeterData.Configuration _configuration;
        #endregion

        public CommitAreaToMeterDataController(ILogger<CommitAreaToMeterDataController> logger, Entity.System.API.CommitAreaToMeterData.Configuration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("CommitAreaToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsyncAndDoNotAwaitResult(_configuration.APIId, _configuration.HostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitAreaToMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var systemMethods = new Methods.System();
            var informationMethods = new Methods.Information();

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
                    _configuration.APIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(_configuration.APIGUID, _configuration.APIId, _configuration.HostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, _configuration.APIId);
                
                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.Temp.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, _configuration.APIId, false, null);
                    return;
                }

                var mappingMethods = new Methods.Mapping();
                var customerMethods = new Methods.Customer();
                
                var areas = commitableMeterEntities.Select(cme => cme.Area).Distinct()
                    .ToDictionary(a => a, a => informationMethods.GetAreaId(createdByUserId, sourceId, a));
                
                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(_configuration.MeterIdentifierMeterAttributeId, m).FirstOrDefault());

                var newAreaToMeterEntities = commitableMeterEntities.Where(cme => mappingMethods.AreaToMeter_GetAreaToMeterIdByAreaIdAndMeterId(areas[cme.Area], meters[cme.MPXN]) == 0)
                    .GroupBy(cme => new { cme.Area, cme.MPXN }).ToList();

                foreach(var meterEntity in newAreaToMeterEntities)
                {
                    //End date existing AreaToMeter mappings
                    mappingMethods.AreaToMeter_DeleteByMeterId(meters[meterEntity.Key.MPXN]);

                    //Insert into [Mapping].[AreaToMeter]
                    mappingMethods.AreaToMeter_Insert(createdByUserId, sourceId, areas[meterEntity.Key.Area], meters[meterEntity.Key.MPXN]);            
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, _configuration.APIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, _configuration.APIId, true, $"System Error Id {errorId}");
            }
        }
    }
}