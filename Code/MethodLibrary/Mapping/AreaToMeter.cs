using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public void AreaToMeter_Insert(long createdByUserId, long sourceId, long areaId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.AreaToMeter_Insert, 
                    createdByUserId, sourceId, areaId, meterId);
            }

            public long AreaToMeter_GetAreaToMeterIdByAreaIdAndMeterId(long areaId, long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.AreaToMeter_GetByAreaIdAndMeterId, 
                    areaId, meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("AreaToMeterId"))
                    .FirstOrDefault();
            }

            public long AreaToMeter_GetAreaIdByMeterId(long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.AreaToMeter_GetByMeterId, 
                    meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("AreaId"))
                    .FirstOrDefault();
            }

            public List<long> AreaToMeter_GetMeterIdListByAreaId(long areaId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.AreaToMeter_GetByAreaId, 
                    areaId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterId"))
                    .ToList();
            }

            public void AreaToMeter_DeleteByMeterId(long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.AreaToMeter_DeleteByMeterId, 
                    meterId);
            }
        }
    }
}