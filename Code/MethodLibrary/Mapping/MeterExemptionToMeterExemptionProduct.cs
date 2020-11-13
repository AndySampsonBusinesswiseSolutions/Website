using System.Reflection;
using System.Data;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void MeterExemptionToMeterExemptionProduct_Insert(long createdByUserId, long sourceId, long meterExemptionId, long meterExemptionProductId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterExemptionToMeterExemptionProduct_Insert, 
                    createdByUserId, sourceId, meterExemptionId, meterExemptionProductId);
            }

            public long MeterExemptionToMeterExemptionProduct_GetMeterExemptionToMeterExemptionProductIdByMeterExemptionIdAndMeterExemptionProductId(long meterExemptionId, long meterExemptionProductId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.MeterExemptionToMeterExemptionProduct_GetByMeterExemptionIdAndMeterExemptionProductId, 
                    meterExemptionId, meterExemptionProductId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterExemptionToMeterExemptionProductId"))
                    .FirstOrDefault();
            }
        }
    }
}