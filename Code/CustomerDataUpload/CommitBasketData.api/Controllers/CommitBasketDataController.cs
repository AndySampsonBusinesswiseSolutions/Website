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
        #region Variables
        private readonly ILogger<CommitBasketDataController> _logger;
        private readonly Methods.System _systemMethods = new Methods.System();
        private readonly Methods.System.API _systemAPIMethods = new Methods.System.API();
        private readonly Methods.Information _informationMethods = new Methods.Information();
        private readonly Methods.Customer _customerMethods = new Methods.Customer();
        private readonly Methods.Mapping _mappingMethods = new Methods.Mapping();
        private readonly Methods.Temp.CustomerDataUpload _tempCustomerDataUploadMethods = new Methods.Temp.CustomerDataUpload();
        private static readonly Enums.SystemSchema.API.Name _systemAPINameEnums = new Enums.SystemSchema.API.Name();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private readonly Enums.CustomerSchema.Contract.Attribute _customerContractAttributeEnums = new Enums.CustomerSchema.Contract.Attribute();
        private readonly Enums.CustomerSchema.Meter.Attribute _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
        private readonly Enums.CustomerSchema.Basket.Attribute _customerBasketAttributeEnums = new Enums.CustomerSchema.Basket.Attribute();
        private readonly Enums.InformationSchema.ContractType _informationContractTypeEnums = new Enums.InformationSchema.ContractType();
        private readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();
        private readonly Int64 commitBasketDataAPIId;
        private readonly string hostEnvironment;
        #endregion

        public CommitBasketDataController(ILogger<CommitBasketDataController> logger, IConfiguration configuration)
        {
            var password = configuration["Password"];
            hostEnvironment = configuration["HostEnvironment"];

            _logger = logger;
            new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitBasketDataAPI, password);
            commitBasketDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitBasketDataAPI);
        }

        [HttpPost]
        [Route("CommitBasketData/IsRunning")]
        public bool IsRunning([FromBody] object data)
        {
            //Launch API process
            _systemAPIMethods.PostAsJsonAsync(commitBasketDataAPIId, hostEnvironment, JObject.Parse(data.ToString()));

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

                if(!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitBasketDataAPI, commitBasketDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitBasketDataAPIId);

                //Get data from [Temp.CustomerDataUpload].[FlexContract] where CanCommit = 1
                var flexContractEntities = new Methods.Temp.CustomerDataUpload.FlexContract().FlexContract_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableFlexContractEntities = _tempCustomerDataUploadMethods.GetCommitableEntities(flexContractEntities);

                if(!commitableFlexContractEntities.Any())
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

                var baskets = commitableFlexContractEntities.Select(cfce => cfce.BasketReference).Distinct()
                    .ToDictionary(b => b, b => _customerMethods.BasketDetail_GetBasketIdByBasketAttributeIdAndBasketDetailDescription(basketReferenceBasketAttributeId, b));

                var meters = commitableFlexContractEntities.Select(cfce => cfce.MPXN).Distinct()
                    .ToDictionary(m => m, m => _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                var contractReferences = commitableFlexContractEntities.Select(cfce => cfce.ContractReference).Distinct()
                    .ToDictionary(c => c, c => _customerMethods.ContractDetail_GetContractIdListByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, c));

                var contractMeterToMeterContractMeterIdLists = meters.Select(m => m.Value)
                    .Distinct()
                    .ToDictionary(m => m, m => _mappingMethods.ContractMeterToMeter_GetContractMeterIdListByMeterId(m));

                foreach(var flexContractEntity in commitableFlexContractEntities)
                {
                    //Get BasketId from [Customer].[BasketDetail] by BasketReference
                    var basketId = baskets[flexContractEntity.BasketReference];

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[flexContractEntity.MPXN];

                    //Get ContractIdList from [Customer].[Contract] by ContractReference
                    var customerContractIdList = contractReferences[flexContractEntity.ContractReference];

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