using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace CommitContractData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitContractDataController : ControllerBase
    {
        private readonly ILogger<CommitContractDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Administration _administrationMethods = new Methods.Administration();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Information.ContractType _informationContractTypeEnums = new Enums.Information.ContractType();
        private readonly Enums.Information.RateType _informationRateTypeEnums = new Enums.Information.RateType();
        private readonly Enums.Customer.Contract.Attribute _customerContractAttributeEnums = new Enums.Customer.Contract.Attribute();
        private readonly Enums.Customer.ContractMeter.Attribute _customerContractMeterAttributeEnums = new Enums.Customer.ContractMeter.Attribute();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitContractDataAPIId;

        public CommitContractDataController(ILogger<CommitContractDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitContractDataAPI, _systemAPIPasswordEnums.CommitContractDataAPI);
            commitContractDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitContractDataAPI);
        }

        [HttpPost]
        [Route("CommitContractData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitContractDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitContractData/Commit")]
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
                    commitContractDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitContractDataAPI, commitContractDataAPIId, jsonObject))
                {
                    return;
                }

                var dataRowList = (IEnumerable<DataRow>) JsonConvert.DeserializeObject(jsonObject[_systemAPIRequiredDataKeyEnums.ContractData].ToString(), typeof(List<DataRow>));

                //Get ContractType from jsonObject
                var contractType = jsonObject[_systemAPIRequiredDataKeyEnums.ContractType].ToString();

                //Get ContractTypeId from [Information].[ContractType]
                var contractTypeId = _informationMethods.ContractType_GetContractTypeIdByContractTypeDescription(contractType);

                var contractReferenceContractAttributeId = _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference);
                var contractStartDateContractMeterAttributeId = _customerMethods.ContractMeterAttribute_GetContractMeterAttributeIdByContractMeterAttributeDescription(_customerContractMeterAttributeEnums.ContractStartDate);
                var contractEndDateContractMeterAttributeId = _customerMethods.ContractMeterAttribute_GetContractMeterAttributeIdByContractMeterAttributeDescription(_customerContractMeterAttributeEnums.ContractEndDate);
                var rateCountContractMeterAttributeId = _customerMethods.ContractMeterAttribute_GetContractMeterAttributeIdByContractMeterAttributeDescription(_customerContractMeterAttributeEnums.RateCount);
                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var standingChargeRateTypeId = _informationMethods.RateType_GetRateTypeIdByRateTypeDescription(_informationRateTypeEnums.StandingCharge);
                var capacityChargeRateTypeId = _informationMethods.RateType_GetRateTypeIdByRateTypeDescription(_informationRateTypeEnums.CapacityCharge);

                foreach(var dataRow in dataRowList)
                {
                    //Get ContractId from [Customer].[ContractDetail] by ContractReference
                    var contractReference = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference);
                    var contractId = _customerMethods.ContractDetail_GetContractIdListByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, contractReference).First();

                    if(contractId == 0)
                    {
                        //Create new ContractGUID
                        var contractGUID = Guid.NewGuid().ToString();

                        //Insert into [Customer].[Contract]
                        _customerMethods.Contract_Insert(createdByUserId, sourceId, contractGUID);
                        contractId = _customerMethods.Contract_GetContractIdByContractGUID(contractGUID);

                        //Insert into [Customer].[ContractDetail]
                        _customerMethods.ContractDetail_Insert(createdByUserId, sourceId, contractId, contractReferenceContractAttributeId, contractReference);
                    }

                    //Get ContractToContractTypeId by ContractId and ContractTypeId
                    var contractToContractTypeId = _mappingMethods.ContractToContractType_GetContractToContractTypeIdByContractIdAndContractTypeId(contractId, contractTypeId);

                    if(contractToContractTypeId == 0)
                    {
                        //Insert into [Mapping].[ContractToContractType]
                        _mappingMethods.ContractToContractType_Insert(createdByUserId, sourceId, contractId, contractTypeId);
                    }

                    //Get ContractMeterId from [Customer].[ContractMeterDetail] by ContractStartDate, ContractEndDate and RateCount
                    var contractStartDate = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractStartDate);
                    var contractEndDate = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractEndDate);
                    var rateCount = contractType == _informationContractTypeEnums.Fixed
                        ? dataRow.Field<string>(_customerDataUploadValidationEntityEnums.RateCount)
                        : "1";

                    var contractMeterId = 0L;
                    var contractStartDateMeterIdList = _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(contractStartDateContractMeterAttributeId, contractStartDate);
                    if(contractStartDateMeterIdList.Any())
                    {
                        var contractEndDateMeterIdList = _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(contractEndDateContractMeterAttributeId, contractEndDate);
                        if(contractEndDateMeterIdList.Any())
                        {
                            var rateCountMeterIdList = _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(rateCountContractMeterAttributeId, rateCount);
                            if(rateCountMeterIdList.Any())
                            {
                                contractMeterId = contractStartDateMeterIdList.Intersect(contractEndDateMeterIdList).Intersect(rateCountMeterIdList).First();
                            }
                        }
                    }

                    if(contractMeterId == 0)
                    {
                        //Create new ContractMeterGUID
                        var contractMeterGUID = Guid.NewGuid().ToString();

                        //Insert into [Customer].[ContractMeter]
                        _customerMethods.ContractMeter_Insert(createdByUserId, sourceId, contractMeterGUID);
                        contractMeterId = _customerMethods.ContractMeter_GetContractMeterIdByContractMeterGUID(contractMeterGUID);

                        //Insert into [Customer].[ContractMeterDetail]
                        _customerMethods.ContractMeterDetail_Insert(createdByUserId, sourceId, contractMeterId, contractStartDateContractMeterAttributeId, contractStartDate);
                        _customerMethods.ContractMeterDetail_Insert(createdByUserId, sourceId, contractMeterId, contractEndDateContractMeterAttributeId, contractEndDate);
                        _customerMethods.ContractMeterDetail_Insert(createdByUserId, sourceId, contractMeterId, rateCountContractMeterAttributeId, rateCount);
                    }

                    //Get ContractToContractMeterId by ContractId and ContractMeterId
                    var contractToContractMeterId = _mappingMethods.ContractToContractMeter_GetContractToContractMeterIdByContractIdAndContractMeterId(contractId, contractMeterId);

                    if(contractToContractMeterId == 0)
                    {
                        //Insert into [Mapping].[ContractToContractMeter]
                        _mappingMethods.ContractToContractMeter_Insert(createdByUserId, sourceId, contractId, contractMeterId);
                    }

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var mpxn = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN);
                    var meterId = _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn);

                    //Get ContractMeterToMeterId by ContractMeterId and MeterId
                    var contractMeterToMeterId = _mappingMethods.ContractMeterToMeter_GetContractMeterToMeterIdByContractMeterIdAndMeterId(contractMeterId, meterId);

                    if(contractMeterToMeterId == 0)
                    {
                        //Insert into [Mapping].[ContractMeterToMeter]
                        _mappingMethods.ContractMeterToMeter_Insert(createdByUserId, sourceId, contractMeterId, meterId);
                    }

                    var standingCharge = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.StandingCharge);
                    var capacityCharge = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.CapacityCharge);
                }

                //For each rate and standing and capacity charges
                //Get RateTypeId from [Information].[RateType]
                //Get ContractMeterRateDetailId from [Customer].[ContractMeterRateDetail] by Value
                //If ContractMeterRateDetailId == 0
                //Insert into [Customer].[ContractMeterRateDetail]

                //Insert into [Mapping].[ContractMeterRateDetailToRateType]
                //Insert into [Mapping].[ContractToMeterToContractMeterRateDetail]

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitContractDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

