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

namespace CommitMeterExemptionData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitMeterExemptionDataController : ControllerBase
    {
        #region Variables
        private readonly ILogger<CommitMeterExemptionDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Information.MeterExemption.Attribute _informationMeterExemptionAttributeEnums = new Enums.Information.MeterExemption.Attribute();
        private readonly Enums.Customer.MeterExemption.Attribute _customerMeterExemptionAttributeEnums = new Enums.Customer.MeterExemption.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitMeterExemptionDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitMeterExemptionDataController(ILogger<CommitMeterExemptionDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.System.API.Name().CommitMeterExemptionDataAPI, password);
            commitMeterExemptionDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitMeterExemptionDataAPI);
        }

        [HttpPost]
        [Route("CommitMeterExemptionData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitMeterExemptionDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitMeterExemptionData/Commit")]
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
                    commitMeterExemptionDataAPIId);

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitMeterExemptionDataAPI, commitMeterExemptionDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitMeterExemptionDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[MeterExemption] where CanCommit = 1
                var meterExemptionEntities = new Methods.Temp.CustomerDataUpload.MeterExemption().MeterExemption_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableMeterExemptionEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(meterExemptionEntities);

                if(!commitableMeterExemptionEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterExemptionDataAPIId, false, null);
                    return;
                }

                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var meterExemptionProductMeterExemptionAttributeId = _informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_informationMeterExemptionAttributeEnums.MeterExemptionProduct);
                var meterExemptionProportionMeterExemptionAttributeId = _informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_informationMeterExemptionAttributeEnums.MeterExemptionProportion);
                var useDefaultValueMeterExemptionAttributeId = _informationMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_informationMeterExemptionAttributeEnums.UseDefaultValue);

                var dateFromMeterExemptionAttributeId = _customerMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_customerMeterExemptionAttributeEnums.DateFrom);
                var dateToMeterExemptionAttributeId = _customerMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_customerMeterExemptionAttributeEnums.DateTo);
                var exemptionProportionMeterExemptionAttributeId = _customerMethods.MeterExemptionAttribute_GetMeterExemptionAttributeIdByMeterExemptionAttributeDescription(_customerMeterExemptionAttributeEnums.ExemptionProportion);

                var meterExemptions = commitableMeterExemptionEntities.Select(cmee => cmee.ExemptionProduct).Distinct()
                    .ToDictionary(me => me, me => _informationMethods.MeterExemptionDetail_GetMeterExemptionIdByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(meterExemptionProductMeterExemptionAttributeId, me));
                
                var meters = commitableMeterExemptionEntities.Select(cmee => cmee.MPXN).Distinct()
                    .ToDictionary(m => m, m => _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                foreach(var meterExemptionEntity in commitableMeterExemptionEntities)
                {
                    //Get MeterExemptionId from [Information].[MeterExemption]
                    var meterExemptionId = meterExemptions[meterExemptionEntity.ExemptionProduct];

                    //Check if a default value exists
                    var useDefaultValue = _informationMethods.MeterExemptionDetail_GetMeterExemptionDetailDescriptionByMeterExemptionIdAndMeterExemptionAttributeId(meterExemptionId, useDefaultValueMeterExemptionAttributeId);

                    //Get ExemptionId from [Customer].[MeterExemptionDetail] by DateFrom, DateTo and ExemptionProportion
                    var exemptionProportion = string.IsNullOrWhiteSpace(useDefaultValue)
                        ? (Convert.ToInt64(meterExemptionEntity.ExemptionProportion.Replace("%", string.Empty))/100M).ToString()
                        : _informationMethods.MeterExemptionDetail_GetMeterExemptionDetailDescriptionByMeterExemptionIdAndMeterExemptionAttributeId(meterExemptionId, meterExemptionProportionMeterExemptionAttributeId);

                    var customerMeterExemptionId = 0L;
                    var dateFromMeterExemptionIdList = _customerMethods.MeterExemptionDetail_GetMeterExemptionIdListByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(dateFromMeterExemptionAttributeId, meterExemptionEntity.DateFrom);
                    if(dateFromMeterExemptionIdList.Any())
                    {
                        var dateToMeterExemptionIdList = _customerMethods.MeterExemptionDetail_GetMeterExemptionIdListByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(dateToMeterExemptionAttributeId, meterExemptionEntity.DateTo);
                        if(dateToMeterExemptionIdList.Any())
                        {
                            var exemptionProportionMeterExemptionIdList = _customerMethods.MeterExemptionDetail_GetMeterExemptionIdListByMeterExemptionAttributeIdAndMeterExemptionDetailDescription(exemptionProportionMeterExemptionAttributeId, exemptionProportion);
                            if(exemptionProportionMeterExemptionIdList.Any())
                            {
                                customerMeterExemptionId = dateFromMeterExemptionIdList.Intersect(dateToMeterExemptionIdList).Intersect(exemptionProportionMeterExemptionIdList).FirstOrDefault();
                            }
                        }
                    }

                    if(customerMeterExemptionId == 0)
                    {
                        customerMeterExemptionId = _customerMethods.InsertNewMeterExemption(createdByUserId, sourceId);

                        //Insert into [Customer].[MeterExemptionDetail]
                        _customerMethods.MeterExemptionDetail_Insert(createdByUserId, sourceId, customerMeterExemptionId, dateFromMeterExemptionAttributeId, meterExemptionEntity.DateFrom);
                        _customerMethods.MeterExemptionDetail_Insert(createdByUserId, sourceId, customerMeterExemptionId, dateToMeterExemptionAttributeId, meterExemptionEntity.DateTo);
                        _customerMethods.MeterExemptionDetail_Insert(createdByUserId, sourceId, customerMeterExemptionId, exemptionProportionMeterExemptionAttributeId, exemptionProportion);
                    }

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[meterExemptionEntity.MPXN];

                    //Insert into [Mapping].[MeterToMeterExemption]
                    _mappingMethods.MeterToMeterExemption_Insert(createdByUserId, sourceId, meterId, customerMeterExemptionId);

                    var meterToMeterExemptionId = _mappingMethods.MeterToMeterExemption_GetMeterToMeterExemptionIdByMeterIdAndMeterExemptionId(meterId, customerMeterExemptionId);

                    //Insert into [Mapping].[MeterExemptionToMeterExemptionProduct]
                    _mappingMethods.MeterExemptionToMeterExemptionProduct_Insert(createdByUserId, sourceId, customerMeterExemptionId, meterExemptionId);

                    //Insert into [Mapping].[MeterToMeterExemptionToMeterExemptionProduct]
                    _mappingMethods.MeterToMeterExemptionToMeterExemptionProduct_Insert(createdByUserId, sourceId, meterToMeterExemptionId, meterExemptionId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterExemptionDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitMeterExemptionDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}