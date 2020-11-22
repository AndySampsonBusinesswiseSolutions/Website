using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Data;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class SupplySchema
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

                    ForecastUsageGranularityLatest_CreateGetLatestStoredProcedure(meterId, granularityId, meterType);
                    ForecastUsageGranularityLatest_GrantExecuteToStoredProcedures(meterId, granularityId, meterType);
                    GrantAlterTable(granularityId, meterType, meterId);
                }
            }

            private void ForecastUsageGranularityLatest_CreateTable(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageLatestTableSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityLatest_CreateGetLatestStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageLatestGetLatestStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityLatest_GrantExecuteToStoredProcedures(long meterId, long granularityId, string meterType)
            {
                GrantExecuteToStoredProcedures(_storedProcedureSupplyEnums.ForecastUsageGranularityLatestStoredProcedureList, granularityId, meterType, meterId);
            }

            public List<DataRow> ForecastUsageGranularityLatest_GetLatest(string meterType, long meterId, string granularityCode)
            {
                var forecastUsageGranularityLatestGetLatestStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityLatest_GetLatest, meterType, meterId, granularityCode);

                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), forecastUsageGranularityLatestGetLatestStoredProcedure);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<Tuple<long, long, decimal>> ForecastUsageGranularityLatest_GetLatestTuple(string meterType, long meterId, string granularityCode, string mainId, string additionalId)
            {
                return ForecastUsageGranularityLatest_GetLatest(meterType, meterId, granularityCode)
                    .Select(d => Tuple.Create((long)d[mainId], (long)d[additionalId], (decimal)d["Usage"])).ToList();
            }

            public List<Tuple<long, decimal>> ForecastUsageGranularityLatest_GetLatestTuple(string meterType, long meterId, string granularityCode, string mainId)
            {
                return ForecastUsageGranularityLatest_GetLatest(meterType, meterId, granularityCode)
                    .Select(d => Tuple.Create((long)d[mainId], (decimal)d["Usage"])).ToList();
            }

            public void ForecastUsageGranularityLatest_Insert(string meterType, long meterId, string granularityCode, DataTable forecastUsageGranularityLatestDataTable)
            {
                ExecuteSQL($"TRUNCATE TABLE [Supply.{meterType}{meterId}].[ForecastUsage{granularityCode}Latest]");
                new Methods().BulkInsert(forecastUsageGranularityLatestDataTable, $"[Supply.{meterType}{meterId}].[ForecastUsage{granularityCode}Latest]");
            }
        }
    }
}