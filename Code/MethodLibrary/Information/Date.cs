using System.Data;
using System.Linq;
using System.Reflection;

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
        }
    }
}