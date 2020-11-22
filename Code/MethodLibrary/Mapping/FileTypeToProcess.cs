using System.Data;
using System.Linq;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class MappingSchema
        {
            public long FileTypeToProcess_GetProcessIdByFileTypeId(long fileTypeId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureMappingEnums.FileTypeToProcess_GetByFileTypeId, 
                    fileTypeId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessId"))
                    .FirstOrDefault();
            }
        }
    }
}