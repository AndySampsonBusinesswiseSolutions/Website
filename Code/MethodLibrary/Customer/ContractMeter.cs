using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Customer
        {
            public long ContractMeterAttribute_GetContractMeterAttributeIdByContractMeterAttributeDescription(string contractMeterAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeterAttribute_GetByContractMeterAttributeDescription, 
                    contractMeterAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterAttributeId"))
                    .FirstOrDefault();
            }

            public void ContractMeter_Insert(long createdByUserId, long sourceId, string contractMeterGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.ContractMeter_Insert, 
                    createdByUserId, sourceId, contractMeterGUID);
            }

            public long ContractMeter_GetContractMeterIdByContractMeterGUID(string contractMeterGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeter_GetByContractMeterGUID, 
                    contractMeterGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterId"))
                    .FirstOrDefault();
            }

            public void ContractMeterDetail_Insert(long createdByUserId, long sourceId, long contractMeterId, long contractMeterAttributeId, string contractMeterDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.ContractMeterDetail_Insert, 
                    createdByUserId, sourceId, contractMeterId, contractMeterAttributeId, contractMeterDetailDescription);
            }

            public List<long> ContractMeterDetail_GetContractMeterIdListByContractMeterAttributeIdAndContractMeterDetailDescription(long contractMeterAttributeId, string contractMeterDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeterDetail_GetByContractMeterAttributeIdAndContractMeterDetailDescription, 
                    contractMeterAttributeId, contractMeterDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterId"))
                    .ToList();
            }
        }
    }
}
