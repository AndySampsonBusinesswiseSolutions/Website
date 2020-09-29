using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public List<DataRow> ForecastGroupToTimePeriodToProfileToProfileValue_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ForecastGroupToTimePeriodToProfileToProfileValue_GetList);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }
        }
    }
}