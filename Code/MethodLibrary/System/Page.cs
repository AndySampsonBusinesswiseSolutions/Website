using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class System
        {
            public long Page_GetPageIdByGUID(string pageGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.Page_GetByPageGUID, 
                    pageGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("PageId"))
                    .FirstOrDefault();
            }
        }
    }
}
