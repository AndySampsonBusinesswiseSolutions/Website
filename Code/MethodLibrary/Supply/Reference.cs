using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            public void CreateMeterTables(string schemaName, long meterId, string meterType)
            {
                //Get Granularities
                var granularityIdList = _informationMethods.Granularity_GetGranularityIdList();

                //Create Schema
                var schemaId = Schema_GetSchemaIdBySchemaName(schemaName);
                if(schemaId == 0)
                {
                    Schema_Create(meterId, meterType);
                }

                //Create ForecastUsageHistory tables and stored procedures by granularity
                CreateForecastUsageGranularityHistoryEntities(granularityIdList, schemaId, meterId, meterType);

                //Create ForecastUsageLatest tables and stored procedures by granularity
                CreateForecastUsageGranularityLatestEntities(granularityIdList, schemaId, meterId, meterType);

                //Create EstimatedUsage tables and stored procedures
                CreateEstimatedAnnualUsageEntities(schemaId, meterId, meterType);

                //Create LoadedUsage tables and stored procedures
                CreateLoadedUsageEntities(schemaId, meterId, meterType);

                //Create DateMapping tables and stored procedures
                CreateDateMappingEntities(schemaId, meterId, meterType);
            }

            public string SupplyForecastUsageTableName(long granularityId, string historyLatest)
            {
                var granularityCodeGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityCode);
                var granularityCode = _informationMethods.GranularityDetail_GetGranularityDetailDescriptionByGranularityIdAndGranularityAttributeId(granularityId, granularityCodeGranularityAttributeId);
                return $"ForecastUsage{granularityCode}{historyLatest}";
            }

            public void CreateGranularSupplyObject(long granularityId, string granularityAttributeDescription, string meterType, long meterId)
            {
                var granularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(granularityAttributeDescription);
                var SQL = _informationMethods.GranularityDetail_GetGranularityDetailDescriptionByGranularityIdAndGranularityAttributeId(granularityId, granularityAttributeId);
                SQL = SQL.Replace("Supply.X", $"Supply.{meterType}{meterId}");

                ExecuteScript(SQL);
            }

            public void GrantExecuteToStoredProcedures(List<string> storedProcedureList, long granularityId, string meterType, long meterId)
            {
                var granularityCodeGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityCode);
                var granularityCode = _informationMethods.GranularityDetail_GetGranularityDetailDescriptionByGranularityIdAndGranularityAttributeId(granularityId, granularityCodeGranularityAttributeId);

                foreach(var storedProcedure in storedProcedureList)
                {
                    var storedProcedureFullName = string.Format(storedProcedure, meterType, meterId, granularityCode);

                    foreach(var api in _systemAPIRequireAccessToUsageEntitiesEnums.APIList)
                    {
                        var SQL = $"GRANT EXECUTE ON OBJECT::{storedProcedureFullName} TO [{api}];";
                        ExecuteSQL(SQL);
                    }
                }
            }

            public void GrantAlterTable(long granularityId, string meterType, long meterId)
            {
                var granularityCodeGranularityAttributeId = _informationMethods.GranularityAttribute_GetGranularityAttributeIdByGranularityAttributeDescription(_informationGranularityAttributeEnums.GranularityCode);
                var granularityCode = _informationMethods.GranularityDetail_GetGranularityDetailDescriptionByGranularityIdAndGranularityAttributeId(granularityId, granularityCodeGranularityAttributeId);

                foreach(var api in _systemAPIRequireAccessToUsageEntitiesEnums.APIList)
                {
                    var SQL = $"GRANT ALTER ON OBJECT::[Supply.{meterType}{meterId}].[ForecastUsage{granularityCode}Latest] TO [{api}];";
                    ExecuteSQL(SQL);
                }
            }

            public void CreateGranularSupplyForecastDataTables(string meterType, long meterId, string granularityCode, long createdByUserId, long sourceId, List<string> columnNames, List<Tuple<long, long, decimal>> newForecasts, List<Tuple<long, long, decimal>> existingForecasts)
            {
                //Create DataTable
                var dataTable = CreateHistoryForecastDataTable(granularityCode, columnNames, createdByUserId, sourceId);
                PopulateForecastDataTable(columnNames, newForecasts, dataTable);

                //Setup latest forecast
                var latestForecastDataTable = CreateLatestForecastDataTable(dataTable, granularityCode);
                PopulateForecastDataTable(columnNames, existingForecasts, latestForecastDataTable);

                //Insert into history and latest tables
                InsertGranularSupplyForecasts(dataTable, latestForecastDataTable, meterType, meterId, granularityCode);
            }

            public void CreateGranularSupplyForecastDataTables(string meterType, long meterId, string granularityCode, long createdByUserId, long sourceId, List<string> columnNames, List<Tuple<long, decimal>> newForecasts, List<Tuple<long, decimal>> existingForecasts)
            {
                //Create DataTable
                var dataTable = CreateHistoryForecastDataTable(granularityCode, columnNames, createdByUserId, sourceId);
                PopulateForecastDataTable(columnNames, newForecasts, dataTable);

                //Setup latest forecast
                var latestForecastDataTable = CreateLatestForecastDataTable(dataTable, granularityCode);
                PopulateForecastDataTable(columnNames, existingForecasts, latestForecastDataTable);

                //Insert into history and latest tables
                InsertGranularSupplyForecasts(dataTable, latestForecastDataTable, meterType, meterId, granularityCode);
            }

            public void InsertGranularSupplyForecasts(DataTable dataTable, DataTable latestForecastDataTable, string meterType, long meterId, string granularityCode)
            {
                //Bulk insert into History table
                ForecastUsageGranularityHistory_Insert(meterType, meterId, granularityCode, dataTable);

                //Bulk insert into Latest table
                ForecastUsageGranularityLatest_Insert(meterType, meterId, granularityCode, latestForecastDataTable);
            }

            public DataTable CreateHistoryForecastDataTable(string granularityCode, List<string> idList, long createdByUserId, long sourceId)
            {
                //Create DataTable
                var dataTable = new DataTable();
                dataTable.Columns.Add($"ForecastUsage{granularityCode}HistoryId", typeof(long));
                dataTable.Columns.Add("CreatedDateTime", typeof(DateTime));
                dataTable.Columns.Add("CreatedByUserId", typeof(long));
                dataTable.Columns.Add("SourceId", typeof(long));

                foreach(var id in idList)
                {
                    dataTable.Columns.Add(id, typeof(long));
                }
                
                dataTable.Columns.Add("Usage", typeof(decimal));

                //Set default values
                dataTable.Columns["CreatedDateTime"].DefaultValue = DateTime.UtcNow;
                dataTable.Columns["CreatedByUserId"].DefaultValue = createdByUserId;
                dataTable.Columns["SourceId"].DefaultValue = sourceId;

                return dataTable;
            }

            public DataTable CreateLatestForecastDataTable(DataTable dataTable, string granularityCode)
            {
                var latestForecastDataTable = dataTable.Clone();
                latestForecastDataTable.Columns.Remove($"ForecastUsage{granularityCode}HistoryId");
                latestForecastDataTable.Columns.Remove("CreatedDateTime");
                latestForecastDataTable.Columns.Remove("CreatedByUserId");
                latestForecastDataTable.Columns.Remove("SourceId");

                return latestForecastDataTable;
            }

            public void PopulateForecastDataTable(List<string> columnNames, List<Tuple<long, long, decimal>> forecasts, DataTable dataTable)
            {
                foreach (var forecast in forecasts)
                {
                    AddToForecastDataTable(dataTable, columnNames, new List<long>{forecast.Item1, forecast.Item2}, forecast.Item3);
                }
            }

            public void PopulateForecastDataTable(List<string> columnNames, List<Tuple<long, decimal>> forecasts, DataTable dataTable)
            {
                foreach (var forecast in forecasts)
                {
                    AddToForecastDataTable(dataTable, columnNames, new List<long>{forecast.Item1}, forecast.Item2);
                }
            }

            public void AddToForecastDataTable(DataTable dataTable, List<string> columnNames, List<long> ids, decimal usage)
            {
                var dataRow = dataTable.NewRow();

                for(var i = 0; i < columnNames.Count; i++)
                {
                    dataRow[columnNames[i]] = ids[i];
                }

                dataRow["Usage"] = usage;
                dataTable.Rows.Add(dataRow);
            }

            public decimal GetUsageByUsageType(List<Tuple<long, long, long, decimal>> usageForTimePeriodList)
            {
                var usageTypePriority = new Dictionary<long, long>{{1, 3}, {2, 2}, {3, 4}, {4, 1}}; //TODO: Resolve
                return usageForTimePeriodList.ToDictionary(u => usageTypePriority.First(ut => ut.Value == u.Item3).Key, u => u.Item4)
                    .OrderBy(u => u.Key).First().Value;
            }

            public void SetForecastValue(Dictionary<long, decimal> forecast, Dictionary<long, bool> forecastFound, long timePeriodId, decimal usage)
            {
                //Add usage to forecast
                forecast[timePeriodId] = Math.Round(usage, 10);
                forecastFound[timePeriodId] = true;
            }
        }
    }
}