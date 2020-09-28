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
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryTableSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_CreateDeleteStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryDeleteStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_CreateInsertStoredProcedure(long meterId, long granularityId, string meterType)
            {
                CreateGranularSupplyObject(granularityId, _informationGranularityAttributeEnums.ForecastUsageHistoryInsertStoredProcedureSQL, meterType, meterId);
            }

            private void ForecastUsageGranularityHistory_GrantExecuteToStoredProcedures(long meterId, long granularityId, string meterType)
            {
                GrantExecuteToStoredProcedures(_storedProcedureSupplyEnums.ForecastUsageGranularityHistoryStoredProcedureList, granularityId, meterType, meterId);
            }

            public void ForecastUsageGranularityHistory_Delete(string meterType, long meterId, string granularityCode, KeyValuePair<long, long> parameterIds)
            {
                var forecastUsageGranularityHistoryDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityHistory_Delete, meterType, meterId, granularityCode);
                
                switch(granularityCode)
                {
                    case "FiveMinute":
                        ForecastUsageFiveMinuteHistory_Delete(forecastUsageGranularityHistoryDeleteStoredProcedure, parameterIds.Key, parameterIds.Value);
                        break;
                    case "HalfHour":
                        ForecastUsageHalfHourHistory_Delete(forecastUsageGranularityHistoryDeleteStoredProcedure, parameterIds.Key, parameterIds.Value);
                        break;
                    case "Date":
                        ForecastUsageDateHistory_Delete(forecastUsageGranularityHistoryDeleteStoredProcedure, parameterIds.Key);
                        break;
                    case "Week":
                        ForecastUsageWeekHistory_Delete(forecastUsageGranularityHistoryDeleteStoredProcedure, parameterIds.Key, parameterIds.Value);
                        break;
                    case "Month":
                        ForecastUsageMonthHistory_Delete(forecastUsageGranularityHistoryDeleteStoredProcedure, parameterIds.Key, parameterIds.Value);
                        break;
                    case "Quarter":
                        ForecastUsageQuarterHistory_Delete(forecastUsageGranularityHistoryDeleteStoredProcedure, parameterIds.Key, parameterIds.Value);
                        break;
                    case "Year":
                        ForecastUsageYearHistory_Delete(forecastUsageGranularityHistoryDeleteStoredProcedure, parameterIds.Key);
                        break;
                }
            }

            private void ForecastUsageFiveMinuteHistory_Delete(string storedProcedure, long dateId, long timePeriodId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, dateId, timePeriodId);
            }

            private void ForecastUsageHalfHourHistory_Delete(string storedProcedure, long dateId, long timePeriodId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, dateId, timePeriodId);
            }

            private void ForecastUsageDateHistory_Delete(string storedProcedure, long dateId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, dateId);
            }

            private void ForecastUsageWeekHistory_Delete(string storedProcedure, long yearId, long weekId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId, weekId);
            }

            private void ForecastUsageMonthHistory_Delete(string storedProcedure, long yearId, long monthId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId, monthId);
            }

            private void ForecastUsageQuarterHistory_Delete(string storedProcedure, long yearId, long quarterId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId, quarterId);
            }

            private void ForecastUsageYearHistory_Delete(string storedProcedure, long yearId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId);
            }

            public void ForecastUsageGranularityHistory_Insert(string meterType, long meterId, string granularityCode, long createdByUserId, long sourceId, KeyValuePair<long, long> parameterIds, decimal usage)
            {
                var forecastUsageGranularityHistoryInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityHistory_Insert, meterType, meterId, granularityCode);
                
                switch(granularityCode)
                {
                    case "FiveMinute":
                        ForecastUsageFiveMinuteHistory_Insert(forecastUsageGranularityHistoryInsertStoredProcedure, createdByUserId, sourceId, parameterIds.Key, parameterIds.Value, usage);
                        break;
                    case "HalfHour":
                        ForecastUsageHalfHourHistory_Insert(forecastUsageGranularityHistoryInsertStoredProcedure, createdByUserId, sourceId, parameterIds.Key, parameterIds.Value, usage);
                        break;
                    case "Date":
                        ForecastUsageDateHistory_Insert(forecastUsageGranularityHistoryInsertStoredProcedure, createdByUserId, sourceId, parameterIds.Key, usage);
                        break;
                    case "Week":
                        ForecastUsageWeekHistory_Insert(forecastUsageGranularityHistoryInsertStoredProcedure, createdByUserId, sourceId, parameterIds.Key, parameterIds.Value, usage);
                        break;
                    case "Month":
                        ForecastUsageMonthHistory_Insert(forecastUsageGranularityHistoryInsertStoredProcedure, createdByUserId, sourceId, parameterIds.Key, parameterIds.Value, usage);
                        break;
                    case "Quarter":
                        ForecastUsageQuarterHistory_Insert(forecastUsageGranularityHistoryInsertStoredProcedure, createdByUserId, sourceId, parameterIds.Key, parameterIds.Value, usage);
                        break;
                    case "Year":
                        ForecastUsageYearHistory_Insert(forecastUsageGranularityHistoryInsertStoredProcedure, createdByUserId, sourceId, parameterIds.Key, usage);
                        break;
                }
            }

            private void ForecastUsageFiveMinuteHistory_Insert(string storedProcedure, long createdByUserId, long sourceId, long dateId, long timePeriodId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, createdByUserId, sourceId, dateId, timePeriodId, usage);
            }

            private void ForecastUsageHalfHourHistory_Insert(string storedProcedure, long createdByUserId, long sourceId, long dateId, long timePeriodId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, createdByUserId, sourceId, dateId, timePeriodId, usage);
            }

            private void ForecastUsageDateHistory_Insert(string storedProcedure, long createdByUserId, long sourceId, long dateId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, createdByUserId, sourceId, dateId, usage);
            }

            private void ForecastUsageWeekHistory_Insert(string storedProcedure, long createdByUserId, long sourceId, long yearId, long weekId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, createdByUserId, sourceId, yearId, weekId, usage);
            }

            private void ForecastUsageMonthHistory_Insert(string storedProcedure, long createdByUserId, long sourceId, long yearId, long monthId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, createdByUserId, sourceId, yearId, monthId, usage);
            }

            private void ForecastUsageQuarterHistory_Insert(string storedProcedure, long createdByUserId, long sourceId, long yearId, long quarterId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, createdByUserId, sourceId, yearId, quarterId, usage);
            }

            private void ForecastUsageYearHistory_Insert(string storedProcedure, long createdByUserId, long sourceId, long yearId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, createdByUserId, sourceId, yearId, usage);
            }

            public IEnumerable<DataRow> ForecastUsageGranularityHistory_GetLatest(string meterType, long meterId, string granularityCode)
            {
                var forecastUsageGranularityHistoryGetLatestStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityHistory_GetLatest, meterType, meterId, granularityCode);

                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), forecastUsageGranularityHistoryGetLatestStoredProcedure);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }
        }
    }
}