using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private void CreateForecastUsageGranularityLatestEntities(IEnumerable<string> granularityCodeList, long schemaId, long meterId, string meterType)
            {
                foreach(var granularityCode in granularityCodeList)
                {
                    var tableName = $"ForecastUsage{granularityCode}Latest";
                    var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                    if(tableId == 0)
                    {
                        ForecastUsageGranularityLatest_CreateTable(meterId, granularityCode, meterType);
                    }

                    ForecastUsageGranularityLatest_CreateDeleteStoredProcedure(meterId, granularityCode, meterType);
                    ForecastUsageGranularityLatest_CreateInsertStoredProcedure(meterId, granularityCode, meterType);
                    ForecastUsageGranularityLatest_GrantExecuteToStoredProcedures(meterId, granularityCode, meterType);
                }
            }

            private void ForecastUsageGranularityLatest_CreateTable(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityLatest_CreateTable, 
                    meterId, granularityCode, meterType);
            }

            private void ForecastUsageGranularityLatest_CreateDeleteStoredProcedure(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityLatest_CreateDeleteStoredProcedure, 
                    meterId, granularityCode, meterType);
            }

            private void ForecastUsageGranularityLatest_CreateInsertStoredProcedure(long meterId, string granularityCode, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.ForecastUsageGranularityLatest_CreateInsertStoredProcedure, 
                    meterId, granularityCode, meterType);
            }

            private void ForecastUsageGranularityLatest_GrantExecuteToStoredProcedures(long meterId, string granularityCode, string meterType)
            {
                foreach(var forecastUsageGranularityLatestStoredProcedure in _storedProcedureSupplyEnums.ForecastUsageGranularityLatestStoredProcedureList)
                {
                    var storedProcedure = string.Format(forecastUsageGranularityLatestStoredProcedure, meterType, meterId, granularityCode);

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