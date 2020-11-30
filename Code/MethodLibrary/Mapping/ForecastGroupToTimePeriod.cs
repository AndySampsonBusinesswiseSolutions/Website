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
            public List<Entity.Mapping.ForecastGroupToTimePeriod> ForecastGroupToTimePeriod_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ForecastGroupToTimePeriod_GetList);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Mapping.ForecastGroupToTimePeriod(d)).ToList();
            }
        }
    }
}