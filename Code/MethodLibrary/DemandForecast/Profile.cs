using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class DemandForecastSchema
        {
            public long ProfileAttribute_GetProfileAttributeIdByProfileAttributeDescription(string profileAttributeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileAttribute_GetByProfileAttributeDescription, 
                    profileAttributeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileAttributeId"))
                    .FirstOrDefault();
            }

            public long ProfileDetail_GetProfileIdByProfileAttributeIdAndProfileDetailDescription(long profileAttributeId, string profileDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileDetail_GetByProfileAttributeIdAndProfileDetailDescription, 
                    profileAttributeId, profileDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileId"))
                    .FirstOrDefault();
            }

            public List<long> ProfileDetail_GetProfileIdListByProfileAttributeIdAndProfileDetailDescription(long profileAttributeId, string profileDetailDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileDetail_GetByProfileAttributeIdAndProfileDetailDescription, 
                    profileAttributeId, profileDetailDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProfileId"))
                    .ToList();
            }

            public List<string> ProfileDetail_GetProfileDetailDescriptionListByProfileIdAndProfileAttributeId(long profileId, long profileAttributeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureDemandForecastEnums.ProfileDetail_GetByProfileIdAndProfileAttributeId, 
                    profileId, profileAttributeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("ProfileDetailDescription"))
                    .ToList();
            }
        }
    }
}