using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
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
        }
    }
}