using System.Reflection;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class SupplySchema
        {
            private void CreateDateMappingEntities(long schemaId, long meterId, string meterType)
            {
                var tableName = $"DateMapping";
                var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                if(tableId == 0)
                {
                    DateMapping_CreateTable(meterId, meterType);
                }

                tableName = $"DateMapping_Temp";
                tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                if(tableId == 0)
                {
                    DateMapping_CreateTempTable(meterId, meterType);
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

            private void DateMapping_CreateTempTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.DateMapping_CreateTempTable, 
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
                var dateMappingDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.DateMapping_Delete, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, dateMappingDeleteStoredProcedure);
            }

            public void DateMapping_Insert(string meterType, long meterId, string processQueueGUID)
            {
                var dateMappingInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.DateMapping_Insert, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    dateMappingInsertStoredProcedure,
                    processQueueGUID);
            }

            public void InsertDateMapping(string meterType, long meterId, DataTable dateMappingDataTable, string processQueueGUID)
            {
                //Bulk Insert new Date Mapping into DateMapping_Temp table
                DateMappingTemp_Insert(meterType, meterId, dateMappingDataTable);

                //End date existing Date Mapping
                DateMapping_Delete(meterType, meterId);

                //Insert new Date Mapping into DateMapping table
                DateMapping_Insert(meterType, meterId, processQueueGUID);
            }

            public void DateMappingTemp_Insert(string meterType, long meterId, DataTable dateMappingDataTable)
            {
                new Methods().BulkInsert(dateMappingDataTable, $"[Supply.{meterType}{meterId}].[DateMapping_Temp]");
            }

            public List<Entity.Supply.DateMapping> DateMapping_GetLatest(string meterType, long meterId)
            {
                var dateMappingGetLatestStoredProcedure = string.Format(_storedProcedureSupplyEnums.DateMapping_GetLatest, meterType, meterId);

                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), dateMappingGetLatestStoredProcedure);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Supply.DateMapping(d)).ToList();
            }

            public Dictionary<long, long> DateMapping_GetLatestDictionary(string meterType, long meterId)
            {
                return DateMapping_GetLatest(meterType, meterId).ToDictionary(d => d.DateId, d => d.MappedDateId);
            }

            private void DateMapping_GrantExecuteToStoredProcedures(long meterId, string meterType)
            {
                foreach(var dateMappingStoredProcedure in _storedProcedureSupplyEnums.DateMappingStoredProcedureList)
                {
                    var storedProcedure = string.Format(dateMappingStoredProcedure, meterType, meterId);

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