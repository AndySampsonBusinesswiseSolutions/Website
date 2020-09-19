using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class DemandForecast
        {
            public IEnumerable<DataRow> ForecastAgent_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ForecastAgent_GetList);

                return dataTable.Rows.Cast<DataRow>();
            }
        }
    }
}
