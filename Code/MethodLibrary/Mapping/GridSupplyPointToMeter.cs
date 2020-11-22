using System.Reflection;
using System.Data;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public void GridSupplyPointToMeter_Insert(long createdByUserId, long sourceId, long gridSupplyPointId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.GridSupplyPointToMeter_Insert, 
                    createdByUserId, sourceId, gridSupplyPointId, meterId);
            }

            public long GridSupplyPointToMeter_GetGridSupplyPointToMeterIdByGridSupplyPointIdAndMeterId(long gridSupplyPointId, long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.GridSupplyPointToMeter_GetByGridSupplyPointIdAndMeterId, 
                    gridSupplyPointId, meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GridSupplyPointToMeterId"))
                    .FirstOrDefault();
            }
        }
    }
}