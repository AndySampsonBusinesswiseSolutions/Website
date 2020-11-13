using System.Reflection;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void SubAreaToSubMeter_Insert(long createdByUserId, long sourceId, long subAreaId, long subMeterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.SubAreaToSubMeter_Insert, 
                    createdByUserId, sourceId, subAreaId, subMeterId);
            }

            public long SubAreaToSubMeter_GetSubAreaToSubMeterIdBySubAreaIdAndSubMeterId(long subAreaId, long subMeterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.SubAreaToSubMeter_GetBySubAreaIdAndSubMeterId,
                    subAreaId, subMeterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubAreaToSubMeterId"))
                    .FirstOrDefault();
            }

            public long SubAreaToSubMeter_GetSubAreaIdBySubMeterId(long subMeterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.SubAreaToSubMeter_GetBySubMeterId,
                    subMeterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubAreaId"))
                    .FirstOrDefault();
            }

            public List<long> SubAreaToSubMeter_GetSubMeterIdListBySubAreaId(long subAreaId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.SubAreaToSubMeter_GetBySubAreaId,
                    subAreaId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubMeterId"))
                    .ToList();
            }
        }
    }
}