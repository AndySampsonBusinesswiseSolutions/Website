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
            public IEnumerable<DataRow> TimePeriodToTimePeriod_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.TimePeriodToTimePeriod_GetList);

                return dataTable.Rows.Cast<DataRow>();
            }

            public Dictionary<long, List<long>> TimePeriodToTimePeriod_GetDictionary()
            {
                var timePeriodToTimePeriodList = TimePeriodToTimePeriod_GetList();
                var timePeriodIdList = timePeriodToTimePeriodList
                    .Select(r => r.Field<long>("TimePeriodId"))
                    .Distinct();

                var dictionary = new Dictionary<long, List<long>>();

                foreach(var timePeriodId in timePeriodIdList)
                {
                    var mappedTimePeriodList = timePeriodToTimePeriodList
                        .Where(r => r.Field<long>("TimePeriodId") == timePeriodId)
                        .Select(t => t.Field<long>("MappedTimePeriodId")).ToList();
                    
                    dictionary.Add(timePeriodId, mappedTimePeriodList);
                }

                return dictionary;
            }
        }
    }
}