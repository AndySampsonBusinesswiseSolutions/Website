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
            public List<Entity.Mapping.TimePeriodToTimePeriod> TimePeriodToTimePeriod_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.TimePeriodToTimePeriod_GetList);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Mapping.TimePeriodToTimePeriod(d)).ToList();
            }

            public Dictionary<long, List<long>> TimePeriodToTimePeriod_GetDictionary()
            {
                var timePeriodToTimePeriodList = TimePeriodToTimePeriod_GetList();
                var timePeriodIdList = timePeriodToTimePeriodList
                    .Select(r => r.TimePeriodId)
                    .Distinct();

                var dictionary = new Dictionary<long, List<long>>();

                foreach(var timePeriodId in timePeriodIdList)
                {
                    var mappedTimePeriodList = timePeriodToTimePeriodList
                        .Where(r => r.TimePeriodId == timePeriodId)
                        .Select(t => t.MappedTimePeriodId).ToList();
                    
                    dictionary.Add(timePeriodId, mappedTimePeriodList);
                }

                return dictionary;
            }

            public Dictionary<long, Dictionary<long, List<long>>> TimePeriodToTimePeriod_GetOrderedDictionary()
            {
                var timePeriodToTimePeriodDictionary = TimePeriodToTimePeriod_GetDictionary();
                return timePeriodToTimePeriodDictionary.ToDictionary(
                    t => t.Key,
                    t => t.Value.Select(m => m).Distinct()
                        .ToDictionary(m => m, m => timePeriodToTimePeriodDictionary.Where(t => t.Value.Contains(m)).Select(t => t.Key).ToList())
                        .OrderBy(m => m.Value.Count())
                        .ToDictionary(o => o.Key, o => o.Value)
                );
            }
        }
    }
}