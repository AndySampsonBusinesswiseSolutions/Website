using System.Reflection;
using System.Data;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void LocalDistributionZoneToMeter_Insert(long createdByUserId, long sourceId, long localDistributionZoneId, long meterId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.LocalDistributionZoneToMeter_Insert, 
                    createdByUserId, sourceId, localDistributionZoneId, meterId);
            }

            public long LocalDistributionZoneToMeter_GetLocalDistributionZoneToMeterIdByLocalDistributionZoneIdAndMeterId(long localDistributionZoneId, long meterId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.LocalDistributionZoneToMeter_GetByLocalDistributionZoneIdAndMeterId, 
                    localDistributionZoneId, meterId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("LocalDistributionZoneToMeterId"))
                    .FirstOrDefault();
            }
        }
    }
}