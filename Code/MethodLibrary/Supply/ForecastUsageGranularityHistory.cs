using System.Collections.Generic;
using enums;
using System.Data;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private readonly Enums.InformationSchema.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.InformationSchema.Granularity.Attribute();

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

                    ForecastUsageGranularityHistory_CreateGetLatestStoredProcedure(meterId, granularityId, meterType);
                    ForecastUsageGranularityHistory_GrantExecuteToStoredProcedures(meterId, granularityId, meterType);
                }
            }

            private void ForecastUsageGranularityHistory_CreateTable(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryTableSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_CreateGetLatestStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryGetLatestStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_GrantExecuteToStoredProcedures(long meterId, long granularityId, string meterType)
            {
                GrantExecuteToStoredProcedures(_storedProcedureSupplyEnums.ForecastUsageGranularityHistoryStoredProcedureList, granularityId, meterType, meterId);
            }

            public void ForecastUsageGranularityHistory_Insert(string meterType, long meterId, string granularityCode, DataTable forecastUsageGranularityHistoryDataTable)
            {
                new Methods().BulkInsert(forecastUsageGranularityHistoryDataTable, $"[Supply.{meterType}{meterId}].[ForecastUsage{granularityCode}History]");
            }
        }
    }
}