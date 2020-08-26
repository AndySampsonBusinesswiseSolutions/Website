using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public List<long> ContractToContractType_GetContractIdListByContractTypeId(long contractTypeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToContractType_GetByContractTypeId, 
                    contractTypeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractId"))
                    .ToList();
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
        }
    }
}