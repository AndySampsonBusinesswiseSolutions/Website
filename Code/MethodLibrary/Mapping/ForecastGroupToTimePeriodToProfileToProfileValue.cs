using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public long ForecastGroupToTimePeriodToProfileToProfileValue_GetProfileValueIdByForecastGroupToTimePeriodToProfileId(long forecastGroupToTimePeriodToProfileId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ForecastGroupToTimePeriodToProfileToProfileValue_GetByForecastGroupToTimePeriodToProfileId, 
                    forecastGroupToTimePeriodToProfileId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileValueId"))
                    .FirstOrDefault();
            }
        }
    }
}