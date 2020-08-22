using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;

namespace CommitMeterExemptionData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterExemptionDataController : ControllerBase
    {
        private readonly ILogger<CommitMeterExemptionDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Information.MeterExemption.Attribute _informationMeterExemptionAttributeEnums = new Enums.Information.MeterExemption.Attribute();
        private readonly Enums.Customer.MeterExemption.Attribute _customerMeterExemptionAttributeEnums = new Enums.Customer.MeterExemption.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitMeterExemptionDataAPIId;

        public CommitMeterExemptionDataController(ILogger<CommitMeterExemptionDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitMeterExemptionDataAPI, _systemAPIPasswordEnums.CommitMeterExemptionDataAPI);
            commitMeterExemptionDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitMeterExemptionDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterExemptionData/Commit")]
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
                    commitMeterExemptionDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitMeterExemptionDataAPI, commitMeterExemptionDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[MeterExemption] where CanCommit = 1
                var meterExemptionDataRows = _tempCustomerDataUploadMethods.MeterExemption_GetByProcessQueueGUID(processQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(meterExemptionDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterExemptionDataAPIId, false, null);
                    return;
                }

                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var meterExemptionProductMeterExemptionAttributeId = _informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_informationMeterExemptionAttributeEnums.MeterExemptionProduct);
                var meterExemptionProportionMeterExemptionAttributeId = _informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_informationMeterExemptionAttributeEnums.MeterExemptionProportion);
                var useDefaultValueMeterExemptionAttributeId = _informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_informationMeterExemptionAttributeEnums.UseDefaultValue);

                var dateFromMeterExemptionAttributeId = _customerMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_customerMeterExemptionAttributeEnums.DateFrom);
                var dateToMeterExemptionAttributeId = _customerMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_customerMeterExemptionAttributeEnums.DateTo);
                var exemptionProportionMeterExemptionAttributeId = _customerMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_customerMeterExemptionAttributeEnums.ExemptionProportion);

                foreach(var dataRow in commitableDataRows)
                {
                    //Get MeterExemptionId from [Information].[MeterExemption]
                    var meterExemptionProduct = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ExemptionProduct);
                    var meterExemptionId = _informationMethods.MeterExemptionDetail_GetMeterExemptionIdByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(meterExemptionProductMeterExemptionAttributeId, meterExemptionProduct);

                    //Check if a default value exists
                    var useDefaultValue = _informationMethods.MeterExemptionDetail_GetMeterExemptionDetailDescriptionByMeterExemptionIdAndMeterExemptionAttributeId(meterExemptionId, useDefaultValueMeterExemptionAttributeId);

                    //Get ExemptionId from [Customer].[MeterExemptionDetail] by DateFrom, DateTo and ExemptionProportion
                    var dateFrom = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.DateFrom);
                    var dateTo = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.DateTo);
                    var exemptionProportion = string.IsNullOrWhiteSpace(useDefaultValue)
                        ? (Convert.ToInt64(dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ExemptionProportion).Replace("%", string.Empty))/100M).ToString()
                        : _informationMethods.MeterExemptionDetail_GetMeterExemptionDetailDescriptionByMeterExemptionIdAndMeterExemptionAttributeId(meterExemptionId, meterExemptionProportionMeterExemptionAttributeId);

                    var customerMeterExemptionId = 0L;
                    var dateFromMeterExemptionIdList = _customerMethods.MeterExemptionDetail_GetMeterExemptionIdListByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(dateFromMeterExemptionAttributeId, dateFrom);
                    if(dateFromMeterExemptionIdList.Any())
                    {
                        var dateToMeterExemptionIdList = _customerMethods.MeterExemptionDetail_GetMeterExemptionIdListByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(dateToMeterExemptionAttributeId, dateTo);
                        if(dateToMeterExemptionIdList.Any())
                        {
                            var exemptionProportionMeterExemptionIdList = _customerMethods.MeterExemptionDetail_GetMeterExemptionIdListByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(exemptionProportionMeterExemptionAttributeId, exemptionProportion);
                            if(exemptionProportionMeterExemptionIdList.Any())
                            {
                                customerMeterExemptionId = dateFromMeterExemptionIdList.Intersect(dateToMeterExemptionIdList).Intersect(exemptionProportionMeterExemptionIdList).First();
                            }
                        }
                    }

                    if(customerMeterExemptionId == 0)
                    {
                        //Create new MeterGUID
                        var meterExemptionGUID = Guid.NewGuid().ToString();

                        //Insert into [Customer].[MeterExemption]
                        _customerMethods.MeterExemption_Insert(createdByUserId, sourceId, meterExemptionGUID);
                        customerMeterExemptionId = _customerMethods.MeterExemption_GetMeterExemptionIdByMeterExemptionGUID(meterExemptionGUID);

                        //Insert into [Customer].[MeterExemptionDetail]
                        _customerMethods.MeterExemptionDetail_Insert(createdByUserId, sourceId, customerMeterExemptionId, dateFromMeterExemptionAttributeId, dateFrom);
                        _customerMethods.MeterExemptionDetail_Insert(createdByUserId, sourceId, customerMeterExemptionId, dateToMeterExemptionAttributeId, dateTo);
                        _customerMethods.MeterExemptionDetail_Insert(createdByUserId, sourceId, customerMeterExemptionId, exemptionProportionMeterExemptionAttributeId, exemptionProportion);
                    }

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var mpxn = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN);
                    var meterId = _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn);

                    //Insert into [Mapping].[MeterToMeterExemption]
                    _mappingMethods.MeterToMeterExemption_Insert(createdByUserId, sourceId, meterId, customerMeterExemptionId);

                    var meterToMeterExemptionId = _mappingMethods.MeterToMeterExemption_GetMeterToMeterExemptionIdByMeterIdAndMeterExemptionId(meterId, customerMeterExemptionId);

                    //Insert into [Mapping].[MeterExemptionToMeterExemptionProduct]
                    _mappingMethods.MeterExemptionToMeterExemptionProduct_Insert(createdByUserId, sourceId, customerMeterExemptionId, meterExemptionId);

                    //Insert into [Mapping].[MeterToMeterExemptionToMeterExemptionProduct]
                    _mappingMethods.MeterToMeterExemptionToMeterExemptionProduct_Insert(createdByUserId, sourceId, meterToMeterExemptionId, meterExemptionId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterExemptionDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitMeterExemptionDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

