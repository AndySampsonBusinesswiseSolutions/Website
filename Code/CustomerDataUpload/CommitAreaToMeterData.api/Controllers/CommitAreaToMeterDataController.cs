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

namespace CommitAreaToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitAreaToMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitAreaToMeterDataController> _logger;
        private readonly Int64 commitAreaToMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitAreaToMeterDataController(ILogger<CommitAreaToMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitAreaToMeterDataAPI, password);
            commitAreaToMeterDataAPIId = new Methods.System.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitAreaToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitAreaToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            new Methods.System.API().PostAsJsonAsync(commitAreaToMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitAreaToMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            var administrationUserMethods = new Methods.Administration.User();
            var systemMethods = new Methods.System();
            var informationMethods = new Methods.Information();

            //Get base variables
            var createdByUserId = administrationUserMethods.GetSystemUserId();
            var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

            //Get Queue GUID
            var jsonObject = JObject.Parse(data.ToString());
            var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

            try
            {
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitAreaToMeterDataAPIId);

                if(!new Methods.System.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitAreaToMeterDataAPI, commitAreaToMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitAreaToMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = new Methods.Temp.CustomerDataUpload().GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAreaToMeterDataAPIId, false, null);
                    return;
                }

                var mappingMethods = new Methods.Mapping();
                var customerMethods = new Methods.Customer();
                var customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();

                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                
                var areas = commitableMeterEntities.Select(cme => cme.Area).Distinct()
                    .ToDictionary(a => a, a => informationMethods.GetAreaId(createdByUserId, sourceId, a));
                
                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                var newAreaToMeterEntities = commitableMeterEntities.Where(cme => mappingMethods.AreaToMeter_GetAreaToMeterIdByAreaIdAndMeterId(areas[cme.Area], meters[cme.MPXN]) == 0)
                    .GroupBy(cme => new { cme.Area, cme.MPXN }).ToList();

                foreach(var meterEntity in newAreaToMeterEntities)
                {
                    //Insert into [Mapping].[AreaToMeter]
                    mappingMethods.AreaToMeter_Insert(createdByUserId, sourceId, areas[meterEntity.Key.Area], meters[meterEntity.Key.MPXN]);            
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAreaToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitAreaToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}