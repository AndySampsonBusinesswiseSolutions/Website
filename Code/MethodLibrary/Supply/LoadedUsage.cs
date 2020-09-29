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

                tableName = $"LoadedUsage_Temp";
                tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                if(tableId == 0)
                {
                    LoadedUsage_CreateTempTable(meterId, meterType);
                }

                LoadedUsage_CreateDeleteStoredProcedure(meterId, meterType);
                LoadedUsage_CreateInsertStoredProcedure(meterId, meterType);
                LoadedUsage_CreateGetLatestStoredProcedure(meterId, meterType);
                LoadedUsage_GrantExecuteToStoredProcedures(meterId, meterType);
            }

            private void LoadedUsage_CreateTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateTable, 
                    meterId, meterType);
            }

            private void LoadedUsage_CreateTempTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateTempTable, 
                    meterId, meterType);
            }

            private void LoadedUsage_CreateDeleteStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateDeleteStoredProcedure, 
                    meterId, meterType);
            }

            private void LoadedUsage_CreateInsertStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateInsertStoredProcedure, 
                    meterId, meterType);
            }

            private void LoadedUsage_CreateGetLatestStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateGetLatestStoredProcedure, 
                    meterId, meterType);
            }

            public void LoadedUsage_Delete(string meterType, long meterId)
            {
                var loadedUsageDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.LoadedUsage_Delete, meterType, meterId);

                ExecuteNonQuery(new List<ParameterInfo>().ToArray(), loadedUsageDeleteStoredProcedure);
            }

            public void LoadedUsage_Insert(string meterType, long meterId, string processQueueGUID)
            {
                var loadedUsageInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.LoadedUsage_Insert, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    loadedUsageInsertStoredProcedure,
                    processQueueGUID);
            }

            public void LoadedUsageTemp_Insert(string meterType, long meterId, DataTable loadedUsageDataTable)
            {
                new Methods().BulkInsert(loadedUsageDataTable, $"[Supply.{meterType}{meterId}].[LoadedUsage_Temp]");
            }

            public IEnumerable<DataRow> LoadedUsage_GetLatest(string meterType, long meterId)
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
