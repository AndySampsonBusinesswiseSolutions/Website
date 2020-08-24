using System.Reflection;
using System.Data;
using System.Linq;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private void CreateLoadedUsageEntities(long schemaId, long meterId, string meterType)
            {
                var tableName = $"LoadedUsage";
                var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                if(tableId == 0)
                {
                    LoadedUsage_CreateTable(meterId, meterType);
                }

                LoadedUsage_CreateDeleteStoredProcedure(meterId, meterType);
                LoadedUsage_CreateInsertStoredProcedure(meterId, meterType);
                LoadedUsage_GrantExecuteToStoredProcedures(meterId, meterType);
            }

            private void LoadedUsage_CreateTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateTable, 
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

            public void LoadedUsage_Delete(string meterType, long meterId, long dateId, long timePeriodId)
            {
                var loadedUsageDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.LoadedUsage_Delete, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    loadedUsageDeleteStoredProcedure,
                    dateId, timePeriodId);
            }

            public void LoadedUsage_Insert(long createdByUserId, long sourceId, string meterType, long meterId, long dateId, long timePeriodId, long usageTypeId, decimal usage)
            {
                var loadedUsageInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.LoadedUsage_Insert, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    loadedUsageInsertStoredProcedure,
                    createdByUserId, sourceId, dateId, timePeriodId, usageTypeId, usage);
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
