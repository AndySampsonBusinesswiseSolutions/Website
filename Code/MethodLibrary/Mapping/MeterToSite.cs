using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public void MeterToSite_Insert(long createdByUserId, long sourceId, long meterId, long siteId)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureMappingEnums.MeterToSite_Insert, 
                    createdByUserId, sourceId, meterId, siteId);
            }

            public long MeterToSite_GetMetertoSiteIdByMeterIdAndSiteId(long meterId, long siteId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.MeterToSite_GetByMeterIdAndSiteId,
                    meterId, siteId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterToSiteId"))
                    .FirstOrDefault();
            }

            public List<long> MeterToSite_GetMeterIdListBySiteId(long siteId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.MeterToSite_GetBySiteId,
                    siteId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("MeterId"))
                    .ToList();
            }
        }
    }
}