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
            public Dictionary<long, long> DateToForecastGroup_GetDateForecastGroupDictionaryByPriority(long priority)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToForecastGroup_GetByPriority, 
                    priority);

                return dataTable.AsEnumerable()
                    .ToDictionary(r => r.Field<long>("DateId"), r => r.Field<long>("ForecastGroupId"));
            }
        }
    }
}