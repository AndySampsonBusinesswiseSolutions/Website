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

namespace CommitGridSupplyPointToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitGridSupplyPointToMeterDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitGridSupplyPointToMeterDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Meter.Attribute _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
        private readonly Enums.InformationSchema.GridSupplyPoint.Attribute _informationGridSupplyPointAttributeEnums = new Enums.InformationSchema.GridSupplyPoint.Attribute();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitGridSupplyPointToMeterDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitGridSupplyPointToMeterDataController(ILogger<CommitGridSupplyPointToMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitGridSupplyPointToMeterDataAPI, password);
            commitGridSupplyPointToMeterDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitGridSupplyPointToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitGridSupplyPointToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitGridSupplyPointToMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitGridSupplyPointToMeterData/Commit")]
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
                    commitGridSupplyPointToMeterDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitGridSupplyPointToMeterDataAPI, commitGridSupplyPointToMeterDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitGridSupplyPointToMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterEntities = new Methods.Temp.CustomerDataUpload.Meter().Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(meterEntities);

                if(!commitableMeterEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitGridSupplyPointToMeterDataAPIId, false, null);
                    return;
                }

                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var gridSupplyPointGroupIdGridSupplyPointAttributeId = _informationMethods.GridSupplyPointAttribute_GetGridSupplyPointAttributeIdByGridSupplyPointAttributeDescription(_informationGridSupplyPointAttributeEnums.GridSupplyPointGroupId);

                var gridSupplyPoints = commitableMeterEntities.Select(cme => cme.GridSupplyPoint).Distinct()
                    .ToDictionary(gsp => gsp, gsp => GetGridSupplyPointId(gsp, createdByUserId, sourceId, gridSupplyPointGroupIdGridSupplyPointAttributeId));
                
                var meters = commitableMeterEntities.Select(cme => cme.MPXN).Distinct()
                    .ToDictionary(m => m, m => _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterEntity in commitableMeterEntities)
                {
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterEntity.MPXN];

                    //Get GridSupplyPointId from [Information].[GridSupplyPointDetail]
                    var gridSupplyPointId = gridSupplyPoints[meterEntity.GridSupplyPoint];

                    //Insert into [Mapping].[GridSupplyPointToMeter]
                    _mappingMethods.GridSupplyPointToMeter_Insert(createdByUserId, sourceId, gridSupplyPointId, meterId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitGridSupplyPointToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitGridSupplyPointToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private long GetGridSupplyPointId(string gridSupplyPoint, long createdByUserId, long sourceId, long gridSupplyPointGroupIdGridSupplyPointAttributeId)
        {
            var gridSupplyPointId = _informationMethods.GridSupplyPointDetail_GetGridSupplyPointIdByGridSupplyPointAttributeIdAndGridSupplyPointDetailDescription(gridSupplyPointGroupIdGridSupplyPointAttributeId, gridSupplyPoint);

            if(gridSupplyPointId == 0)
            {
                gridSupplyPointId = _informationMethods.InsertNewGridSupplyPoint(createdByUserId, sourceId);

                //Insert into [Customer].[GridSupplyPointDetail]
                _informationMethods.GridSupplyPointDetail_Insert(createdByUserId, sourceId, gridSupplyPointId, gridSupplyPointGroupIdGridSupplyPointAttributeId, gridSupplyPoint);
            }

            return gridSupplyPointId;
        }
    }
}