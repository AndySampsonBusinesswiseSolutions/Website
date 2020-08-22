using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public List<long> GranularityToTimePeriod_GetTimePeriodIdListByGranularityId(long granularityId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.GranularityToTimePeriod_GetByGranularityId, 
                    granularityId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TimePeriodId"))
                    .ToList();
            }
        }
    }
}