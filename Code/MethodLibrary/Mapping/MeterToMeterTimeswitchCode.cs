using System.Reflection;
using System.Data;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public void MeterToMeterTimeswitchCode_Insert(long createdByUserId, long sourceId, long meterTimeswitchCodeId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToMeterTimeswitchCode_Insert, 
                    createdByUserId, sourceId, meterTimeswitchCodeId, meterId);
            }

            public long MeterToMeterTimeswitchCode_GetMeterToMeterTimeswitchCodeIdByMeterIdAndMeterTimeswitchCodeId(long meterId, long meterTimeswitchCodeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.MeterToMeterTimeswitchCode_GetByMeterIdAndMeterTimeswitchCodeId, 
                    meterId, meterTimeswitchCodeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterToMeterTimeswitchCodeId"))
                    .FirstOrDefault();
            }
        }
    }
}