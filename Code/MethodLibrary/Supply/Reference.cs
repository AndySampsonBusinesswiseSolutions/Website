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

                //Create ForecastUsageHistory tables and stored procedures by granularity
                CreateForecastUsageGranularityHistoryEntities(granularityCodeList, schemaId, meterId, meterType);

                //Create ForecastUsageLatest tables and stored procedures by granularity
                CreateForecastUsageGranularityLatestEntities(granularityCodeList, schemaId, meterId, meterType);

                //Create EstimatedUsage tables and stored procedures
                CreateEstimatedAnnualUsageEntities(schemaId, meterId, meterType);

                //Create LoadedUsage tables and stored procedures
                CreateLoadedUsageEntities(schemaId, meterId, meterType);
            }
        }
    }
}