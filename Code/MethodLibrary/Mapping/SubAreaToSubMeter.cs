using System.Reflection;
using System.Data;
using System.Linq;

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

            public long SubAreaToSubMeter_GetSubAreaIdBySubMeterId(long subMeterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.SubAreaToSubMeter_GetBySubMeterId,
                    subMeterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("SubMeterId"))
                    .FirstOrDefault();
            }
        }
    }
}