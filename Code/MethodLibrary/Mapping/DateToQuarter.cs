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
            public List<Entity.Mapping.DateToQuarter> DateToQuarter_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToQuarter_GetList);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Mapping.DateToQuarter(d)).ToList();
            }

            public Dictionary<long, long> GetDateToQuarterDictionary()
            {
                return DateToQuarter_GetList().ToDictionary(d => d.DateId, d => d.QuarterId);
            }
        }
    }
}