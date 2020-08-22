using System.Data;
using System.Linq;
using System;
using System.Reflection;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class System
        {
            public long Process_GetProcessIdByProcessGUID(string processGUID)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.Process_GetByProcessGUID, 
                    processGUID);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("ProcessId"))
                    .FirstOrDefault();
            }
            
            public string Process_GetProcessGUIDByProcessId(long processId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSystemEnums.Process_GetByProcessId, 
                    processId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<Guid>("ProcessGUID"))
                    .Select(r => r.ToString())
                    .FirstOrDefault();
            }
        }
    }
}
