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
            public List<long> ContractMeterToMeter_GetContractMeterIdListByMeterId(long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ContractMeterToMeter_GetByContractMeterId, 
                    meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterId"))
                    .ToList();
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
        }
    }
}