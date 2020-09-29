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
            public long ProfileAgentAttribute_GetProfileAgentAttributeIdByProfileAgentAttributeDescription(string profileAgentAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileAgentAttribute_GetByProfileAgentAttributeDescription, 
                    profileAgentAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileAgentAttributeId"))
                    .FirstOrDefault();
            }

            public List<string> ProfileAgentDetail_GetProfileAgentDetailDescriptionByProfileAgentAttributeId(long profileAgentAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileAgentDetail_GetByProfileAgentAttributeId, 
                    profileAgentAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("ProfileAgentDetailDescription"))
                    .ToList();
            }

            public string ProfileAgentDetail_GetProfileAgentDetailDescriptionByProfileAgentIdAndProfileAgentAttributeId(long profileAgentId, long profileAgentAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileAgentDetail_GetByProfileAgentIdAndProfileAgentAttributeId, 
                    profileAgentId, profileAgentAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("ProfileAgentDetailDescription"))
                    .FirstOrDefault();
            }

            public long ProfileAgentDetail_GetProfileAgentIdByProfileAgentAttributeIdAndProfileAgentDetailDescription(long profileAgentAttributeId, string profileAgentDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileAgentDetail_GetByProfileAgentAttributeIdAndProfileAgentDetailDescription, 
                    profileAgentAttributeId, profileAgentDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileAgentId"))
                    .First();
            }
        }
    }
}
