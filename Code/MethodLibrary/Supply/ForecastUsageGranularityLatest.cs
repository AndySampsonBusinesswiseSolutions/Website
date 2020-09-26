using System.Reflection;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private void CreateForecastUsageGranularityLatestEntities(IEnumerable<long> granularityIdList, long schemaId, long meterId, string meterType)
            {
                foreach(var granularityId in granularityIdList)
                {
                    var tableName = SupplyForecastUsageTableName(granularityId, "Latest");
                    var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                    if(tableId == 0)
                    {
                        ForecastUsageGranularityLatest_CreateTable(meterId, granularityId, meterType);
                    }

                    ForecastUsageGranularityLatest_CreateDeleteStoredProcedure(meterId, granularityId, meterType);
                    ForecastUsageGranularityLatest_CreateInsertStoredProcedure(meterId, granularityId, meterType);
                    ForecastUsageGranularityLatest_GrantExecuteToStoredProcedures(meterId, granularityId, meterType);
                }
            }

            private void ForecastUsageGranularityLatest_CreateTable(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageLatestTableSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityLatest_CreateDeleteStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageLatestDeleteStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityLatest_CreateInsertStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageLatestInsertStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityLatest_GrantExecuteToStoredProcedures(long meterId, long granularityId, string meterType)
            {
                GrantExecuteToStoredProcedures(_storedProcedureSupplyEnums.ForecastUsageGranularityLatestStoredProcedureList, granularityId, meterType, meterId);
            }
        }
    }
}