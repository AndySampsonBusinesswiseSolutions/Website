using System.Collections.Concurrent;
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
            public ConcurrentDictionary<long, Dictionary<long, int>> DateToForecastAgent_GetDateForecastAgentDictionary()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToForecastAgent_GetList);

                var dateIdList = dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DateId"))
                    .Distinct();

                var dictionary = new ConcurrentDictionary<long, Dictionary<long, int>>();

                foreach(var dateId in dateIdList)
                {
                    var forecastAgentDictionary = dataTable.AsEnumerable()
                        .Where(r => r.Field<long>("DateId") == dateId)
                        .ToDictionary(r => r.Field<long>("ForecastAgentId"), r => r.Field<int>("Priority"));
                    
                    dictionary.TryAdd(dateId, forecastAgentDictionary);
                }

                return dictionary;
            }
        }
    }
}