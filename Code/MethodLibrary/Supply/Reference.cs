using System.Collections.Generic;
using System.Data;
using System;

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

            public void InsertGranularSupplyForecast(DataTable dataTable, DataTable latestForecastDataTable, string meterType, long meterId, string granularityCode)
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
        }
    }
}