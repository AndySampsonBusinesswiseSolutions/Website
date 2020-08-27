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
            public long Date_GetDateIdByDateDescription(string dateDescription)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Date_GetByDateDescription, 
                    dateDescription);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("DateId"))
                    .FirstOrDefault();
            }

            public Dictionary<string, long> Date_GetDateDescriptionIdDictionary()
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureInformationEnums.Date_GetList);

                return dataTable.Rows.Cast<DataRow>()
                    .ToDictionary(x => x.Field<string>("DateDescription"), x => x.Field<long>("DateId"));
            }
        }
    }
}