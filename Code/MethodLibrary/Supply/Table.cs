using System.Reflection;
using System.Data;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private long Table_GetTableIdByTableNameAndSchemaId(string tableName, long schemaId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSupplyEnums.Table_GetByTableNameAndSchemaId,
                    tableName, schemaId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<int>("object_id"))
                    .FirstOrDefault();
            }
        }
    }
}