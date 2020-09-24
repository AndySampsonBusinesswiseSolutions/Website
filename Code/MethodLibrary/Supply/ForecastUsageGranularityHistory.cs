using System.Collections.Generic;
using enums;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private readonly Methods.Supply _supplyMethods = new Methods.Supply();
            private readonly Enums.Information.Granularity.Attribute _informationGranularityAttributeEnums = new Enums.Information.Granularity.Attribute();

            private void CreateForecastUsageGranularityHistoryEntities(IEnumerable<long> granularityIdList, long schemaId, long meterId, string meterType)
            {
                foreach(var granularityId in granularityIdList)
                {
                    var tableName = SupplyForecastUsageTableName(granularityId, "History");
                    var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                    if(tableId == 0)
                    {
                        ForecastUsageGranularityHistory_CreateTable(meterId, granularityId, meterType);
                    }

                    ForecastUsageGranularityHistory_CreateDeleteStoredProcedure(meterId, granularityId, meterType);
                    ForecastUsageGranularityHistory_CreateInsertStoredProcedure(meterId, granularityId, meterType);
                    ForecastUsageGranularityHistory_GrantExecuteToStoredProcedures(meterId, granularityId, meterType);
                }
            }

            private void ForecastUsageGranularityHistory_CreateTable(long meterId, long granularityId, string meterType)
            {
                _supplyMethods.CreateSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryTableSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_CreateDeleteStoredProcedure(long meterId, long granularityId, string meterType)
            {
                _supplyMethods.CreateSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryDeleteStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_CreateInsertStoredProcedure(long meterId, long granularityId, string meterType)
            {
                _supplyMethods.CreateSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryInsertStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_GrantExecuteToStoredProcedures(long meterId, long granularityId, string meterType)
            {
                _supplyMethods.GrantExecuteToStoredProcedures(_storedProcedureSupplyEnums.ForecastUsageGranularityHistoryStoredProcedureList, granularityId, meterType, meterId);
            }
        }
    }
}