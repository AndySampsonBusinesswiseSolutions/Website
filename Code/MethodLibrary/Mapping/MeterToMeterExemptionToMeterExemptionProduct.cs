using System.Reflection;
using System.Data;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public void MeterToMeterExemptionToMeterExemptionProduct_Insert(long createdByUserId, long sourceId, long meterToMeterExemptionId, long meterExemptionProductId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToMeterExemptionToMeterExemptionProduct_Insert, 
                    createdByUserId, sourceId, meterToMeterExemptionId, meterExemptionProductId);
            }

            public long MeterToMeterExemptionToMeterExemptionProduct_GetMeterToMeterExemptionToMeterExemptionProductIdByMeterToMeterExemptionIdAndMeterExemptionProductId(long meterToMeterExemptionId, long meterExemptionProductId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.MeterToMeterExemptionToMeterExemptionProduct_GetByMeterToMeterExemptionIdAndMeterExemptionProductId, 
                    meterToMeterExemptionId, meterExemptionProductId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterToMeterExemptionToMeterExemptionProductId"))
                    .FirstOrDefault();
            }
        }
    }
}