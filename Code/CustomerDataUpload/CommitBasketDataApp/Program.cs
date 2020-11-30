using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CommitBasketDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var password = "zkNYKFH6sdsPPXqz";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitBasketDataAPI, password);
                var commitBasketDataAPIId = new Methods.SystemSchema.API().API_GetAPIIdByAPIGUID(new Enums.SystemSchema.API.GUID().CommitBasketDataAPI);

                var systemMethods = new Methods.SystemSchema();
                var informationMethods = new Methods.InformationSchema();

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = informationMethods.GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                systemMethods.ProcessQueue_Insert(
                    processQueueGUID, 
                    createdByUserId,
                    sourceId,
                    commitBasketDataAPIId);

                if(!new Methods.SystemSchema.API().PrerequisiteAPIsAreSuccessful(new Enums.SystemSchema.API.GUID().CommitBasketDataAPI, commitBasketDataAPIId, hostEnvironment, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitBasketDataAPIId);

                var customerDataUploadProcessQueueGUID = systemMethods.GetCustomerDataUploadProcessQueueGUIDFromJObject(jsonObject);

                //Get data from [Temp.CustomerDataUpload].[FlexContract] where CanCommit = 1
                var flexContractEntities = new Methods.TempSchema.CustomerDataUpload.FlexContract().FlexContract_GetByProcessQueueGUID(customerDataUploadProcessQueueGUID);
                var commitableFlexContractEntities = new Methods.TempSchema.CustomerDataUpload().GetCommitableEntities(flexContractEntities);

                if(!commitableFlexContractEntities.Any())
                {
                    //Nothing to commit so update Process Queue and exit
                    systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitBasketDataAPIId, false, null);
                    return;
                }

                var customerMethods = new Methods.CustomerSchema();
                var mappingMethods = new Methods.MappingSchema();

                var basketReferenceBasketAttributeId = customerMethods.BasketAttribute_GetBasketAttributeIdByBasketAttributeDescription(new Enums.CustomerSchema.Basket.Attribute().BasketReference);
                var meterIdentifierMeterAttributeId = customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(new Enums.CustomerSchema.Meter.Attribute().MeterIdentifier);
                var contractReferenceContractAttributeId = customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(new Enums.CustomerSchema.Contract.Attribute().ContractReference);

                //Get ContractTypeId from [Information].[ContractType] where ContractTypeDescription = 'Flex'
                var contractTypeId = informationMethods.ContractType_GetContractTypeIdByContractTypeDescription(new Enums.InformationSchema.ContractType().Flex);

                //Get ContractIdList from [Mapping].[ContractToContractType] by ContractTypeId
                var contractIdList = mappingMethods.ContractToContractType_GetContractIdListByContractTypeId(contractTypeId);

                var basketDictionary = commitableFlexContractEntities.Select(cfce => cfce.BasketReference).Distinct()
                    .ToDictionary(b => b, b => customerMethods.BasketDetail_GetBasketIdByBasketAttributeIdAndBasketDetailDescription(basketReferenceBasketAttributeId, b));

                var meterDictionary = commitableFlexContractEntities.Select(cfce => cfce.MPXN).Distinct()
                    .ToDictionary(m => m, m => customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(meterIdentifierMeterAttributeId, m).FirstOrDefault());

                var contractReferenceDictionary = commitableFlexContractEntities.Select(cfce => cfce.ContractReference).Distinct()
                    .ToDictionary(c => c, c => customerMethods.ContractDetail_GetContractIdListByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, c));

                var contractMeterDictionary = meterDictionary.Select(m => m.Value).Distinct()
                    .ToDictionary(m => m, m => mappingMethods.ContractMeterToMeter_GetContractMeterIdListByMeterId(m));

                foreach(var flexContractEntity in commitableFlexContractEntities)
                {
                    //Get BasketId from [Customer].[BasketDetail] by BasketReference
                    var basketId = basketDictionary[flexContractEntity.BasketReference];

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meterDictionary[flexContractEntity.MPXN];

                    //Get ContractIdList from [Customer].[Contract] by ContractReference
                    var customerContractIdList = contractReferenceDictionary[flexContractEntity.ContractReference];

                    //Get ContractId from intersect of Customer ContractIdList and Mapping ContractIdList
                    var contractId = customerContractIdList.Intersect(contractIdList).FirstOrDefault();

                    //Get ContractMeterIdList from [Mapping].[ContractMeterToMeter] by MeterId
                    var contractMeterToMeterContractMeterIdList = contractMeterDictionary[meterId];

                    //Get ContractMeterIdList from [Mapping].[ContractToContractMeter] by ContractId
                    var contractToContractMeterContractMeterIdList = mappingMethods.ContractToContractMeter_GetContractMeterIdListByContractId(contractId);

                    //Get ContractMeterId from intersect of ContractMeterToMeter ContractMeterIdList and ContractToContractMeter ContractMeterIdList
                    var contractMeterId = contractMeterToMeterContractMeterIdList.Intersect(contractToContractMeterContractMeterIdList).FirstOrDefault();

                    //Get existing BasketToContractMeter Id
                    var basketToContractMeterId = mappingMethods.BasketToContractMeter_GetBasketToContractMeterIdByBasketIdAndContractMeterId(basketId, contractMeterId);

                    if(basketToContractMeterId == 0)
                    {
                        //Insert into [Mapping].[BasketToContractMeter]
                        mappingMethods.BasketToContractMeter_Insert(createdByUserId, sourceId, basketId, contractMeterId);
                    }
                }

                //Update Process Queue
                systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitBasketDataAPIId, false, null);
            }
            catch(Exception error)
            {
                var errorId = new Methods.SystemSchema().InsertSystemError(1, 1, error);

                //Update Process Queue
                //systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitBasketDataAPIId, true, $"System Error Id {errorId}");
            }
        }
    }
}
