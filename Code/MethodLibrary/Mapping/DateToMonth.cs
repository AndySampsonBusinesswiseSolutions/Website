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
            public List<Entity.Mapping.DateToMonth> DateToMonth_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToMonth_GetList);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Mapping.DateToMonth(d)).ToList();
            }

            public Dictionary<long, long> GetDateToMonthDictionary()
            {
                return DateToMonth_GetList().ToDictionary(d => d.DateId, d => d.MonthId);
            }
        }
    }
}