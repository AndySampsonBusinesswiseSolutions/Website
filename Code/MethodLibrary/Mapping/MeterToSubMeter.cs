using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void MeterToSubMeter_Insert(long createdByUserId, long sourceId, long meterId, long subMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToSubMeter_Insert, 
                    createdByUserId, sourceId, meterId, subMeterId);
            }

            public long MeterToSubMeter_GetMeterToSubMeterIdByMeterIdAndSubMeterId(long meterId, long subMeterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.MeterToSubMeter_GetByMeterIdAndSubMeterId,
                    meterId, subMeterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterToSubMeterId"))
                    .FirstOrDefault();
            }

            public List<long> MeterToSubMeter_GetSubMeterIdListByMeterId(long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.MeterToSubMeter_GetByMeterId,
                    meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubMeterId"))
                    .ToList();
            }

            public long MeterToSubMeter_GetMeterIdBySubMeterId(long subMeterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.MeterToSubMeter_GetBySubMeterId,
                    subMeterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterId"))
                    .FirstOrDefault();
            }
        }
    }
}