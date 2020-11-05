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
            public class APIDetailToHostEnvironment
            {
                public List<long> APIDetailToHostEnvironment_GetAPIDetailIdListByHostEnvironmentId(long hostEnvironmentId)
                {
                    var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                        _storedProcedureMappingAPIDetailToHostEnvironmentEnums.APIDetailToHostEnvironment_GetByHostEnvironmentId, 
                        hostEnvironmentId);

                    return dataTable.AsEnumerable()
                        .Select(r => r.Field<long>("APIDetailId"))
                        .ToList();
                }
            }
        }
    }
}