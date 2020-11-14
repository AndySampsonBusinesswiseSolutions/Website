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
            public List<Entity.Mapping.DateToWeek> DateToWeek_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToWeek_GetList);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Mapping.DateToWeek(d)).ToList();
            }

            public Dictionary<long, long> GetDateToWeekDictionary()
            {
                return DateToWeek_GetList().ToDictionary(d => d.DateId, d => d.WeekId);
            }
        }
    }
}