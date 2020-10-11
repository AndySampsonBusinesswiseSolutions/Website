using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
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
        }
    }
}