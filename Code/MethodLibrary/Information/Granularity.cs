using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public List<string> Granularity_GetGranularityCodeList()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Granularity_GetList);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<string>("GranularityCode"))
                    .ToList();
            }

            public long Granularity_GetGranularityIdByGranularityDescription(string granularityDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Granularity_GetByGranularityDescription, 
                    granularityDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("GranularityId"))
                    .FirstOrDefault();
            }
        }
    }
}