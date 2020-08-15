using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace CommitBasketData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitBasketDataController : ControllerBase
    {
        private readonly ILogger<CommitBasketDataController> _logger;
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
        private readonly Enums.Customer.Contract.Attribute _customerContractAttributeEnums = new Enums.Customer.Contract.Attribute();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Customer.Basket.Attribute _customerBasketAttributeEnums = new Enums.Customer.Basket.Attribute();
        private readonly Enums.Information.ContractType _informationContractTypeEnums = new Enums.Information.ContractType();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitBasketDataAPIId;

        public CommitBasketDataController(ILogger<CommitBasketDataController> logger)
        {
            _logger = logger;
            _methods.InitialiseDatabaseInteraction(_systemAPINameEnums.CommitBasketDataAPI, _systemAPIPasswordEnums.CommitBasketDataAPI);
            commitBasketDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitBasketDataAPI);
        }

        [HttpPost]
        [Route("CommitBasketData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitBasketDataAPIId, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitBasketData/Commit")]
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
                    commitBasketDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitBasketDataAPI, commitBasketDataAPIId, jsonObject))
                {
                    return;
                }

                //Get data from [Temp.CustomerDataUpload].[Meter] where CanCommit = 1
                var meterDataRows = _tempCustomerMethods.Meter_GetByProcessQueueGUID(processQueueGUID);
                var commitableDataRows = _tempCustomerMethods.GetCommitableRows(meterDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_Update(processQueueGUID, commitBasketDataAPIId, false, null);
                    return;
                }

                var basketReferenceBasketAttributeId = _customerMethods.BasketAttribute_GetBasketAttributeIdByBasketAttributeDescription(_customerBasketAttributeEnums.BasketReference);
                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var contractReferenceContractAttributeId = _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference);

                foreach(var dataRow in commitableDataRows)
                {
                    //Get BasketId from [Customer].[BasketDetail] by BasketReference
                    var basketReference = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.BasketReference);
                    var basketId = _customerMethods.BasketDetail_GetBasketIdByBasketAttributeIdAndBasketDetailDescription(basketReferenceBasketAttributeId, basketReference);

                    if(basketId == 0)
                    {
                        //Create new BasketGUID
                        var basketGUID = Guid.NewGuid().ToString();

                        //Insert into [Customer].[Basket]
                        _customerMethods.Basket_Insert(createdByUserId, sourceId, basketGUID);
                        basketId = _customerMethods.Basket_GetBasketIdByBasketGUID(basketGUID);

                        _customerMethods.BasketDetail_Insert(createdByUserId, sourceId, basketId, basketReferenceBasketAttributeId, basketReference);
                    }

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var mpxn = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN);
                    var meterId = _customerMethods.MeterDetail_GetMeterDetailIdByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, mpxn);

                    //Get ContractTypeId from [Information].[ContractType] where ContractTypeDescription = 'Flex'
                    var contractTypeId = _informationMethods.ContractType_GetContractTypeIdByContractTypeDescription(_informationContractTypeEnums.Flex);

                    //Get ContractIdList from [Mapping].[ContractToContractType] by ContractTypeId
                    var mappingContractIdList = _mappingMethods.ContractToContractType_GetContractIdListByContractTypeId(contractTypeId);

                    //Get ContractIdList from [Customer].[Contract] by ContractReference
                    var contractReference = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference);
                    var customerContractIdList = _customerMethods.ContractDetail_GetContractIdListByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, contractReference);

                    //Get ContractId from intersect of Customer ContractIdList and Mapping ContractIdList
                    var contractId = customerContractIdList.Intersect(mappingContractIdList).First();

                    //Get ContractMeterIdList from [Mapping].[ContractMeterToMeter] by MeterId
                    var contractMeterToMeterContractMeterIdList = _mappingMethods.ContractMeterToMeter_GetContractMeterIdListByMeterId(meterId);

                    //Get ContractMeterIdList from [Mapping].[ContractToContractMeter] by ContractId
                    var contractToContractMeterContractMeterIdList = _mappingMethods.ContractToContractMeter_GetContractMeterIdListByContractId(contractId);

                    //Get ContractMeterId from intersect of ContractMeterToMeter ContractMeterIdList and ContractToContractMeter ContractMeterIdList
                    var contractMeterId = contractMeterToMeterContractMeterIdList.Intersect(contractToContractMeterContractMeterIdList).First();

                    //Insert into [Mapping].[BasketToContractMeter]
                    _mappingMethods.BasketToMeter_Insert(createdByUserId, sourceId, basketId, contractMeterId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitBasketDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_Update(processQueueGUID, commitBasketDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}

