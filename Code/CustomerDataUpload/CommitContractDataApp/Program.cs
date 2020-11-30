using MethodLibrary;
using enums;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace CommitContractDataApp
{
    class Program
    {
        private static readonly Methods.SystemSchema _systemMethods = new Methods.SystemSchema();
        private static readonly Methods.SystemSchema.API _systemAPIMethods = new Methods.SystemSchema.API();
        private static readonly Methods.InformationSchema _informationMethods = new Methods.InformationSchema();
        private static readonly Methods.CustomerSchema _customerMethods = new Methods.CustomerSchema();
        private static readonly Methods.MappingSchema _mappingMethods = new Methods.MappingSchema();
        private static readonly Methods.SupplierSchema _supplierMethods = new Methods.SupplierSchema();
        private static readonly Enums.SystemSchema.API.GUID _systemAPIGUIDEnums = new Enums.SystemSchema.API.GUID();
        private static readonly Enums.SystemSchema.API.RequiredDataKey _systemAPIRequiredDataKeyEnums = new Enums.SystemSchema.API.RequiredDataKey();
        private static readonly Enums.CustomerSchema.Meter.Attribute _customerMeterAttributeEnums = new Enums.CustomerSchema.Meter.Attribute();
        private static readonly Enums.InformationSchema.ContractType _informationContractTypeEnums = new Enums.InformationSchema.ContractType();
        private static readonly Enums.SupplierSchema.Attribute _supplierAttributeEnums = new Enums.SupplierSchema.Attribute();
        private static readonly Enums.SupplierSchema.Product.Attribute _supplierProductAttributeEnums = new Enums.SupplierSchema.Product.Attribute();
        private static readonly Enums.CustomerSchema.Contract.Attribute _customerContractAttributeEnums = new Enums.CustomerSchema.Contract.Attribute();
        private static readonly Enums.CustomerSchema.Basket.Attribute _customerBasketAttributeEnums = new Enums.CustomerSchema.Basket.Attribute();
        private static readonly Enums.CustomerSchema.ContractMeterRate.Attribute _customerContractMeterRateAttributeEnums = new Enums.CustomerSchema.ContractMeterRate.Attribute();
        private static readonly Enums.CustomerSchema.ContractMeter.Attribute _customerContractMeterAttributeEnums = new Enums.CustomerSchema.ContractMeter.Attribute();
        private static readonly Enums.CustomerSchema.DataUploadValidation.Entity _customerDataUploadValidationEntityEnums = new Enums.CustomerSchema.DataUploadValidation.Entity();

        static void Main(string[] args)
        {
            try
            {
                var password = "wsxbn8B2jTb9bDFM";
                var hostEnvironment = "Development";

                new Methods().InitialiseDatabaseInteraction(hostEnvironment, new Enums.SystemSchema.API.Name().CommitContractDataAPI, password);
                var commitContractDataAPIId = _systemAPIMethods.API_GetAPIIdByAPIGUID(_systemAPIGUIDEnums.CommitContractDataAPI);

                //Get base variables
                var createdByUserId = new Methods.AdministrationSchema.User().GetSystemUserId();
                var sourceId = _informationMethods.GetSystemUserGeneratedSourceId();

                //Get Queue GUID
                var jsonObject = JObject.Parse(args[0]);
                var processQueueGUID = _systemMethods.GetProcessQueueGUIDFromJObject(jsonObject);
            
                //Insert into ProcessQueue
                _systemMethods.ProcessQueue_Insert(
                    processQueueGUID,
                    createdByUserId,
                    sourceId,
                    commitContractDataAPIId);

                if (!_systemAPIMethods.PrerequisiteAPIsAreSuccessful(_systemAPIGUIDEnums.CommitContractDataAPI, commitContractDataAPIId, hostEnvironment, jsonObject))
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
                //TODO: Work out how to do this with entities
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
                var errorId = _systemMethods.InsertSystemError(1, 1, error);

                //Update Process Queue
                //_systemMethods.ProcessQueue_UpdateEffectiveToDateTime(processQueueGUID, commitContractDataAPIId, true, $"System Error Id {errorId}");
            }
        }

        private static DataTable CreateDataTable(string contractType)
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

        private static void InsertContractToMeterToContractMeterToProductToContractMeterRateToRateType(long createdByUserId, long sourceId, long contractToMeterToContractMeterToProductId, long contractMeterRateToRateTypeId)
        {
            var contractToMeterToContractMeterToProductToContractMeterRateToRateTypeId = _mappingMethods.ContractToMeterToContractMeterToProductToContractMeterRateToRateType_GetContractToMeterToContractMeterToProductToContractMeterRateToRateTypeIdByBasketIdAndContractMeterId(contractToMeterToContractMeterToProductId, contractMeterRateToRateTypeId);

            if(contractToMeterToContractMeterToProductToContractMeterRateToRateTypeId == 0)
            {
                _mappingMethods.ContractToMeterToContractMeterToProductToContractMeterRateToRateType_Insert(createdByUserId, sourceId, contractToMeterToContractMeterToProductId, contractMeterRateToRateTypeId);
            }
        }

        private static void InsertBasketToContractMeter(long createdByUserId, long sourceId, long basketId, long contractMeterId)
        {
            var basketToContractMeterId = _mappingMethods.BasketToContractMeter_GetBasketToContractMeterIdByBasketIdAndContractMeterId(basketId, contractMeterId);

            if(basketToContractMeterId == 0)
            {
                _mappingMethods.BasketToContractMeter_Insert(createdByUserId, sourceId, basketId, contractMeterId);
            }
        }

        private static long GetContractToMeterToContractMeterToProductId(long createdByUserId, long sourceId, long contractToMeterId, long contractMeterToProductId)
        {
            var contractToMeterToContractMeterToProductId = _mappingMethods.ContractToMeterToContractMeterToProduct_GetContractToMeterToContractMeterToProductIdByContractToMeterIdAndContractMeterToProductId(contractToMeterId, contractMeterToProductId);

            if(contractToMeterToContractMeterToProductId == 0)
            {
                _mappingMethods.ContractToMeterToContractMeterToProduct_Insert(createdByUserId, sourceId, contractToMeterId, contractMeterToProductId);
                contractToMeterToContractMeterToProductId = _mappingMethods.ContractToMeterToContractMeterToProduct_GetContractToMeterToContractMeterToProductIdByContractToMeterIdAndContractMeterToProductId(contractToMeterId, contractMeterToProductId);
            }

            return contractToMeterToContractMeterToProductId;
        }

        private static long GetContractMeterRateToRateTypeId(long createdByUserId, long sourceId, long contractMeterRateId, long rateTypeId)
        {
            var contractMeterRateToRateTypeId = _mappingMethods.ContractMeterRateToRateType_GetContractMeterRateToRateTypeIdByContractMeterRateIdAndRateTypeId(contractMeterRateId, rateTypeId);

            if(contractMeterRateToRateTypeId == 0)
            {
                _mappingMethods.ContractMeterRateToRateType_Insert(createdByUserId, sourceId, contractMeterRateId, rateTypeId);
                contractMeterRateToRateTypeId = _mappingMethods.ContractMeterRateToRateType_GetContractMeterRateToRateTypeIdByContractMeterRateIdAndRateTypeId(contractMeterRateId, rateTypeId);
            }

            return contractMeterRateToRateTypeId;
        }

        private static long GetContractMeterToProductId(long createdByUserId, long sourceId, long contractMeterId, long productId)
        {
            var contractMeterToProductId = _mappingMethods.ContractMeterToProduct_GetContractMeterToProductIdByContractMeterIdAndProductId(contractMeterId, productId);

            if(contractMeterToProductId == 0)
            {
                _mappingMethods.ContractMeterToProduct_Insert(createdByUserId, sourceId, contractMeterId, productId);
                contractMeterToProductId = _mappingMethods.ContractMeterToProduct_GetContractMeterToProductIdByContractMeterIdAndProductId(contractMeterId, productId);
            }

            return contractMeterToProductId;
        }

        private static void InsertContractToSupplier(long createdByUserId, long sourceId, long contractId, long supplierId)
        {
            var contractToSupplierId = _mappingMethods.ContractToSupplier_GetContractToSupplierIdByContractIdAndSupplierId(contractId, supplierId);

            if(contractToSupplierId == 0)
            {
                _mappingMethods.ContractToSupplier_Insert(createdByUserId, sourceId, contractId, supplierId);
            }
        }

        private static long GetContractToMeterId(long createdByUserId, long sourceId, long contractId, long meterId)
        {
            var contractToMeterId = _mappingMethods.ContractToMeter_GetContractToMeterIdByContractIdAndMeterId(contractId, meterId);

            if(contractToMeterId == 0)
            {
                _mappingMethods.ContractToMeter_Insert(createdByUserId, sourceId, contractId, meterId);
                contractToMeterId = _mappingMethods.ContractToMeter_GetContractToMeterIdByContractIdAndMeterId(contractId, meterId);
            }

            return contractToMeterId;
        }

        private static void InsertContractToContractType(long createdByUserId, long sourceId, long contractId, long contractTypeId)
        {
            var contractToContractTypeId = _mappingMethods.ContractToContractType_GetContractToContractTypeIdByContractIdAndContractTypeId(contractId, contractTypeId);

            if(contractToContractTypeId == 0)
            {
                _mappingMethods.ContractToContractType_Insert(createdByUserId, sourceId, contractId, contractTypeId);
            }
        }

        private static long GetBasketId(long createdByUserId, long sourceId, Dictionary<string, long> attributeIdDictionary, string basketReference)
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

        private static long GetContractMeterRateId(long createdByUserId, long sourceId, long contractMeterRateValueContractMeterRateAttributeId, string contractMeterRateValue)
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

        private static long GetProductId(Dictionary<string, long> attributeIdDictionary, string product)
        {
            return _supplierMethods.ProductDetail_GetProductIdByProductAttributeIdAndProductDetailDescription(attributeIdDictionary[_supplierProductAttributeEnums.ProductName], product);
        }

        private static long GetSupplierId(Dictionary<string, long> attributeIdDictionary, string supplier)
        {
            var supplierId = _supplierMethods.SupplierDetail_GetSupplierIdBySupplierAttributeIdAndSupplierDetailDescription(attributeIdDictionary[_supplierAttributeEnums.SupplierName], supplier);

            if(supplierId == 0)
            {
                supplierId = _supplierMethods.SupplierDetail_GetSupplierIdBySupplierAttributeIdAndSupplierDetailDescription(attributeIdDictionary[_supplierAttributeEnums.SupplierAlsoKnownAs], supplier);
            }

            return supplierId;
        }

        private static long GetContractMeterId(long createdByUserId, long sourceId, string contractType, DataRow dataRow, Dictionary<string, long> attributeIdDictionary)
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

        private static long GetContractId(long createdByUserId, long sourceId, long contractReferenceContractAttributeId, string contractReference)
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
