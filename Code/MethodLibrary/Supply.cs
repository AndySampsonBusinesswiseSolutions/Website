using System.Reflection;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace MethodLibrary
{
    public partial class Methods
    {
        public class Supply
        {
            private void Schema_Create(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.Schema_Create, 
                    meterId, meterType);
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

            public void EstimatedAnnualUsage_CreateTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.EstimatedAnnualUsage_CreateTable, 
                    meterId, meterType);
            }

            private void EstimatedAnnualUsage_CreateDeleteStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.EstimatedAnnualUsage_CreateDeleteStoredProcedure, 
                    meterId, meterType);
            }

            private void EstimatedAnnualUsage_CreateInsertStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.EstimatedAnnualUsage_CreateInsertStoredProcedure, 
                    meterId, meterType);
            }

            private void LoadedUsage_CreateTable(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateTable, 
                    meterId, meterType);
            }

            private void LoadedUsage_CreateDeleteStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateDeleteStoredProcedure, 
                    meterId, meterType);
            }

            private void LoadedUsage_CreateInsertStoredProcedure(long meterId, string meterType)
            {
                ExecuteNonQuery(MethodBase.GetCurrentMethod().GetParameters(),
                    _storedProcedureSupplyEnums.LoadedUsage_CreateInsertStoredProcedure, 
                    meterId, meterType);
            }

            private long Schema_GetSchemaIdBySchemaName(string schemaName)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSupplyEnums.Schema_GetBySchemaName,
                    schemaName);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("schema_id"))
                    .FirstOrDefault();
            }

            private long Table_GetTableIdByTableNameAndSchemaId(string tableName, long schemaId)
            {
                var dataTable = GetDataTable(MethodBase.GetCurrentMethod().GetParameters(), 
                    _storedProcedureSupplyEnums.Table_GetByTableNameAndSchemaId,
                    tableName, schemaId);

                return dataTable.AsEnumerable()
                    .Select(r => r.Field<long>("object_id"))
                    .FirstOrDefault();
            }

            public void CreateMeterTables(string schemaName, long meterId, string meterType)
            {
                //Get Granularities
                var granularityCodeList = _informationMethods.Granularity_GetGranularityCodeList();

                //Create Schema
                var schemaId = Schema_GetSchemaIdBySchemaName(schemaName);
                if(schemaId == 0)
                {
                    Schema_Create(meterId, meterType);
                }

                //Create ForecastUsage table and stored procedures by granularity
                foreach(var granularityCode in granularityCodeList)
                {
                    var latestTableName = $"ForecastUsage{granularityCode}Latest";
                    var latestTableId = Table_GetTableIdByTableNameAndSchemaId(latestTableName, schemaId);

                    var historyTableName = $"ForecastUsage{granularityCode}History";
                    var historyTableId = Table_GetTableIdByTableNameAndSchemaId(historyTableName, schemaId);

                    //If tables don't exist, create them
                    if(latestTableId == 0)
                    {
                        ForecastUsageGranularityLatest_CreateTable(meterId, granularityCode, meterType);
                    }
                    
                    if(historyTableId == 0)
                    {
                        ForecastUsageGranularityHistory_CreateTable(meterId, granularityCode, meterType);
                    }

                    //Create/Amend stored procedures
                    ForecastUsageGranularityLatest_CreateDeleteStoredProcedure(meterId, granularityCode, meterType);
                    ForecastUsageGranularityLatest_CreateInsertStoredProcedure(meterId, granularityCode, meterType);
                    ForecastUsageGranularityHistory_CreateDeleteStoredProcedure(meterId, granularityCode, meterType);
                    ForecastUsageGranularityHistory_CreateInsertStoredProcedure(meterId, granularityCode, meterType);
                }

                //Create EstimatedUsage and LoadedUsage tables and stored procedures
                EstimatedAnnualUsage_CreateTable(meterId, meterType);
                EstimatedAnnualUsage_CreateDeleteStoredProcedure(meterId, meterType);
                EstimatedAnnualUsage_CreateInsertStoredProcedure(meterId, meterType);

                LoadedUsage_CreateTable(meterId, meterType);
                LoadedUsage_CreateDeleteStoredProcedure(meterId, meterType);
                LoadedUsage_CreateInsertStoredProcedure(meterId, meterType);
            }

            public void EstimatedAnnualUsage_Delete(string meterType, long meterId)
            {
                var estimatedAnnualUsageDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.EstimatedAnnualUsage_Delete, meterType, meterId);

                ExecuteNonQuery(new List<ParameterInfo>().ToArray(), estimatedAnnualUsageDeleteStoredProcedure);
            }

            public void EstimatedAnnualUsage_Insert(long createdByUserId, long sourceId, string meterType, long meterId, decimal estimatedAnnualUsage)
            {
                var estimatedAnnualUsageInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.EstimatedAnnualUsage_Insert, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    estimatedAnnualUsageInsertStoredProcedure,
                    createdByUserId, sourceId, estimatedAnnualUsage);
            }

            public void LoadedUsage_Delete(string meterType, long meterId, long dateId, long timePeriodId)
            {
                var loadedUsageDeleteStoredProcedure = string.Format(_storedProcedureSupplyEnums.LoadedUsage_Delete, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    loadedUsageDeleteStoredProcedure,
                    dateId, timePeriodId);
            }

            public void LoadedUsage_Insert(long createdByUserId, long sourceId, string meterType, long meterId, long dateId, long timePeriodId, long usageTypeId, decimal usage)
            {
                var loadedUsageInsertStoredProcedure = string.Format(_storedProcedureSupplyEnums.LoadedUsage_Insert, meterType, meterId);
                var parameterInfoList = MethodBase.GetCurrentMethod().GetParameters()
                    .Where(p => p.Name != "meterType" && p.Name != "meterId").ToArray();

                ExecuteNonQuery(parameterInfoList, 
                    loadedUsageInsertStoredProcedure,
                    createdByUserId, sourceId, dateId, timePeriodId, usageTypeId, usage);
            }
        }
    }
}
