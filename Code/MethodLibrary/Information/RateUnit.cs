using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class InformationSchema
        {
            public long RateUnit_GetRateUnitIdByRateUnitDescription(string rateUnitDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.RateUnit_GetByRateUnitDescription, 
                    rateUnitDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("RateUnitId"))
                    .FirstOrDefault();
            }
        }
    }
}