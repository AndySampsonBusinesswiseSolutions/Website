using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace CommitLocalDistributionZoneToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitLocalDistributionZoneToMeterDataController : ControllerBase
    {
        private readonly ILogger<CommitLocalDistributionZoneToMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Information.LocalDistributionZone.Attribute _informationLocalDistributionZoneAttributeEnums = new Enums.Information.LocalDistributionZone.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitLocalDistributionZoneToMeterDataAPIId;

        public CommitLocalDistributionZoneToMeterDataController(ILogger<CommitLocalDistributionZoneToMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitLocalDistributionZoneToMeterDataAPI, _systemAPIPasswordEnums.CommitLocalDistributionZoneToMeterDataAPI);
            commitLocalDistributionZoneToMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitLocalDistributionZoneToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitLocalDistributionZoneToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitLocalDistributionZoneToMeterDataAPIId, JObject.Parse(data.ToString()));

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

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitLocalDistributionZoneToMeterDataAPI, commitLocalDistributionZoneToMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterDataRows = _tempCustomerDataUploadMethods.Meter_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(meterDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitLocalDistributionZoneToMeterDataAPIId, false, null);
                    return;
                }

                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var localDistributionZoneLocalDistributionZoneAttributeId = _informationMethods.LocalDistributionZoneAttribute_GetLocalDistributionZoneAttributeIdByLocalDistributionZoneAttributeDescription(_informationLocalDistributionZoneAttributeEnums.LocalDistributionZone);

                var localDistributionZones = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.LocalDistributionZone))
                    .Distinct()
                    .ToDictionary(ldz => ldz, ldz => GetLocalDistributionZoneId(ldz, createdByUserId, sourceId, localDistributionZoneLocalDistributionZoneAttributeId));
                
                var meters = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN))
                    .Distinct()
                    .ToDictionary(m => m, m => _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var dataRow in commitableDataRows)
                {
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)];

                    //Get LocalDistributionZoneId from [Information].[LocalDistributionZoneDetail]
                    var localDistributionZoneId = localDistributionZones[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.LocalDistributionZone)];

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