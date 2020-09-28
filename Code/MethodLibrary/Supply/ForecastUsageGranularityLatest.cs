using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Data;

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

            public void ForecastUsageGranularityLatest_Delete(string meterType, long meterId, string granularityCode, KeyValuePair<long, long> parameterIds)
            {
                var forecastUsageGranularityLatestDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityLatest_Delete, meterType, meterId, granularityCode);
                
                switch(granularityCode)
                {
                    case "FiveMinute":
                        ForecastUsageFiveMinuteLatest_Delete(forecastUsageGranularityLatestDeleteStoredProcedure, parameterIds.Key, parameterIds.Value);
                        break;
                    case "HalfHour":
                        ForecastUsageHalfHourLatest_Delete(forecastUsageGranularityLatestDeleteStoredProcedure, parameterIds.Key, parameterIds.Value);
                        break;
                    case "Date":
                        ForecastUsageDateLatest_Delete(forecastUsageGranularityLatestDeleteStoredProcedure, parameterIds.Key);
                        break;
                    case "Week":
                        ForecastUsageWeekLatest_Delete(forecastUsageGranularityLatestDeleteStoredProcedure, parameterIds.Key, parameterIds.Value);
                        break;
                    case "Month":
                        ForecastUsageMonthLatest_Delete(forecastUsageGranularityLatestDeleteStoredProcedure, parameterIds.Key, parameterIds.Value);
                        break;
                    case "Quarter":
                        ForecastUsageQuarterLatest_Delete(forecastUsageGranularityLatestDeleteStoredProcedure, parameterIds.Key, parameterIds.Value);
                        break;
                    case "Year":
                        ForecastUsageYearLatest_Delete(forecastUsageGranularityLatestDeleteStoredProcedure, parameterIds.Key);
                        break;
                }
            }

            private void ForecastUsageFiveMinuteLatest_Delete(string storedProcedure, long dateId, long timePeriodId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, dateId, timePeriodId);
            }

            private void ForecastUsageHalfHourLatest_Delete(string storedProcedure, long dateId, long timePeriodId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, dateId, timePeriodId);
            }

            private void ForecastUsageDateLatest_Delete(string storedProcedure, long dateId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, dateId);
            }

            private void ForecastUsageWeekLatest_Delete(string storedProcedure, long yearId, long weekId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId, weekId);
            }

            private void ForecastUsageMonthLatest_Delete(string storedProcedure, long yearId, long monthId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId, monthId);
            }

            private void ForecastUsageQuarterLatest_Delete(string storedProcedure, long yearId, long quarterId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId, quarterId);
            }

            private void ForecastUsageYearLatest_Delete(string storedProcedure, long yearId)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId);
            }

            public void ForecastUsageGranularityLatest_Insert(string meterType, long meterId, string granularityCode, KeyValuePair<long, long> parameterIds, decimal usage)
            {
                var forecastUsageGranularityLatestInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityLatest_Insert, meterType, meterId, granularityCode);
                
                switch(granularityCode)
                {
                    case "FiveMinute":
                        ForecastUsageFiveMinuteLatest_Insert(forecastUsageGranularityLatestInsertStoredProcedure, parameterIds.Key, parameterIds.Value, usage);
                        break;
                    case "HalfHour":
                        ForecastUsageHalfHourLatest_Insert(forecastUsageGranularityLatestInsertStoredProcedure, parameterIds.Key, parameterIds.Value, usage);
                        break;
                    case "Date":
                        ForecastUsageDateLatest_Insert(forecastUsageGranularityLatestInsertStoredProcedure, parameterIds.Key, usage);
                        break;
                    case "Week":
                        ForecastUsageWeekLatest_Insert(forecastUsageGranularityLatestInsertStoredProcedure, parameterIds.Key, parameterIds.Value, usage);
                        break;
                    case "Month":
                        ForecastUsageMonthLatest_Insert(forecastUsageGranularityLatestInsertStoredProcedure, parameterIds.Key, parameterIds.Value, usage);
                        break;
                    case "Quarter":
                        ForecastUsageQuarterLatest_Insert(forecastUsageGranularityLatestInsertStoredProcedure, parameterIds.Key, parameterIds.Value, usage);
                        break;
                    case "Year":
                        ForecastUsageYearLatest_Insert(forecastUsageGranularityLatestInsertStoredProcedure, parameterIds.Key, usage);
                        break;
                }
            }

            private void ForecastUsageFiveMinuteLatest_Insert(string storedProcedure, long dateId, long timePeriodId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, dateId, timePeriodId, usage);
            }

            private void ForecastUsageHalfHourLatest_Insert(string storedProcedure, long dateId, long timePeriodId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, dateId, timePeriodId, usage);
            }

            private void ForecastUsageDateLatest_Insert(string storedProcedure, long dateId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, dateId, usage);
            }

            private void ForecastUsageWeekLatest_Insert(string storedProcedure, long yearId, long weekId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId, weekId, usage);
            }

            private void ForecastUsageMonthLatest_Insert(string storedProcedure, long yearId, long monthId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId, monthId, usage);
            }

            private void ForecastUsageQuarterLatest_Insert(string storedProcedure, long yearId, long quarterId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId, quarterId, usage);
            }

            private void ForecastUsageYearLatest_Insert(string storedProcedure, long yearId, decimal usage)
            {
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "storedProcedure").ToArray();

                ExecuteNonQuery(parameterInfoList, storedProcedure, yearId, usage);
            }

            public IEnumerable<DataRow> ForecastUsageGranularityLatest_GetLatest(string meterType, long meterId, string granularityCode)
            {
                var forecastUsageGranularityLatestGetLatestStoredProcedure = string.Format(_storedProcedureSupplyEnums.ForecastUsageGranularityLatest_GetLatest, meterType, meterId, granularityCode);

                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), forecastUsageGranularityLatestGetLatestStoredProcedure);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }
        }
    }
}