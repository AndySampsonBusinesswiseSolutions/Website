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
            public List<Entity.DemandForecast.ProfileValue> ProfileValue_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileValue_GetList);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.DemandForecast.ProfileValue(d)).ToList();
            }

            public Dictionary<long, decimal> ProfileValue_GetDictionary()
            {
                return ProfileValue_GetList().ToDictionary(pv => pv.ProfileValueId, pv => pv.Value);
            }
        }
    }
}