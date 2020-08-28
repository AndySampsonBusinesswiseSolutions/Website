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
        private readonly Methods.Supplier _supplierMethods = new Methods.Supplier();
        private static readonly Enums.System.API.Name _systemAPINameEnums = new Enums.System.API.Name();
        private static readonly Enums.System.API.Password _systemAPIPasswordEnums = new Enums.System.API.Password();
        private static readonly Enums.System.API.GUID _systemAPIGUIDEnums = new Enums.System.API.GUID();
        private readonly Enums.System.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.System.API.RequiredDataKey();
        private readonly Enums.Customer.Meter.Attribute _customerMeterAttributeEnums = new Enums.Customer.Meter.Attribute();
        private readonly Enums.Information.ContractType _informationContractTypeEnums = new Enums.Information.ContractType();
        private readonly Enums.Information.RateType _informationRateTypeEnums = new Enums.Information.RateType();
        private readonly Enums.Supplier.Attribute _supplierAttributeEnums = new Enums.Supplier.Attribute();
        private readonly Enums.Supplier.Product.Attribute _supplierProductAttributeEnums = new Enums.Supplier.Product.Attribute();
        private readonly Enums.Customer.Contract.Attribute _customerContractAttributeEnums = new Enums.Customer.Contract.Attribute();
        private readonly Enums.Customer.Basket.Attribute _customerBasketAttributeEnums = new Enums.Customer.Basket.Attribute();
        private readonly Enums.Customer.ContractMeterRate.Attribute _customerContractMeterRateAttributeEnums = new Enums.Customer.ContractMeterRate.Attribute();
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

                if (!_systemMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitContractDataAPI, commitContractDataAPIId, jsonObject))
                {
                    return;
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveFromDateTime(processQueueGUID, commitContractDataAPIId);

                //Get ContractType from jsonObject
                var contractType = jsonObject[_systemAPIRequiredDataKeyEnums.ContractType].ToString();

                //Get Contract Data
                var contractDataJSON = jsonObject[_systemAPIRequiredDataKeyEnums.ContractData].ToString();
                var contractDataArray = contractDataJSON.Split(";;", StringSplitOptions.RemoveEmptyEntries);

                //Create DataTable
                var dataTable = CreateDataTable(contractType);

                //Populate DataTable
                foreach(var contractDataRow in contractDataArray)
                {
                    var dataRow = dataTable.NewRow();
                    var contractDataRowArray = contractDataRow.Split("|");

                    for(var itemCount = 0; itemCount < contractDataRowArray.Count(); itemCount++)
                    {
                        dataRow[itemCount] = contractDataRowArray[itemCount];
                    }

                    dataTable.Rows.Add(dataRow);
                }

                //Get ContractTypeId from [Information].[ContractType]
                var contractTypeId = _informationMethods.ContractType_GetContractTypeIdByContractTypeDescription(contractType);

                //Setup AttributeId Dictionary
                var attributeIdDictionary = new Dictionary<string, long>
                {
                    {_customerContractAttributeEnums.ContractReference, _customerMethods.ContractAttribute_GetContractAttributeIdByContractAttributeDescription(_customerContractAttributeEnums.ContractReference)},
                    {_customerMeterAttributeEnums.MeterIdentifier, _customerMethods.MeterAttribute_GetMeterAttributeIdByMeterAttributeDescription(_customerMeterAttributeEnums.MeterIdentifier)},
                    {_supplierAttributeEnums.SupplierName, _supplierMethods.SupplierAttribute_GetSupplierAttributeIdBySupplierAttributeDescription(_supplierAttributeEnums.SupplierName)},
                    {_supplierAttributeEnums.SupplierAlsoKnownAs, _supplierMethods.SupplierAttribute_GetSupplierAttributeIdBySupplierAttributeDescription(_supplierAttributeEnums.SupplierAlsoKnownAs)},
                    {_customerContractMeterAttributeEnums.ContractStartDate, _customerMethods.ContractMeterAttribute_GetContractMeterAttributeIdByContractMeterAttributeDescription(_customerContractMeterAttributeEnums.ContractStartDate)},
                    {_customerContractMeterAttributeEnums.ContractEndDate, _customerMethods.ContractMeterAttribute_GetContractMeterAttributeIdByContractMeterAttributeDescription(_customerContractMeterAttributeEnums.ContractEndDate)},
                    {_customerContractMeterAttributeEnums.RateCount, _customerMethods.ContractMeterAttribute_GetContractMeterAttributeIdByContractMeterAttributeDescription(_customerContractMeterAttributeEnums.RateCount)},
                    {_supplierProductAttributeEnums.ProductName, _supplierMethods.ProductAttribute_GetProductAttributeIdByProductAttributeDescription(_supplierProductAttributeEnums.ProductName)},
                    {_customerContractMeterRateAttributeEnums.RateValue, _customerMethods.ContractMeterRateAttribute_GetContractMeterRateAttributeIdByContractMeterRateAttributeDescription(_customerContractMeterRateAttributeEnums.RateValue)},
                    {_customerBasketAttributeEnums.BasketReference, _customerMethods.BasketAttribute_GetBasketAttributeIdByBasketAttributeDescription(_customerBasketAttributeEnums.BasketReference)},
                };

                var dataRows = dataTable.Rows.Cast<DataRow>().ToList();

                var contracts = dataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference))
                    .Distinct()
                    .ToDictionary(c => c, c => GetContractId(createdByUserId, sourceId, attributeIdDictionary[_customerContractAttributeEnums.ContractReference], c));

                var meters = dataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.MPXN))
                    .Distinct()
                    .ToDictionary(m => m, m => _customerMethods.MeterDetail_GetMeterIdListByMeterAttributeIdAndMeterDetailDescription(attributeIdDictionary[_customerMeterAttributeEnums.MeterIdentifier], m).FirstOrDefault());

                var suppliers = dataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.Supplier))
                    .Distinct()
                    .ToDictionary(s => s, s => GetSupplierId(attributeIdDictionary, s));

                var contractMeterRates = dataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.Value))
                    .Distinct()
                    .ToDictionary(cmr => cmr, cmr => GetContractMeterRateId(createdByUserId, sourceId, attributeIdDictionary[_customerContractMeterRateAttributeEnums.RateValue], cmr));

                var rateTypes = dataRows.Select(r => r.Field<string>(_customerDataUploadValidationEntityEnums.RateType))
                    .Distinct()
                    .ToDictionary(rt => rt, rt => _informationMethods.RateType_GetRateTypeIdByRateTypeCode(rt));

                foreach (var dataRow in dataRows)
                {
                    //Get ContractId from [Customer].[ContractDetail] by ContractReference
                    var contractId = contracts[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractReference)];

                    //Get MeterId from [Customer].[MeterDetail] by MPXN
                    var meterId = meters[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.MPXN)];

                    //Get SupplierId from [Supplier].[SupplierDetail] by SupplierName
                    var supplierId = suppliers[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Supplier)];

                    //Get ContractMeterId from [Customer].[ContractMeterDetail] by ContractStartDate, ContractEndDate and RateCount
                    var contractMeterId = GetContractMeterId(createdByUserId, sourceId, contractType, dataRow, attributeIdDictionary);
                    _mappingMethods.ContractToContractMeter_Insert(createdByUserId, sourceId, contractId, contractMeterId);
                    _mappingMethods.ContractMeterToMeter_Insert(createdByUserId, sourceId, contractMeterId, meterId);

                    //Get ProductId from [Product].[ProductDetail] by ProductName
                    var supplierProductId = GetProductId(attributeIdDictionary, dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Product));

                    //Get ContractMeterRateId from [Customer].[ContractMeterRateDetail] by Value
                    var contractMeterRateId = contractMeterRates[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.Value)];

                    //Get RateTypeId from [Information].[RateType] by RateType
                    var rateTypeId = rateTypes[dataRow.Field<string>(_customerDataUploadValidationEntityEnums.RateType)];

                    if (contractType == _informationContractTypeEnums.Flex)
                    {
                        //Get BasketId from [Customer].[BasketDetail] by BasketReference
                        var basketId = GetBasketId(createdByUserId, sourceId, attributeIdDictionary, dataRow.Field<string>(_customerDataUploadValidationEntityEnums.BasketReference));

                        //Insert into [Mapping].[BasketToContractMeter]
                        InsertBasketToContractMeter(createdByUserId, sourceId, basketId, contractMeterId);
                    }

                    //Insert into [Mapping].[ContractToContractType] - 1
                    InsertContractToContractType(createdByUserId, sourceId, contractId, contractTypeId);

                    //Insert into [Mapping].[ContractToMeter] - 2
                    var contractToMeterId = GetContractToMeterId(createdByUserId, sourceId, contractId, meterId);

                    //Insert into [Mapping].[ContractToSupplier] - 3
                    InsertContractToSupplier(createdByUserId, sourceId, contractId, supplierId);

                    //Insert into [Mapping].[ContractMeterToProduct] - 4
                    var contractMeterToProductId = GetContractMeterToProductId(createdByUserId, sourceId, contractMeterId, supplierProductId);

                    //Insert into [Mapping].[ContractMeterRateToRateType] - 5
                    var contractMeterRateToRateTypeId = GetContractMeterRateToRateTypeId(createdByUserId, sourceId, contractMeterRateId, rateTypeId);

                    //Insert into [Mapping].[ContractToMeterToContractMeterToProduct] - 6 (2 to 4)
                    var contractToMeterToContractMeterToProductId = GetContractToMeterToContractMeterToProductId(createdByUserId, sourceId, contractToMeterId, contractMeterToProductId);

                    //Insert into [Mapping].[ContractToMeterToContractMeterToProductToContractMeterRateToRateType] - 7 (6 to 5)
                    InsertContractToMeterToContractMeterToProductToContractMeterRateToRateType(createdByUserId, sourceId, contractToMeterToContractMeterToProductId, contractMeterRateToRateTypeId);
                }

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitContractDataAPIId, false, null);
            }
            catch (Exception error)
            {
                var errorId = _systemMethods.InsertSystemError(createdByUserId, sourceId, error);

                //Update Process Queue
                _systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private DataTable CreateDataTable(string contractType)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ProcessQueueGUID", typeof(string));
            dataTable.Columns.Add("RowId", typeof(string));
            dataTable.Columns.Add("ContractReference", typeof(string));

            if (contractType == _informationContractTypeEnums.Flex)
            {
                dataTable.Columns.Add("BasketReference", typeof(string));
            }

            dataTable.Columns.Add("MPXN", typeof(string));
            dataTable.Columns.Add("Supplier", typeof(string));
            dataTable.Columns.Add("ContractStartDate", typeof(string));
            dataTable.Columns.Add("ContractEndDate", typeof(string));
            dataTable.Columns.Add("Product", typeof(string));

            if (contractType == _informationContractTypeEnums.Fixed)
            {
                dataTable.Columns.Add("RateCount", typeof(string));
            }

            dataTable.Columns.Add("RateType", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));
            dataTable.Columns.Add("CanCommit", typeof(string));

            return dataTable;
        }

        private void InsertContractToMeterToContractMeterToProductToContractMeterRateToRateType(long createdByUserId, long sourceId, long contractToMeterToContractMeterToProductId, long contractMeterRateToRateTypeId)
        {
            var contractToMeterToContractMeterToProductToContractMeterRateToRateTypeId = _mappingMethods.ContractToMeterToContractMeterToProductToContractMeterRateToRateType_GetContractToMeterToContractMeterToProductToContractMeterRateToRateTypeIdByBasketIdAndContractMeterId(contractToMeterToContractMeterToProductId, contractMeterRateToRateTypeId);

            if(contractToMeterToContractMeterToProductToContractMeterRateToRateTypeId == 0)
            {
                _mappingMethods.ContractToMeterToContractMeterToProductToContractMeterRateToRateType_Insert(createdByUserId, sourceId, contractToMeterToContractMeterToProductId, contractMeterRateToRateTypeId);
            }
        }

        private void InsertBasketToContractMeter(long createdByUserId, long sourceId, long basketId, long contractMeterId)
        {
            var basketToContractMeterId = _mappingMethods.BasketToContractMeter_GetBasketToContractMeterIdByBasketIdAndContractMeterId(basketId, contractMeterId);

            if(basketToContractMeterId == 0)
            {
                _mappingMethods.BasketToContractMeter_Insert(createdByUserId, sourceId, basketId, contractMeterId);
            }
        }

        private long GetContractToMeterToContractMeterToProductId(long createdByUserId, long sourceId, long contractToMeterId, long contractMeterToProductId)
        {
            var contractToMeterToContractMeterToProductId = _mappingMethods.ContractToMeterToContractMeterToProduct_GetContractToMeterToContractMeterToProductIdByContractToMeterIdAndContractMeterToProductId(contractToMeterId, contractMeterToProductId);

            if(contractToMeterToContractMeterToProductId == 0)
            {
                _mappingMethods.ContractToMeterToContractMeterToProduct_Insert(createdByUserId, sourceId, contractToMeterId, contractMeterToProductId);
                contractToMeterToContractMeterToProductId = _mappingMethods.ContractToMeterToContractMeterToProduct_GetContractToMeterToContractMeterToProductIdByContractToMeterIdAndContractMeterToProductId(contractToMeterId, contractMeterToProductId);
            }

            return contractToMeterToContractMeterToProductId;
        }

        private long GetContractMeterRateToRateTypeId(long createdByUserId, long sourceId, long contractMeterRateId, long rateTypeId)
        {
            var contractMeterRateToRateTypeId = _mappingMethods.ContractMeterRateToRateType_GetContractMeterRateToRateTypeIdByContractMeterRateIdAndRateTypeId(contractMeterRateId, rateTypeId);

            if(contractMeterRateToRateTypeId == 0)
            {
                _mappingMethods.ContractMeterRateToRateType_Insert(createdByUserId, sourceId, contractMeterRateId, rateTypeId);
                contractMeterRateToRateTypeId = _mappingMethods.ContractMeterRateToRateType_GetContractMeterRateToRateTypeIdByContractMeterRateIdAndRateTypeId(contractMeterRateId, rateTypeId);
            }

            return contractMeterRateToRateTypeId;
        }

        private long GetContractMeterToProductId(long createdByUserId, long sourceId, long contractMeterId, long productId)
        {
            var contractMeterToProductId = _mappingMethods.ContractMeterToProduct_GetContractMeterToProductIdByContractMeterIdAndProductId(contractMeterId, productId);

            if(contractMeterToProductId == 0)
            {
                _mappingMethods.ContractMeterToProduct_Insert(createdByUserId, sourceId, contractMeterId, productId);
                contractMeterToProductId = _mappingMethods.ContractMeterToProduct_GetContractMeterToProductIdByContractMeterIdAndProductId(contractMeterId, productId);
            }

            return contractMeterToProductId;
        }

        private void InsertContractToSupplier(long createdByUserId, long sourceId, long contractId, long supplierId)
        {
            var contractToSupplierId = _mappingMethods.ContractToSupplier_GetContractToSupplierIdByContractIdAndSupplierId(contractId, supplierId);

            if(contractToSupplierId == 0)
            {
                _mappingMethods.ContractToSupplier_Insert(createdByUserId, sourceId, contractId, supplierId);
            }
        }

        private long GetContractToMeterId(long createdByUserId, long sourceId, long contractId, long meterId)
        {
            var contractToMeterId = _mappingMethods.ContractToMeter_GetContractToMeterIdByContractIdAndMeterId(contractId, meterId);

            if(contractToMeterId == 0)
            {
                _mappingMethods.ContractToMeter_Insert(createdByUserId, sourceId, contractId, meterId);
                contractToMeterId = _mappingMethods.ContractToMeter_GetContractToMeterIdByContractIdAndMeterId(contractId, meterId);
            }

            return contractToMeterId;
        }

        private void InsertContractToContractType(long createdByUserId, long sourceId, long contractId, long contractTypeId)
        {
            var contractToContractTypeId = _mappingMethods.ContractToContractType_GetContractToContractTypeIdByContractIdAndContractTypeId(contractId, contractTypeId);

            if(contractToContractTypeId == 0)
            {
                _mappingMethods.ContractToContractType_Insert(createdByUserId, sourceId, contractId, contractTypeId);
            }
        }

        private long GetBasketId(long createdByUserId, long sourceId, Dictionary<string, long> attributeIdDictionary, string basketReference)
        {
            var basketReferenceBasketAttributeId = attributeIdDictionary[_customerBasketAttributeEnums.BasketReference];
            var basketId = _customerMethods.BasketDetail_GetBasketIdByBasketAttributeIdAndBasketDetailDescription(basketReferenceBasketAttributeId, basketReference);

            if (basketId == 0)
            {
                basketId = _customerMethods.InsertNewBasket(createdByUserId, sourceId);

                //Insert into [Customer].[BasketDetail]
                _customerMethods.BasketDetail_Insert(createdByUserId, sourceId, basketId, basketReferenceBasketAttributeId, basketReference);
            }

            return basketId;
        }

        private long GetContractMeterRateId(long createdByUserId, long sourceId, long contractMeterRateValueContractMeterRateAttributeId, string contractMeterRateValue)
        {
            var contractMeterRateId = _customerMethods.ContractMeterRateDetail_GetContractMeterRateIdByContractMeterRateAttributeIdAndContractMeterRateDetailDescription(contractMeterRateValueContractMeterRateAttributeId, contractMeterRateValue);

            if (contractMeterRateId == 0)
            {
                contractMeterRateId = _customerMethods.InsertNewContractMeterRate(createdByUserId, sourceId);

                //Insert into [Customer].[ContractMeterRateDetail]
                _customerMethods.ContractMeterRateDetail_Insert(createdByUserId, sourceId, contractMeterRateId, contractMeterRateValueContractMeterRateAttributeId, contractMeterRateValue);
            }

            return contractMeterRateId;
        }

        private long GetProductId(Dictionary<string, long> attributeIdDictionary, string product)
        {
            return _supplierMethods.ProductDetail_GetProductIdByProductAttributeIdAndProductDetailDescription(attributeIdDictionary[_supplierProductAttributeEnums.ProductName], product);
        }

        private long GetSupplierId(Dictionary<string, long> attributeIdDictionary, string supplier)
        {
            var supplierId = _supplierMethods.SupplierDetail_GetSupplierIdBySupplierAttributeIdAndSupplierDetailDescription(attributeIdDictionary[_supplierAttributeEnums.SupplierName], supplier);

            if(supplierId == 0)
            {
                supplierId = _supplierMethods.SupplierDetail_GetSupplierIdBySupplierAttributeIdAndSupplierDetailDescription(attributeIdDictionary[_supplierAttributeEnums.SupplierAlsoKnownAs], supplier);
            }

            return supplierId;
        }

        private long GetContractMeterId(long createdByUserId, long sourceId, string contractType, DataRow dataRow, Dictionary<string, long> attributeIdDictionary)
        {
            var contractStartDate = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractStartDate);
            var contractEndDate = dataRow.Field<string>(_customerDataUploadValidationEntityEnums.ContractEndDate);
            var rateCount = contractType == _informationContractTypeEnums.Fixed
                ? dataRow.Field<string>(_customerDataUploadValidationEntityEnums.RateCount)
                : "1";

            var contractStartDateContractMeterAttributeId = attributeIdDictionary[_customerContractMeterAttributeEnums.ContractStartDate];
            var contractEndDateContractMeterAttributeId = attributeIdDictionary[_customerContractMeterAttributeEnums.ContractEndDate];
            var rateCountContractMeterAttributeId = attributeIdDictionary[_customerContractMeterAttributeEnums.RateCount];

            var contractMeterId = 0L;
            var contractStartDateContractMeterIdList = _customerMethods.ContractMeterDetail_GetContractMeterIdListByContractMeterAttributeIdAndContractMeterDetailDescription(contractStartDateContractMeterAttributeId, contractStartDate);
            if (contractStartDateContractMeterIdList.Any())
            {
                var contractEndDateContractMeterIdList = _customerMethods.ContractMeterDetail_GetContractMeterIdListByContractMeterAttributeIdAndContractMeterDetailDescription(contractEndDateContractMeterAttributeId, contractEndDate);
                if (contractEndDateContractMeterIdList.Any())
                {
                    var rateCountContractMeterIdList = _customerMethods.ContractMeterDetail_GetContractMeterIdListByContractMeterAttributeIdAndContractMeterDetailDescription(rateCountContractMeterAttributeId, rateCount);
                    if (rateCountContractMeterIdList.Any())
                    {
                        contractMeterId = contractStartDateContractMeterIdList.Intersect(contractEndDateContractMeterIdList).Intersect(rateCountContractMeterIdList).FirstOrDefault();
                    }
                }
            }

            if (contractMeterId == 0)
            {
                contractMeterId = _customerMethods.InsertNewContractMeter(createdByUserId, sourceId);

                //Insert into [Customer].[ContractMeterDetail]
                _customerMethods.ContractMeterDetail_Insert(createdByUserId, sourceId, contractMeterId, contractStartDateContractMeterAttributeId, contractStartDate);
                _customerMethods.ContractMeterDetail_Insert(createdByUserId, sourceId, contractMeterId, contractEndDateContractMeterAttributeId, contractEndDate);
                _customerMethods.ContractMeterDetail_Insert(createdByUserId, sourceId, contractMeterId, rateCountContractMeterAttributeId, rateCount);
            }

            return contractMeterId;
        }

        private long GetContractId(long createdByUserId, long sourceId, long contractReferenceContractAttributeId, string contractReference)
        {
            var contractId = _customerMethods.ContractDetail_GetContractIdListByContractAttributeIdAndContractDetailDescription(contractReferenceContractAttributeId, contractReference).FirstOrDefault();

            if (contractId == 0)
            {
                contractId = _customerMethods.InsertNewContract(createdByUserId, sourceId);

                //Insert into [Customer].[ContractDetail]
                _customerMethods.ContractDetail_Insert(createdByUserId, sourceId, contractId, contractReferenceContractAttributeId, contractReference);
            }

            return contractId;
        }
    }
}

