using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using enums;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class DemandForecast
        {
            public Dictionary<long, string> GetForecastAgentDictionary()
            {
                var _demandForecastForecastAgentAttributeEnums = new Enums.DemandForecast.ForecastAgent.Attribute();
                var forecastAgentNameForecastAgentAttributeId = ForecastAgentAttribute_GetForecastAgentAttributeIdByForecastAgentAttributeDescription(_demandForecastForecastAgentAttributeEnums.Name);
                return ForecastAgentDetail_GetForecastAgentDetailDescriptionByForecastAgentAttributeId(forecastAgentNameForecastAgentAttributeId)
                    .ToDictionary(
                        f => ForecastAgentDetail_GetForecastAgentIdByForecastAgentAttributeIdAndForecastAgentDetailDescription(forecastAgentNameForecastAgentAttributeId, f),
                        f => f);
            }

            public long ForecastAgentAttribute_GetForecastAgentAttributeIdByForecastAgentAttributeDescription(string forecastAgentAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ForecastAgentAttribute_GetByForecastAgentAttributeDescription, 
                    forecastAgentAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ForecastAgentAttributeId"))
                    .FirstOrDefault();
            }

            public List<string> ForecastAgentDetail_GetForecastAgentDetailDescriptionByForecastAgentAttributeId(long forecastAgentAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ForecastAgentDetail_GetByForecastAgentAttributeId, 
                    forecastAgentAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("ForecastAgentDetailDescription"))
                    .ToList();
            }

            public long ForecastAgentDetail_GetForecastAgentIdByForecastAgentAttributeIdAndForecastAgentDetailDescription(long forecastAgentAttributeId, string forecastAgentDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ForecastAgentDetail_GetByForecastAgentAttributeIdAndForecastAgentDetailDescription, 
                    forecastAgentAttributeId, forecastAgentDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ForecastAgentId"))
                    .First();
            }
        }
    }
}