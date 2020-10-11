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
                    _storedProcedureMappingEnums.ContractMeterToMeter_GetByMeterId, 
                    meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ContractMeterId"))
                    .ToList();
            }

            public void ContractMeterToMeter_Insert(long createdByUserId, long sourceId, long contractMeterId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.ContractMeterToMeter_Insert, 
                    createdByUserId, sourceId, contractMeterId, meterId);
            }
        }
    }
}