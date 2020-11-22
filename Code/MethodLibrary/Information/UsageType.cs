using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class InformationSchema
        {
            public long UsageType_GetUsageTypeIdByUsageTypeDescription(string usageTypeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.UsageType_GetByUsageTypeDescription, 
                    usageTypeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("UsageTypeId"))
                    .FirstOrDefault();
            }
        }
    }
}