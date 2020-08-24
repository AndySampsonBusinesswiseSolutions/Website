using System.Reflection;
using System.Data;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private void Schema_Create(long meterId, string meterType)
            {
                var SQL = $"CREATE SCHEMA [Supply.{meterType}{meterId}]";
                ExecuteSQL(SQL);
            }

            private long Schema_GetSchemaIdBySchemaName(string schemaName)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSupplyEnums.Schema_GetBySchemaName,
                    schemaName);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("schema_id"))
                    .FirstOrDefault();
            }
        }
    }
}