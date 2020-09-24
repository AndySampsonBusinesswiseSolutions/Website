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
            public Dictionary<long, int> DateToForecastAgent_GetDateForecastAgentDictionaryByDateId(long dateId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToForecastAgent_GetByDateId,
                    dateId);

                return dataTable.AsEnumerable()
                    .Where(r => r.Field<long>("DateId") == dateId)
                    .ToDictionary(r => r.Field<long>("ForecastAgentId"), r => r.Field<int>("Priority"));
            }
        }
    }
}