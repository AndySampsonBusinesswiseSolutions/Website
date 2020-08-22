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
            public List<long> TimePeriod_GetTimePeriodIdListByEndTime(string endTime)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.TimePeriod_GetByEndTime,
                    endTime);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("TimePeriodId"))
                    .ToList();
            }
        }
    }
}