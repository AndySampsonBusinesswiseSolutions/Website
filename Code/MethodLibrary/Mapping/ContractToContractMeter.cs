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
            public List<long> ContractToContractMeter_GetContractMeterIdListByContractId(long contractId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractToContractMeter_GetByContractId, 
                    contractId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterId"))
                    .ToList();
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
        }
    }
}