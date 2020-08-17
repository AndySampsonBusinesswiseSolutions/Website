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
            public long ContractMeterRateAttribute_GetContractMeterRateAttributeIdByContractMeterRateAttributeDescription(string contractMeterAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeterRateAttribute_GetByContractMeterRateAttributeDescription, 
                    contractMeterAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterRateAttributeId"))
                    .FirstOrDefault();
            }

            public void ContractMeterRate_Insert(long createdByUserId, long sourceId, string contractMeterGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.ContractMeterRate_Insert, 
                    createdByUserId, sourceId, contractMeterGUID);
            }

            public long ContractMeterRate_GetContractMeterRateIdByContractMeterRateGUID(string contractMeterGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeterRate_GetByContractMeterRateGUID, 
                    contractMeterGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterRateId"))
                    .FirstOrDefault();
            }

            public void ContractMeterRateDetail_Insert(long createdByUserId, long sourceId, long contractMeterId, long contractMeterAttributeId, string contractMeterDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.ContractMeterRateDetail_Insert, 
                    createdByUserId, sourceId, contractMeterId, contractMeterAttributeId, contractMeterDetailDescription);
            }

            public long ContractMeterRateDetail_GetContractMeterRateIdByContractMeterRateAttributeIdAndContractMeterRateDetailDescription(long contractMeterRateAttributeId, string contractMeterRateDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractMeterRateDetail_GetByContractMeterRateAttributeIdAndContractMeterRateDetailDescription, 
                    contractMeterRateAttributeId, contractMeterRateDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterRateId"))
                    .FirstOrDefault();
            }
        }
    }
}
