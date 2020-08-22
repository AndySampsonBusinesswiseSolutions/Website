namespace MethodLibrary
{
    public partial class Methods
    {
        public partial class Supply
        {
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
        }
    }
}