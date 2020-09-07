using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public IEnumerable<DataRow> DateToGranularityToTimePeriod_GetList()
            {
                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), 
                    _storedProcedureMappingEnums.DateToGranularityToTimePeriod_GetList);

                return dataTable.Rows.Cast<DataRow>();
            }
        }
    }
}