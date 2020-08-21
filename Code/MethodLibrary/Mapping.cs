using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public class Mapping
        {
            public List<long> APIToProcess_GetAPIIdListByProcessId(long processId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.APIToProcess_GetByProcessId, 
                    processId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("APIId"))
                    .ToList();
            }

            public long PasswordToUser_GetPasswordFromJObjectToUserIdByPasswordIdAndUserId(long passwordId, long userId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.PasswordToUser_GetByPasswordIdAndUserId, 
                    passwordId, userId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("PasswordToUserId"))
                    .FirstOrDefault();
            }

            public void LoginToUser_Insert(long createdByUserId, long sourceId, long loginId, long userId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.LoginToUser_Insert, 
                    createdByUserId, sourceId, loginId, userId);
            }

            public List<long> LoginToUser_GetLoginIdListByUserId(long userId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.LoginToUser_GetByUserId, 
                    userId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("LoginId"))
                    .ToList();
            }

            public void ProcessToProcessArchive_Insert(long createdByUserId, long sourceId, long processId, long processArchiveId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ProcessToProcessArchive_Insert, 
                    createdByUserId, sourceId, processId, processArchiveId);
            }

            public List<long> APIToProcessArchiveDetail_GetProcessArchiveDetailIdListByAPIId(long APIId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.APIToProcessArchiveDetail_GetByAPIId, 
                    APIId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessArchiveDetailId"))
                    .ToList();
            }

            public void APIToProcessArchiveDetail_Insert(long createdByUserId, long sourceId, long APIId, long processArchiveDetailId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.APIToProcessArchiveDetail_Insert, 
                    createdByUserId, sourceId, APIId, processArchiveDetailId);
            }

            public Dictionary<long, List<long>> CustomerToChildCustomer_GetCustomerIdToChildCustomerIdDictionary()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CustomerToChildCustomer_GetList);
                
                var dictionary = new Dictionary<long, List<long>>();

                foreach(DataRow r in dataTable.Rows)
                {
                    if(!dictionary.ContainsKey(r.Field<long>("CustomerId")))
                    {
                        dictionary.Add(r.Field<long>("CustomerId"), new List<long>());
                    }

                    dictionary[r.Field<long>("CustomerId")].Add(r.Field<long>("ChildCustomerId"));
                }

                return dictionary;
            }

            public List<long> CustomerToChildCustomer_GetChildCustomerIdListByCustomerId(long customerId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CustomerToChildCustomer_GetByCustomerId, 
                    customerId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ChildCustomerId"))
                    .ToList();
            }

            public void CustomerToChildCustomer_DeleteByCustomerIdAndChildCustomerId(long customerId, long childCustomerId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomertoChildCustomer_DeleteByCustomerIdAndChildCustomerId, 
                    customerId, childCustomerId);
            }

            public void CustomerToChildCustomer_Insert(long createdByUserId, long sourceId, long customerId, long childCustomerId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomerToChildCustomer_Insert, 
                    createdByUserId, sourceId, customerId, childCustomerId);
            }

            public void CustomerToFile_Insert(long createdByUserId, long sourceId, long customerId, long fileId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomerToFile_Insert, 
                    createdByUserId, sourceId, customerId, fileId);
            }

            public void FileToFileType_Insert(long createdByUserId, long sourceId, long fileId, long fileTypeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.FileToFileType_Insert, 
                    createdByUserId, sourceId, fileId, fileTypeId);
            }

            public List<long> ContractToContractMeter_GetContractMeterIdListByContractId(long contractId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToContractMeter_GetByContractId, 
                    contractId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterId"))
                    .ToList();
            }

            public List<long> ContractMeterToMeter_GetContractMeterIdListByMeterId(long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractMeterToMeter_GetByContractMeterId, 
                    meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterId"))
                    .ToList();
            }

            public List<long> BasketToContractMeter_GetContractMeterIdListByBasketId(long basketId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.BasketToContractMeter_GetByBasketId, 
                    basketId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterId"))
                    .ToList();
            }

            public long FileTypeToProcess_GetProcessIdByFileTypeId(long fileTypeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.FileTypeToProcess_GetByFileTypeId, 
                    fileTypeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessId"))
                    .FirstOrDefault();
            }

            public void DataUploadValidationErrorToFile_Insert(long createdByUserId, long sourceId, long dataUploadValidationErrorId, long fileId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.DataUploadValidationErrorToFile_Insert, 
                    createdByUserId, sourceId, dataUploadValidationErrorId, fileId);
            }

            public void MeterToMeterExemption_Insert(long createdByUserId, long sourceId, long meterId, long meterExemptionId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToMeterExemption_Insert, 
                    createdByUserId, sourceId, meterId, meterExemptionId);
            }

            public long MeterToMeterExemption_GetMeterToMeterExemptionIdByMeterIdAndMeterExemptionId(long meterId, long meterExemptionId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.MeterToMeterExemption_GetByMeterIdAndMeterExemptionId, 
                    meterId, meterExemptionId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterToMeterExemptionId"))
                    .FirstOrDefault();
            }

            public void MeterExemptionToMeterExemptionProduct_Insert(long createdByUserId, long sourceId, long meterExemptionId, long meterExemptionProductId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterExemptionToMeterExemptionProduct_Insert, 
                    createdByUserId, sourceId, meterExemptionId, meterExemptionProductId);
            }

            public void MeterToMeterExemptionToMeterExemptionProduct_Insert(long createdByUserId, long sourceId, long meterToMeterExemptionId, long meterExemptionProductId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToMeterExemptionToMeterExemptionProduct_Insert, 
                    createdByUserId, sourceId, meterToMeterExemptionId, meterExemptionProductId);
            }

            public void AreaToMeter_Insert(long createdByUserId, long sourceId, long areaId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.AreaToMeter_Insert, 
                    createdByUserId, sourceId, areaId, meterId);
            }

            public void SubAreaToSubMeter_Insert(long createdByUserId, long sourceId, long subSubAreaId, long subMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.SubAreaToSubMeter_Insert, 
                    createdByUserId, sourceId, subSubAreaId, subMeterId);
            }

            public List<long> ContractToContractType_GetContractIdListByContractTypeId(long contractTypeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToContractType_GetByContractTypeId, 
                    contractTypeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractTypeId"))
                    .ToList();
            }

            public void BasketToContractMeter_Insert(long createdByUserId, long sourceId, long basketId, long contractMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.BasketToContractMeter_Insert, 
                    createdByUserId, sourceId, basketId, contractMeterId);
            }

            public void CommodityToMeter_Insert(long createdByUserId, long sourceId, long commodityId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CommodityToMeter_Insert, 
                    createdByUserId, sourceId, commodityId, meterId);
            }

            public void GridSupplyPointToMeter_Insert(long createdByUserId, long sourceId, long gridSupplyPointId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.GridSupplyPointToMeter_Insert, 
                    createdByUserId, sourceId, gridSupplyPointId, meterId);
            }

            public void LocalDistributionZoneToMeter_Insert(long createdByUserId, long sourceId, long localDistributionZoneId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.LocalDistributionZoneToMeter_Insert, 
                    createdByUserId, sourceId, localDistributionZoneId, meterId);
            }

            public void MeterToProfileClass_Insert(long createdByUserId, long sourceId, long profileClassId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToProfileClass_Insert, 
                    createdByUserId, sourceId, profileClassId, meterId);
            }

            public void MeterToMeterTimeswitchCode_Insert(long createdByUserId, long sourceId, long meterTimeswitchCodeId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToMeterTimeswitchCode_Insert, 
                    createdByUserId, sourceId, meterTimeswitchCodeId, meterId);
            }

            public void AssetToSubMeter_Insert(long createdByUserId, long sourceId, long assetId, long subMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.AssetToSubMeter_Insert, 
                    createdByUserId, sourceId, assetId, subMeterId);
            }

            public void CustomerToSite_Insert(long createdByUserId, long sourceId, long customerId, long siteId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.CustomerToSite_Insert, 
                    createdByUserId, sourceId, customerId, siteId);
            }

            public void ContractToSupplier_Insert(long createdByUserId, long sourceId, long contractId, long supplierId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractToSupplier_Insert, 
                    createdByUserId, sourceId, contractId, supplierId);
            }

            public long ContractToContractType_GetContractToContractTypeIdByContractIdAndContractTypeId(long contractId, long contractTypeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToContractType_GetByContractIdAndContractTypeId, 
                    contractId, contractTypeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractToContractTypeId"))
                    .FirstOrDefault();
            }

            public void ContractToContractType_Insert(long createdByUserId, long sourceId, long contractId, long contractTypeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractToContractType_Insert, 
                    createdByUserId, sourceId, contractId, contractTypeId);
            }

            public long ContractToContractMeter_GetContractToContractMeterIdByContractIdAndContractMeterId(long contractId, long contractMeterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToContractMeter_GetByContractIdAndContractMeterId, 
                    contractId, contractMeterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractToContractMeterId"))
                    .FirstOrDefault();
            }

            public void ContractToContractMeter_Insert(long createdByUserId, long sourceId, long contractId, long contractMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractToContractMeter_Insert, 
                    createdByUserId, sourceId, contractId, contractMeterId);
            }

            public long ContractMeterToMeter_GetContractMeterToMeterIdByContractMeterIdAndMeterId(long contractId, long contractMeterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractMeterToMeter_GetByContractMeterIdAndMeterId, 
                    contractId, contractMeterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterToMeterId"))
                    .FirstOrDefault();
            }

            public void ContractMeterToMeter_Insert(long createdByUserId, long sourceId, long contractId, long contractMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractMeterToMeter_Insert, 
                    createdByUserId, sourceId, contractId, contractMeterId);
            }

            public void ContractToReferenceVolume_Insert(long createdByUserId, long sourceId, long contractId, long referenceVolumeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractToReferenceVolume_Insert, 
                    createdByUserId, sourceId, contractId, referenceVolumeId);
            }

            public void MeterToSite_Insert(long createdByUserId, long sourceId, long meterId, long siteId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToSite_Insert, 
                    createdByUserId, sourceId, meterId, siteId);
            }

            public void MeterToSubMeter_Insert(long createdByUserId, long sourceId, long meterId, long subMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToSubMeter_Insert, 
                    createdByUserId, sourceId, meterId, subMeterId);
            }

            public void ContractToMeter_Insert(long createdByUserId, long sourceId, long contractId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractToMeter_Insert, 
                    createdByUserId, sourceId, contractId, meterId);
            }

            public long ContractToMeter_GetContractToMeterIdByContractIdAndMeterId(long contractId, long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToMeter_GetByContractIdAndMeterId, 
                    contractId, meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractToMeterId"))
                    .FirstOrDefault();
            }

            public long ContractToSupplier_GetContractToSupplierIdByContractIdAndSupplierId(long contractId, long supplierId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToSupplier_GetByContractIdAndSupplierId, 
                    contractId, supplierId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractToSupplierId"))
                    .FirstOrDefault();
            }

            public long ContractMeterToProduct_GetContractMeterToProductIdByContractMeterIdAndProductId(long contractMeterId, long productId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractMeterToProduct_GetByContractMeterIdAndProductId, 
                    contractMeterId, productId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterToProductId"))
                    .FirstOrDefault();
            }

            public void ContractMeterToProduct_Insert(long createdByUserId, long sourceId, long contractMeterId, long productId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractMeterToProduct_Insert, 
                    createdByUserId, sourceId, contractMeterId, productId);
            }

            public long ContractMeterRateToRateType_GetContractMeterRateToRateTypeIdByContractMeterRateIdAndRateTypeId(long contractMeterRateId, long rateTypeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractMeterRateToRateType_GetByContractMeterRateIdAndRateTypeId, 
                    contractMeterRateId, rateTypeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterRateToRateTypeId"))
                    .FirstOrDefault();
            }

            public void ContractMeterRateToRateType_Insert(long createdByUserId, long sourceId, long contractMeterRateId, long rateTypeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractMeterRateToRateType_Insert, 
                    createdByUserId, sourceId, contractMeterRateId, rateTypeId);
            }

            public long ContractToMeterToContractMeterToProduct_GetContractToMeterToContractMeterToProductIdByContractToMeterIdAndContractMeterToProductId(long contractToMeterId, long contractMeterToProductId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToMeterToContractMeterToProduct_GetByContractToMeterIdAndContractMeterToProductId, 
                    contractToMeterId, contractMeterToProductId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractToMeterToContractMeterToProductId"))
                    .FirstOrDefault();
            }

            public void ContractToMeterToContractMeterToProduct_Insert(long createdByUserId, long sourceId, long contractToMeterId, long contractMeterToProductId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractToMeterToContractMeterToProduct_Insert, 
                    createdByUserId, sourceId, contractToMeterId, contractMeterToProductId);
            }

            public long BasketToContractMeter_GetBasketToContractMeterIdByBasketIdAndContractMeterId(long basketId, long contractMeterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.BasketToContractMeter_GetByBasketIdAndContractMeterId, 
                    basketId, contractMeterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketToContractMeterId"))
                    .FirstOrDefault();
            }

            public long ContractToMeterToContractMeterToProductToContractMeterRateToRateType_GetContractToMeterToContractMeterToProductToContractMeterRateToRateTypeIdByBasketIdAndContractMeterId(long contractToMeterToContractMeterToProductId, long contractMeterRateToRateTypeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToMeterToContractMeterToProductToContractMeterRateToRateType_GetByContractToMeterToContractMeterToProductIdAndContractMeterRateToRateTypeId, 
                    contractToMeterToContractMeterToProductId, contractMeterRateToRateTypeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractToMeterToContractMeterToProductToContractMeterRateToRateTypeId"))
                    .FirstOrDefault();
            }

            public void ContractToMeterToContractMeterToProductToContractMeterRateToRateType_Insert(long createdByUserId, long sourceId, long contractToMeterToContractMeterToProductId, long contractMeterRateToRateTypeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractToMeterToContractMeterToProductToContractMeterRateToRateType_Insert, 
                    createdByUserId, sourceId, contractToMeterToContractMeterToProductId, contractMeterRateToRateTypeId);
            }

            public long BasketToTrade_GetBasketToTradeIdByBasketIdAndTradeId(long basketId, long tradeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.BasketToTrade_GetByBasketIdAndTradeId, 
                    basketId, tradeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("BasketToTradeId"))
                    .FirstOrDefault();
            }

            public void BasketToTrade_Insert(long createdByUserId, long sourceId, long basketId, long tradeId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.BasketToTrade_Insert, 
                    createdByUserId, sourceId, basketId, tradeId);
            }

            public long TradeDetailToVolumeUnit_GetTradeDetailToVolumeUnitIdByTradeDetailIdAndVolumeUnitId(long tradeDetailId, long volumeUnitId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.TradeDetailToVolumeUnit_GetByTradeDetailIdAndVolumeUnitId, 
                    tradeDetailId, volumeUnitId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeDetailToVolumeUnitId"))
                    .FirstOrDefault();
            }

            public void TradeDetailToVolumeUnit_Insert(long createdByUserId, long sourceId, long tradeDetailId, long volumeUnitId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.TradeDetailToVolumeUnit_Insert, 
                    createdByUserId, sourceId, tradeDetailId, volumeUnitId);
            }

            public long RateUnitToTradeDetail_GetRateUnitToTradeDetailIdByRateUnitIdAndTradeDetailId(long rateUnitId, long tradeDetailId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.RateUnitToTradeDetail_GetByRateUnitIdAndTradeDetailId, 
                    rateUnitId, tradeDetailId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("RateUnitToTradeDetailId"))
                    .FirstOrDefault();
            }

            public void RateUnitToTradeDetail_Insert(long createdByUserId, long sourceId, long rateUnitId, long tradeDetailId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.RateUnitToTradeDetail_Insert, 
                    createdByUserId, sourceId, rateUnitId, tradeDetailId);
            }

            public long TradeToTradeProduct_GetTradeToTradeProductIdByTradeIdAndTradeProductId(long tradeId, long tradeProductId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.TradeToTradeProduct_GetByTradeIdAndTradeProductId, 
                    tradeId, tradeProductId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeToTradeProductId"))
                    .FirstOrDefault();
            }

            public void TradeToTradeProduct_Insert(long createdByUserId, long sourceId, long tradeId, long tradeProductId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.TradeToTradeProduct_Insert, 
                    createdByUserId, sourceId, tradeId, tradeProductId);
            }

            public long TradeToTradeDirection_GetTradeToTradeDirectionIdByTradeIdAndTradeDirectionId(long tradeId, long tradeDirectionId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.TradeToTradeDirection_GetByTradeIdAndTradeDirectionId, 
                    tradeId, tradeDirectionId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TradeToTradeDirectionId"))
                    .FirstOrDefault();
            }

            public void TradeToTradeDirection_Insert(long createdByUserId, long sourceId, long tradeId, long tradeDirectionId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.TradeToTradeDirection_Insert, 
                    createdByUserId, sourceId, tradeId, tradeDirectionId);
            }

            public long CommodityToMeter_GetCommodityIdByMeterId(long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.CommodityToMeter_GetByMeterId, 
                    meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("CommodityId"))
                    .FirstOrDefault();
            }
        }
    }
}