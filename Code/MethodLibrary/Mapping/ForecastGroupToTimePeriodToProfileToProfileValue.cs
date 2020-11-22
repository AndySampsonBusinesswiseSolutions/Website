using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public List<Entity.Mapping.ForecastGroupToTimePeriodToProfileToProfileValue> ForecastGroupToTimePeriodToProfileToProfileValue_GetList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ForecastGroupToTimePeriodToProfileToProfileValue_GetList);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Mapping.ForecastGroupToTimePeriodToProfileToProfileValue(d)).ToList();
            }

            public Dictionary<long, long> ForecastGroupToTimePeriodToProfileToProfileValue_GetDictionary()
            {
                return ForecastGroupToTimePeriodToProfileToProfileValue_GetList()
                    .ToDictionary(
                    d => d.ForecastGroupToTimePeriodToProfileId, 
                    d => d.ProfileValueId
                );
            }
        }
    }
}