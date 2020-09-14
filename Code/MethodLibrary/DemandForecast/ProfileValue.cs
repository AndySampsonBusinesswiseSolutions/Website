using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class DemandForecast
        {
            public decimal ProfileValue_GetProfileValueByProfileValueId(long profileValueId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileValue_GetByProfileValueId, 
                    profileValueId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<decimal>("ProfileValue"))
                    .First();
            }
        }
    }
}
