using System.Reflection;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using enums;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private static readonly Enums.StoredProcedure.Supply _storedProcedureSupplyEnums = new Enums.StoredProcedure.Supply();

            private void CreateLoadedUsageEntities(long schemaId, long meterId, string meterType)
            {
                var tableName = $"LoadedUsage";
                var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                if(tableId == 0)
                {
                    LoadedUsage_CreateTable(meterId, meterType);
                }

                LoadedUsage_CreateGetLatestStoredProcedure(meterId, meterType);
                LoadedUsage_GrantExecuteToStoredProcedures(meterId, meterType);
            }

            private void LoadedUsage_CreateTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateTable, 
                    meterId, meterType);
            }

            private void LoadedUsage_CreateGetLatestStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateGetLatestStoredProcedure, 
                    meterId, meterType);
            }

            public void LoadedUsage_Insert(string meterType, long meterId, DataTable loadedUsageDataTable)
            {
                new Methods().BulkInsert(loadedUsageDataTable, $"[Supply.{meterType}{meterId}].[LoadedUsage]");
            }

            public List<DataRow> LoadedUsage_GetLatest(string meterType, long meterId)
            {
                var loadedUsageGetLatestStoredProcedure = string.Format(_storedProcedureSupplyEnums.LoadedUsage_GetLatest, meterType, meterId);

                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), loadedUsageGetLatestStoredProcedure);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            private void LoadedUsage_GrantExecuteToStoredProcedures(long meterId, string meterType)
            {
                foreach(var loadedUsageStoredProcedure in _storedProcedureSupplyEnums.LoadedUsageStoredProcedureList)
                {
                    var storedProcedure = string.Format(loadedUsageStoredProcedure, meterType, meterId);

                    foreach(var api in _systemAPIRequireAccessToUsageEntitiesEnums.APIList)
                    {
                        var SQL = $"GRANT EXECUTE ON OBJECT::{storedProcedure} TO [{api}];";
                        ExecuteSQL(SQL);
                    }
                }
            }
        }
    }
}