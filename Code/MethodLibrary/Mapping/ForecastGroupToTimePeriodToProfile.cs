using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Mapping
        {
            public List<Entity.Mapping.ForecastGroupToTimePeriodToProfile> ForecastGroupToTimePeriodToProfile_GetByProfileId(long profileId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ForecastGroupToTimePeriodToProfile_GetByProfileId, 
                    profileId);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Mapping.ForecastGroupToTimePeriodToProfile(d)).ToList();
            }

            public Dictionary<long, long> ForecastGroupToTimePeriodToProfile_GetDictionaryByProfileId(long profileId)
            {
                return ForecastGroupToTimePeriodToProfile_GetByProfileId(profileId).ToDictionary(
                    fgttptp => fgttptp.ForecastGroupToTimePeriodId,
                    fgttptp => fgttptp.ForecastGroupToTimePeriodToProfileId
                );
            }
        }
    }
}