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
            public List<Entity.Mapping.DateToYear> DateToYear_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.DateToYear_GetList);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Mapping.DateToYear(d)).ToList();
            }

            public Dictionary<long, long> GetDateToYearDictionary()
            {
                return DateToYear_GetList().ToDictionary(d => d.DateId, d => d.YearId);
            }
        }
    }
}