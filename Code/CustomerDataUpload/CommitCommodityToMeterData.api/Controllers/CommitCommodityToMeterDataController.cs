using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace CommitCommodityToMeterData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitCommodityToMeterDataController : ControllerBase
    {
        private readonly ILogger<CommitCommodityToMeterDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.Customer _tempCustomerMethods = new Methods.Temp.Customer();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Information.Commodity _informationCommodityEnums = new Enums.Information.Commodity();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitCommodityToMeterDataAPIId;

        public CommitCommodityToMeterDataController(ILogger<CommitCommodityToMeterDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitCommodityToMeterDataAPI, _systemAPIPasswordEnums.CommitCommodityToMeterDataAPI);
            commitCommodityToMeterDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitCommodityToMeterDataAPI);
        }

        [HttpPost]
        [Route("CommitCommodityToMeterData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitCommodityToMeterDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitCommodityToMeterData/Commit")]
        public void Commit([FromBody] object data)
        {
            //Get base variables
            var createdByUserId = _administrationMethods.GetSystemUserId();
            var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

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
                    commitCommodityToMeterDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitCommodityToMeterDataAPI, commitCommodityToMeterDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterDataRows = _tempCustomerMethods.Meter_GetByProcessQueueGUID(processQueueGUID);
                var commitableDataRows = _tempCustomerMethods.GetCommitableRows(meterDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitCommodityToMeterDataAPIId, false, null);
                    return;
                }

                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);

                foreach(var dataRow in commitableDataRows)
                {
                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var mpxn = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN);
                    var meterId = _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn);

                    //Get CommodityId from [Information].[Commodity]
                    var commodity = _methods.IsValidMPAN(mpxn) ? _informationCommodityEnums.Electricity : _informationCommodityEnums.Gas;
                    var commodityId = _informationMethods.Commodity_GetCommodityIdByCommodityDescription(commodity);

                    if(commodityId == 0)
                    {
                        _informationMethods.Commodity_Insert(createdByUserId, sourceId, commodity);
                        commodityId = _informationMethods.Commodity_GetCommodityIdByCommodityDescription(commodity);
                    }

                    //Insert into [Mapping].[CommodityToMeter]
                    _mappingMethods.CommodityToMeter_Insert(createdByUserId, sourceId, commodityId, meterId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitCommodityToMeterDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitCommodityToMeterDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

