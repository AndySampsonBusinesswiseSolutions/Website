using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Information
        {
            public long RateType_GetRateTypeIdByRateTypeCode(string rateTypeCode)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.RateType_GetByRateTypeCode, 
                    rateTypeCode);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("RateTypeId"))
                    .FirstOrDefault();
            }

            public long RateType_GetRateTypeIdByRateTypeDescription(string rateTypeDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.RateType_GetByRateTypeDescription, 
                    rateTypeDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("RateTypeId"))
                    .FirstOrDefault();
            }
        }
    }
}