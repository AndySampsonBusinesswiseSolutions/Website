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
using Microsoft.Extensions.Configuration;

namespace CommitBasketData.api.Controllers
{
    [EnableCors]
    [ApiController]
    public class CommitBasketDataController : ControllerBase
    {
        private readonly ILogger<CommitBasketDataController> _logger;
        private static readonly Methods _methods = new Methods();
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.Customer.Contract.Attribute _customerContractAttributeEnums = new Enums.Customer.Contract.Attribute();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Customer.Basket.Attribute _customerBasketAttributeEnums = new Enums.Customer.Basket.Attribute();
        private readonly Enums.Information.ContractType _informationContractTypeEnums = new Enums.Information.ContractType();
        private readonly Enums.Customer.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.Customer.DataUploadValidation.Entity();
        private readonly Int64 commitBasketDataAPIId;
        private readonly string hostEnvironment;

        public CommitBasketDataController(ILogger<CommitBasketDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            _methods.InitialiseDatabaseInteraction(hostEnvironment, _systemAPINameEnums.CommitBasketDataAPI, password);
            commitBasketDataAPIId = _systemMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitBasketDataAPI);
        }

        [HttpPost]
        [Route("CommitBasketData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemMethods.PostAsJsonAsync(commitBasketDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

            return true;
        }

        [HttpPost]
        [Route("CommitBasketData/Commit")]
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
                    commitBasketDataAPIId);

                if(!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitBasketDataAPI, commitBasketDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitBasketDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexContract] where CanCommit = 1
                var flexContractDataRows = _tempCustomerDataUploadMethods.FlexContract_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableDataRows = _tempCustomerDataUploadMethods.GetCommitableRows(flexContractDataRows);

                if(!commitableDataRows.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitBasketDataAPIId, false, null);
                    return;
                }

                var basketReferenceBasketAttributeId = _customerMethods.BasketAttribute_GetBasketAttributeIdByBasketAttributeDescription(_customerBasketAttributeEnums.BasketReference);
                var meterIdentifierMeterAttributeId = _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier);
                var contractReferenceContractAttributeId = _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference);

                //Get ContractTypeId from [Information].[ContractType] where ContractTypeDescription = 'Flex'
                var contractTypeId = _informationMethods.ContractType_GetContractTypeIdByContractTypeDescription(_informationContractTypeEnums.Flex);

                //Get ContractIdList from [Mapping].[ContractToContractType] by ContractTypeId
                var mappingContractIdList = _mappingMethods.ContractToContractType_GetContractIdListByContractTypeId(contractTypeId);

                var baskets = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.BasketReference))
                    .Distinct()
                    .ToDictionary(b => b, b => _customerMethods.BasketDetail_GetBasketIdByBasketAttributeIdAndBasketDetailDescription(basketReferenceBasketAttributeId, b));

                var meters = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN))
                    .Distinct()
                    .ToDictionary(m => m, m => _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                var contractReferences = commitableDataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference))
                    .Distinct()
                    .ToDictionary(c => c, c => _customerMethods.ContractDetail_GetContractIdListByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, c));

                var contractMeterToMeterContractMeterIdLists = meters.Select(m => m.Value)
                    .Distinct()
                    .ToDictionary(m => m, m => _mappingMethods.ContractMeterToMeter_GetContractMeterIdListByMeterId(m));

                foreach(var dataRow in commitableDataRows)
                {
                    //Get BasketId from [Customer].[BasketDetail] by BasketReference
                    var basketId = baskets[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.BasketReference)];

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)];

                    //Get ContractIdList from [Customer].[Contract] by ContractReference
                    var customerContractIdList = contractReferences[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference)];

                    //Get ContractId from intersect of Customer ContractIdList and Mapping ContractIdList
                    var contractId = customerContractIdList.Intersect(mappingContractIdList).FirstOrDefault();

                    //Get ContractMeterIdList from [Mapping].[ContractMeterToMeter] by MeterId
                    var contractMeterToMeterContractMeterIdList = contractMeterToMeterContractMeterIdLists[meterId];

                    //Get ContractMeterIdList from [Mapping].[ContractToContractMeter] by ContractId
                    var contractToContractMeterContractMeterIdList = _mappingMethods.ContractToContractMeter_GetContractMeterIdListByContractId(contractId);

                    //Get ContractMeterId from intersect of ContractMeterToMeter ContractMeterIdList and ContractToContractMeter ContractMeterIdList
                    var contractMeterId = contractMeterToMeterContractMeterIdList.Intersect(contractToContractMeterContractMeterIdList).FirstOrDefault();

                    //Insert into [Mapping].[BasketToContractMeter]
                    _mappingMethods.BasketToContractMeter_Insert(createdByUserId, sourceId, basketId, contractMeterId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitBasketDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitBasketDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}