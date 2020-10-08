using System.Collections.Generic;
using System.Data;
using System.Linq;

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

            public void InsertGranularSupplyForecast(DataTable dataTable, string meterType, long meterId, string granularityCode, string processQueueGUID)
            {
                var latestDataTable = dataTable.Copy();
                latestDataTable.Columns.Remove("CreatedByUserId");
                latestDataTable.Columns.Remove("SourceId");

                //Bulk insert into Latest Temp table
                ForecastUsageGranularityLatestTemp_Insert(meterType, meterId, granularityCode, latestDataTable);

                //Delete existing latest forecast
                ForecastUsageGranularityLatest_Delete(meterType, meterId, granularityCode);

                //Insert new latest forecast
                ForecastUsageGranularityLatest_Insert(meterType, meterId, granularityCode, processQueueGUID);

                var skip = 0;
                var dataTableRowCount = dataTable.Rows.Count;

                while(skip < dataTableRowCount)
                {
                    var newDataTable = dataTable.AsEnumerable().Skip(skip).Take(50000).CopyToDataTable();

                    //Bulk insert into History Temp table
                    ForecastUsageGranularityHistoryTemp_Insert(meterType, meterId, granularityCode, newDataTable);

                    //End date existing history forecast
                    ForecastUsageGranularityHistory_Delete(meterType, meterId, granularityCode);

                    //Insert new history forecast
                    ForecastUsageGranularityHistory_Insert(meterType, meterId, granularityCode, processQueueGUID);

                    skip += 50000;
                }
            }
        }
    }
}