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
            public long InsertNewContract(long createdByUserId, long sourceId)
            {
                //Create new ContractGUID
                var GUID = Guid.NewGuid().ToString();

                while (Contract_GetContractIdByContractGUID(GUID) > 0)
                {
                    GUID = Guid.NewGuid().ToString();
                }

                //Insert into [Customer].[Contract]
                Contract_Insert(createdByUserId, sourceId, GUID);
                return Contract_GetContractIdByContractGUID(GUID);
            }

            public long ContractAttribute_GetContractAttributeIdByContractAttributeDescription(string contractAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractAttribute_GetByContractAttributeDescription, 
                    contractAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractAttributeId"))
                    .FirstOrDefault();
            }

            public long ContractDetail_GetContractDetailIdByContractAttributeIdAndContractDetailDescription(long contractAttributeId, string contractDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractDetail_GetByContractAttributeIdAndContractDetailDescription, 
                    contractAttributeId, contractDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractDetailId"))
                    .FirstOrDefault();
            }

            public List<long> ContractDetail_GetContractIdListByContractAttributeIdAndContractDetailDescription(long contractAttributeId, string contractDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.ContractDetail_GetByContractAttributeIdAndContractDetailDescription, 
                    contractAttributeId, contractDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractId"))
                    .ToList();
            }

            public void Contract_Insert(long createdByUserId, long sourceId, string contractGUID)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.Contract_Insert, 
                    createdByUserId, sourceId, contractGUID);
            }

            public long Contract_GetContractIdByContractGUID(string contractGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureCustomerEnums.Contract_GetByContractGUID, 
                    contractGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractId"))
                    .FirstOrDefault();
            }

            public void ContractDetail_Insert(long createdByUserId, long sourceId, long contractId, long contractAttributeId, string contractDetailDescription)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureCustomerEnums.ContractDetail_Insert, 
                    createdByUserId, sourceId, contractId, contractAttributeId, contractDetailDescription);
            }
        }
    }
}