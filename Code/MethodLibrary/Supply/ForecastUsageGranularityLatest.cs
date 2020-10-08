using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Data;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private void CreateForecastUsageGranularityLatestEntities(List<long> granularityIdList, long schemaId, long meterId, string meterType)
            {
                foreach(var granularityId in granularityIdList)
                {
                    var tableName = SupplyForecastUsageTableName(granularityId, "Latest");
                    var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                    if(tableId == 0)
                    {
                        ForecastUsageGranularityLatest_CreateTable(meterId, granularityId, meterType);
                    }

                    // tableName = SupplyForecastUsageTableName(granularityId, "Latest_Temp");
                    // tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                    // if(tableId == 0)
                    // {
                    //     ForecastUsageGranularityLatest_CreateTempTable(meterId, granularityId, meterType);
                    // }

                    // ForecastUsageGranularityLatest_CreateDeleteStoredProcedure(meterId, granularityId, meterType);
                    // ForecastUsageGranularityLatest_CreateInsertStoredProcedure(meterId, granularityId, meterType);
                    ForecastUsageGranularityLatest_CreateGetLatestStoredProcedure(meterId, granularityId, meterType);
                    ForecastUsageGranularityLatest_GrantExecuteToStoredProcedures(meterId, granularityId, meterType);
                }
            }

            private void ForecastUsageGranularityLatest_CreateTable(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageLatestTableSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityLatest_CreateTempTable(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageLatestTempTableSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityLatest_CreateDeleteStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageLatestDeleteStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityLatest_CreateInsertStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageLatestInsertStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityLatest_CreateGetLatestStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageLatestGetLatestStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityLatest_GrantExecuteToStoredProcedures(long meterId, long granularityId, string meterType)
            {
                GrantExecuteToStoredProcedures(_storedProcedureSupplyEnums.ForecastUsageGranularityLatestStoredProcedureList, granularityId, meterType, meterId);
            }

            public void ForecastUsageGranularityLatest_Delete(string meterType, long meterId, string granularityCode)
            {
                var forecastUsageGranularityLatestDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityLatest_Delete, meterType, meterId, granularityCode);
                
                ExecuteNonQuery(new List<ParameterInfo>().ToArray(), forecastUsageGranularityLatestDeleteStoredProcedure);
            }

            public void ForecastUsageGranularityLatest_Insert(string meterType, long meterId, string granularityCode, string processQueueGUID)
            {
                var forecastUsageGranularityLatestInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityLatest_Insert, meterType, meterId, granularityCode);
                
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name == "processQueueGUID").ToArray();

                ExecuteNonQuery(parameterInfoList, forecastUsageGranularityLatestInsertStoredProcedure, processQueueGUID);
            }

            public List<DataRow> ForecastUsageGranularityLatest_GetLatest(string meterType, long meterId, string granularityCode)
            {
                var forecastUsageGranularityLatestGetLatestStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityLatest_GetLatest, meterType, meterId, granularityCode);

                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), forecastUsageGranularityLatestGetLatestStoredProcedure);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<Tuple<long, long, decimal>> ForecastUsageGranularityLatest_GetLatestTuple(string meterType, long meterId, string granularityCode, string mainId, string additionalId)
            {
                var dataRows = ForecastUsageGranularityLatest_GetLatest(meterType, meterId, granularityCode);

                var tuple = new List<Tuple<long, long, decimal>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r[mainId], (long)r[additionalId], (decimal)r["Usage"]);
                    tuple.Add(tup);
                }

                return tuple;
            }

            public List<Tuple<long, decimal>> ForecastUsageGranularityLatest_GetLatestTuple(string meterType, long meterId, string granularityCode, string mainId)
            {
                var dataRows = ForecastUsageGranularityLatest_GetLatest(meterType, meterId, granularityCode);

                var tuple = new List<Tuple<long, decimal>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r[mainId], (decimal)r["Usage"]);
                    tuple.Add(tup);
                }

                return tuple;
            }

            public void ForecastUsageGranularityLatest_Insert(string meterType, long meterId, string granularityCode, DataTable forecastUsageGranularityLatestDataTable)
            {
                new Methods().BulkInsert(forecastUsageGranularityLatestDataTable, $"[Supply.{meterType}{meterId}].[ForecastUsage{granularityCode}Latest]");
            }
        }
    }
}