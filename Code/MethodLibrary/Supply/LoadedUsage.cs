using System.Reflection;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using enums;
using System;

namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
            private static readonly Enums.StoredProcedure.Supply _storedProcedureSupplyEnums = new Enums.StoredProcedure.Supply();

            private void CreateLoadedUsageEntities(long schemaId, long meterId, string meterType)
            {
                var tableName = $"LoadedUsageHistory";
                var tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                if(tableId == 0)
                {
                    LoadedUsageHistory_CreateTable(meterId, meterType);
                }

                tableName = $"LoadedUsageLatest";
                tableId = Table_GetTableIdByTableNameAndSchemaId(tableName, schemaId);

                if(tableId == 0)
                {
                    LoadedUsageLatest_CreateTable(meterId, meterType);
                }

                LoadedUsageLatest_CreateGetListStoredProcedure(meterId, meterType);
                LoadedUsage_GrantExecuteToStoredProcedures(meterId, meterType);
                GrantAlterTable(meterType, meterId);
            }

            public void InsertLoadedUsage(long createdByUserId, long sourceId, long meterId, string meterType, long usageTypeId, Dictionary<long, Dictionary<long, decimal>> periodicUsageDictionary)
            {
                var loadedUsageHistoryDataTable = CreateLoadedUsageHistoryDataTable(createdByUserId, sourceId, usageTypeId);
                var loadedUsageLatestDataTable = CreateLoadedUsageLatestDataTable(loadedUsageHistoryDataTable);

                //Get latest loaded usage from database
                var loadedUsageLatestEntities = LoadedUsageLatest_GetList(meterType, meterId);

                //Convert periodicUsageDictionary to LoadedUsageLatest entities
                var periodicUsageLoadedUsageLatestEntities = periodicUsageDictionary.SelectMany(d => d.Value.Select(t => new Entity.Supply.LoadedUsageLatest(d.Key, t.Key, t.Value))).ToList();

                //update existing usages
                var updateLoadedUsageLatestEntities = periodicUsageLoadedUsageLatestEntities.Where(pulule => loadedUsageLatestEntities.Any(lule => lule.DateId == pulule.DateId && lule.TimePeriodId == pulule.TimePeriodId && lule.Usage != pulule.Usage)).ToList();
                foreach(var updateLoadedUsageLatestEntity in updateLoadedUsageLatestEntities)
                {
                    loadedUsageLatestEntities.First(lule => lule.DateId == updateLoadedUsageLatestEntity.DateId && lule.TimePeriodId == updateLoadedUsageLatestEntity.TimePeriodId).Usage = updateLoadedUsageLatestEntity.Usage;
                }

                //add new usages
                foreach(var periodicUsage in periodicUsageLoadedUsageLatestEntities)
                {
                    if(!loadedUsageLatestEntities.Any(lule => lule.DateId == periodicUsage.DateId && lule.TimePeriodId == periodicUsage.TimePeriodId))
                    {
                        loadedUsageLatestEntities.Add(periodicUsage);
                    }
                }

                //add all usages into loadedUsageLatestDataTable
                foreach (var periodicUsage in loadedUsageLatestEntities)
                {
                    var dataRow = loadedUsageLatestDataTable.NewRow();
                    dataRow["DateId"] = periodicUsage.DateId;
                    dataRow["TimePeriodId"] = periodicUsage.TimePeriodId;
                    dataRow["Usage"] = periodicUsage.Usage;
                    loadedUsageLatestDataTable.Rows.Add(dataRow);
                }

                //truncate latest table
                ExecuteSQL($"TRUNCATE TABLE [Supply.{meterType}{meterId}].[LoadedUsageLatest]");

                //insert latest usages
                LoadedUsage_Insert(meterType, meterId, loadedUsageLatestDataTable, true);

                //add all usages into loadedUsageHistoryDataTable
                foreach (var periodicUsage in periodicUsageLoadedUsageLatestEntities)
                {
                    var dataRow = loadedUsageHistoryDataTable.NewRow();
                    dataRow["DateId"] = periodicUsage.DateId;
                    dataRow["TimePeriodId"] = periodicUsage.TimePeriodId;
                    dataRow["Usage"] = periodicUsage.Usage;
                    loadedUsageHistoryDataTable.Rows.Add(dataRow);
                }

                //insert history usages
                LoadedUsage_Insert(meterType, meterId, loadedUsageHistoryDataTable, false);
            }

            private static DataTable CreateLoadedUsageHistoryDataTable(long createdByUserId, long sourceId, long usageTypeId)
            {
                //Create DataTable
                var dataTable = new DataTable();
                dataTable.Columns.Add("LoadedUsageId", typeof(long));
                dataTable.Columns.Add("CreatedDateTime", typeof(DateTime));
                dataTable.Columns.Add("CreatedByUserId", typeof(long));
                dataTable.Columns.Add("SourceId", typeof(long));
                dataTable.Columns.Add("DateId", typeof(long));
                dataTable.Columns.Add("TimePeriodId", typeof(long));
                dataTable.Columns.Add("UsageTypeId", typeof(long));
                dataTable.Columns.Add("Usage", typeof(decimal));

                //Set default values
                dataTable.Columns["CreatedDateTime"].DefaultValue = DateTime.UtcNow;
                dataTable.Columns["CreatedByUserId"].DefaultValue = createdByUserId;
                dataTable.Columns["SourceId"].DefaultValue = sourceId;
                dataTable.Columns["UsageTypeId"].DefaultValue = usageTypeId;
                return dataTable;
            }

            private static DataTable CreateLoadedUsageLatestDataTable(DataTable loadedUsageHistoryDataTable)
            {
                var dataTable = loadedUsageHistoryDataTable.Clone();
                dataTable.Columns.Remove("LoadedUsageId");
                dataTable.Columns.Remove("CreatedDateTime");
                dataTable.Columns.Remove("CreatedByUserId");
                dataTable.Columns.Remove("SourceId");
                dataTable.Columns.Remove("UsageTypeId");
                return dataTable;
            }

            private void LoadedUsageHistory_CreateTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsageHistory_CreateTable, 
                    meterId, meterType);
            }

            private void LoadedUsageLatest_CreateTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsageLatest_CreateTable, 
                    meterId, meterType);
            }

            private void LoadedUsageLatest_CreateGetListStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsageLatest_CreateGetListStoredProcedure, 
                    meterId, meterType);
            }

            public void LoadedUsage_Insert(string meterType, long meterId, DataTable loadedUsageDataTable, bool isLatest)
            {
                new Methods().BulkInsert(loadedUsageDataTable, $"[Supply.{meterType}{meterId}].[LoadedUsage{(isLatest ? "Latest" : "History")}]");
            }

            public List<Entity.Supply.LoadedUsageLatest> LoadedUsageLatest_GetList(string meterType, long meterId)
            {
                var loadedUsageGetLatestStoredProcedure = string.Format(_storedProcedureSupplyEnums.LoadedUsageLatest_GetList, meterType, meterId);

                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), loadedUsageGetLatestStoredProcedure);

                return dataTable.Rows.Cast<DataRow>().Select(d => new Entity.Supply.LoadedUsageLatest(d)).ToList();
            }

            public List<DataRow> LoadedUsage_GetLatest(string meterType, long meterId)
            {
                var loadedUsageGetLatestStoredProcedure = string.Format(_storedProcedureSupplyEnums.LoadedUsageLatest_GetList, meterType, meterId);

                var dataTable = GetDataTable(new List<ParameterInfo>().ToArray(), loadedUsageGetLatestStoredProcedure);

                return dataTable.Rows.Cast<DataRow>().ToList();
            }

            public List<Tuple<long, long, decimal>> LoadedUsage_GetLatestTuple(string meterType, long meterId)
            {
                var dataRows = LoadedUsage_GetLatest(meterType, meterId);

                var tuple = new List<Tuple<long, long, decimal>>();

                foreach (DataRow r in dataRows)
                {
                    var tup = Tuple.Create((long)r["DateId"], (long)r["TimePeriodId"], (decimal)r["Usage"]);
                    tuple.Add(tup);
                }

                return tuple;
            }

            private void LoadedUsage_GrantExecuteToStoredProcedures(long meterId, string meterType)
            {
                foreach(var loadedUsageStoredProcedure in _storedProcedureSupplyEnums.LoadedUsageStoredProcedureList)
                {
                    var storedProcedure = string.Format(loadedUsageStoredProcedure, meterType, meterId);

                    foreach(var api in _systemAPIRequireAccessToUsageEntitiesEnums.APIList)
                    {
                        var SQL = $"GRANT EXECUTE ON OBJECT::{storedProcedure} TO [{api}];";
                        ExecuteSQL(SQL);
                    }
                }
            }

            private void GrantAlterTable(string meterType, long meterId)
            {
                foreach(var api in _systemAPIRequireAccessToUsageEntitiesEnums.APIList)
                {
                    var SQL = $"GRANT ALTER ON OBJECT::[Supply.{meterType}{meterId}].[LoadedUsageLatest] TO [{api}];";
                    ExecuteSQL(SQL);
                }
            }
        }
    }
}