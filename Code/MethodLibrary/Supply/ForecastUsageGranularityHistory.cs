using System.Collections.Generic;
using enums;
using System.Reflection;
using System.Linq;
using System.Data;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private readonly Enums.Information.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();

            private void CreateForecastUsageGranularityHistoryEntities(List<long> granularityIdList, long schemaId, long meterId, string meterType)
            {
                foreach(var granularityId in granularityIdList)
                {
                    var tableName = SupplyForecastUsageTableName(granularityId, "History");
                    var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                    if(tableId == 0)
                    {
                        ForecastUsageGranularityHistory_CreateTable(meterId, granularityId, meterType);
                    }

                    tableName = SupplyForecastUsageTableName(granularityId, "History_Temp");
                    tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                    if(tableId == 0)
                    {
                        ForecastUsageGranularityHistory_CreateTempTable(meterId, granularityId, meterType);
                    }

                    ForecastUsageGranularityHistory_CreateDeleteStoredProcedure(meterId, granularityId, meterType);
                    ForecastUsageGranularityHistory_CreateInsertStoredProcedure(meterId, granularityId, meterType);
                    ForecastUsageGranularityHistory_CreateGetLatestStoredProcedure(meterId, granularityId, meterType);
                    ForecastUsageGranularityHistory_GrantExecuteToStoredProcedures(meterId, granularityId, meterType);
                }
            }

            private void ForecastUsageGranularityHistory_CreateTable(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryTableSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_CreateTempTable(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryTempTableSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_CreateDeleteStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryDeleteStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_CreateInsertStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryInsertStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_CreateGetLatestStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryGetLatestStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_GrantExecuteToStoredProcedures(long meterId, long granularityId, string meterType)
            {
                GrantExecuteToStoredProcedures(_storedProcedureSupplyEnums.ForecastUsageGranularityHistoryStoredProcedureList, granularityId, meterType, meterId);
            }

            public void ForecastUsageGranularityHistory_Delete(string meterType, long meterId, string granularityCode)
            {
                var forecastUsageGranularityHistoryDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityHistory_Delete, meterType, meterId, granularityCode);
                
                ExecuteNonQuery(new List<ParameterInfo>().ToArray(), forecastUsageGranularityHistoryDeleteStoredProcedure);
            }

            public void ForecastUsageGranularityHistory_Insert(string meterType, long meterId, string granularityCode, string processQueueGUID)
            {
                var forecastUsageGranularityHistoryInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityHistory_Insert, meterType, meterId, granularityCode);
                
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name == "processQueueGUID").ToArray();

                ExecuteNonQuery(parameterInfoList, forecastUsageGranularityHistoryInsertStoredProcedure, processQueueGUID);
            }

            public List<DataRow> ForecastUsageGranularityHistory_GetHistory(string meterType, long meterId, string granularityCode)
            {
                var forecastUsageGranularityHistoryGetHistoryStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityHistory_GetLatest, meterType, meterId, granularityCode);

                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), forecastUsageGranularityHistoryGetHistoryStoredProcedure);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public void ForecastUsageGranularityHistoryTemp_Insert(string meterType, long meterId, string granularityCode, DataTable forecastUsageGranularityHistoryDataTable)
            {
                new Methods().BulkInsert(forecastUsageGranularityHistoryDataTable, $"[Supply.{meterType}{meterId}].[ForecastUsage{granularityCode}History_Temp]");
            }
        }
    }
}