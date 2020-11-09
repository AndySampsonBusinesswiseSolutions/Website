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
        private readonly ILogger<CommitGridSupplyPointToMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Information.GridSupplyPoint.Attribute _informationGridSupplyPointAttributeEnums = new Enums.Information.GridSupplyPoint.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitGridSupplyPointToMeterDataAPIId;
        private readonly string hostEnvironment;

        public CommitGridSupplyPointToMeterDataController(ILogger<CommitGridSupplyPointToMeterDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.CommitGridSupplyPointToMeterDataAPI, password);
            commitGridSupplyPointToMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitGridSupplyPointToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitGridSupplyPointToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitGridSupplyPointToMeterDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitGridSupplyPointToMeterDataAPI, commitGridSupplyPointToMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitGridSupplyPointToMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterDataRows = _tempCustomerDataUploadMethods.Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(meterDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitGridSupplyPointToMeterDataAPIId, false, null);
                    return;
                }

                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var gridSupplyPointGroupIdGridSupplyPointAttributeId = _informationMethods.GridSupplyPointAttribute_GetGridSupplyPointAttributeIdByGridSupplyPointAttributeDescription(_informationGridSupplyPointAttributeEnums.GridSupplyPointGroupId);

                var gridSupplyPoints = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.GridSupplyPoint))
                    .Distinct()
                    .ToDictionary(gsp => gsp, gsp => GetGridSupplyPointId(gsp, createdByUserId, sourceId, gridSupplyPointGroupIdGridSupplyPointAttributeId));
                
                var meters = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN))
                    .Distinct()
                    .ToDictionary(m => m, m => _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var dataRow in commitableDataRows)
                {
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)];

                    //Get GridSupplyPointId from [Information].[GridSupplyPointDetail]
                    var gridSupplyPointId = gridSupplyPoints[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.GridSupplyPoint)];

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