using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private void CreateForecastUsageGranularityHistoryEntities(IEnumerable<string> granularityCodeList, long schemaId, long meterId, string meterType)
            {
                foreach(var granularityCode in granularityCodeList)
                {
                    var tableName = $"ForecastUsage{granularityCode}History";
                    var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                    if(tableId == 0)
                    {
                        ForecastUsageGranularityHistory_CreateTable(meterId, granularityCode, meterType);
                    }

                    ForecastUsageGranularityHistory_CreateDeleteStoredProcedure(meterId, granularityCode, meterType);
                    ForecastUsageGranularityHistory_CreateInsertStoredProcedure(meterId, granularityCode, meterType);
                    ForecastUsageGranularityHistory_GrantExecuteToStoredProcedures(meterId, granularityCode, meterType);
                }
            }

            private void ForecastUsageGranularityHistory_CreateTable(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityHistory_CreateTable, 
                    meterId, granularityCode, meterType);
            }

            private void ForecastUsageGranularityHistory_CreateDeleteStoredProcedure(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityHistory_CreateDeleteStoredProcedure, 
                    meterId, granularityCode, meterType);
            }

            private void ForecastUsageGranularityHistory_CreateInsertStoredProcedure(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityHistory_CreateInsertStoredProcedure, 
                    meterId, granularityCode, meterType);
            }

            private void ForecastUsageGranularityHistory_GrantExecuteToStoredProcedures(long meterId, string granularityCode, string meterType)
            {
                foreach(var forecastUsageGranularityHistoryStoredProcedure in _storedProcedureSupplyEnums.ForecastUsageGranularityHistoryStoredProcedureList)
                {
                    var storedProcedure = string.Format(forecastUsageGranularityHistoryStoredProcedure, meterType, meterId, granularityCode);

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