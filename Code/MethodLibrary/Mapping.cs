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

            public void BasketToMeter_Insert(long createdByUserId, long sourceId, long basketId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.BasketToMeter_Insert, 
                    createdByUserId, sourceId, basketId, meterId);
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
        }
    }
}
