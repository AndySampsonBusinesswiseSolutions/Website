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
            public Dictionary<long, long> DateToForecastGroup_GetDateForecastGroupDictionaryByPriority(long priority)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToForecastGroup_GetByPriority, 
                    priority);

                return dataTable.AsEnumerable()
                    .ToDictionary(r => r.Field<long>("DateId"), r => r.Field<long>("ForecastGroupId"));
            }

            public Dictionary<long, Dictionary<long, int>> DateToForecastGroup_GetDateForecastGroupDictionary()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToForecastGroup_GetList);

                var dateIdList = dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DateId"))
                    .Distinct();

                var dictionary = new Dictionary<long, Dictionary<long, int>>();

                foreach(var dateId in dateIdList)
                {
                    var forecastGroupDictionary = dataTable.AsEnumerable()
                        .Where(r => r.Field<long>("DateId") == dateId)
                        .ToDictionary(r => r.Field<long>("ForecastGroupId"), r => r.Field<int>("Priority"));
                    
                    dictionary.Add(dateId, forecastGroupDictionary);
                }

                return dictionary;
            }
        }
    }
}