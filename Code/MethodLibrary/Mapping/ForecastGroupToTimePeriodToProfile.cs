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
            public IEnumerable<DataRow> ForecastGroupToTimePeriodToProfile_GetByProfileId(long profileId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.ForecastGroupToTimePeriodToProfile_GetByProfileId, 
                    profileId);

                return dataTable.Rows.Cast<DataRow>();
            }
        }
    }
}