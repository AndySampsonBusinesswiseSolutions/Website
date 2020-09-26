using System.Reflection;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private void CreateDateMappingEntities(long schemaId, long meterId, string meterType)
            {
                var tableName = $"DateMapping";
                var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                if(tableId == 0)
                {
                    DateMapping_CreateTable(meterId, meterType);
                }

                DateMapping_CreateDeleteStoredProcedure(meterId, meterType);
                DateMapping_CreateInsertStoredProcedure(meterId, meterType);
                DateMapping_CreateGetLatestStoredProcedure(meterId, meterType);
                DateMapping_GrantExecuteToStoredProcedures(meterId, meterType);
            }

            private void DateMapping_CreateTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.DateMapping_CreateTable, 
                    meterId, meterType);
            }

            private void DateMapping_CreateDeleteStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.DateMapping_CreateDeleteStoredProcedure, 
                    meterId, meterType);
            }

            private void DateMapping_CreateInsertStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.DateMapping_CreateInsertStoredProcedure, 
                    meterId, meterType);
            }

            private void DateMapping_CreateGetLatestStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.DateMapping_CreateGetLatestStoredProcedure, 
                    meterId, meterType);
            }

            public void DateMapping_Delete(string meterType, long meterId)
            {
                var loadedUsageDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.DateMapping_Delete, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, loadedUsageDeleteStoredProcedure);
            }

            public void DateMapping_Insert(string meterType, long meterId, string processQueueGUID)
            {
                var loadedUsageInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.DateMapping_Insert, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    loadedUsageInsertStoredProcedure,
                    processQueueGUID);
            }

            public IEnumerable<DataRow> DateMapping_GetLatest(string meterType, long meterId)
            {
                var loadedUsageGetLatestStoredProcedure = string.Format(_storedProcedureSupplyEnums.DateMapping_GetLatest, meterType, meterId);

                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), loadedUsageGetLatestStoredProcedure);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            private void DateMapping_GrantExecuteToStoredProcedures(long meterId, string meterType)
            {
                foreach(var loadedUsageStoredProcedure in _storedProcedureSupplyEnums.DateMappingStoredProcedureList)
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
