namespace enums
{
    public partial class Enums
    {
        public partial class InformationSchema
        {
            public class Granularity
            {
                public class Attribute
                {
                    public string GranularityCode = "Granularity Code";
                    public string GranularityDescription = "Granularity Description";
                    public string GranularityDisplayDescription = "Granularity Display Description";
                    public string IsTimePeriod = "Is Time Period";
                    public string IsElectricityDefault = "Is Electricity Default";
                    public string IsGasDefault = "Is Gas Default";
                    public string ForecastAPIGUID = "Forecast API GUID";

                    public string ForecastUsageHistoryTableSQL = "Forecast Usage History Table SQL";
                    public string ForecastUsageHistoryTempTableSQL = "Forecast Usage History Temp Table SQL";
                    public string ForecastUsageHistoryDeleteStoredProcedureSQL = "Forecast Usage History Delete Stored Procedure SQL";
                    public string ForecastUsageHistoryInsertStoredProcedureSQL = "Forecast Usage History Insert Stored Procedure SQL";
                    public string ForecastUsageHistoryGetLatestStoredProcedureSQL = "Forecast Usage History Get Latest Stored Procedure SQL";
                    
                    public string ForecastUsageLatestTableSQL = "Forecast Usage Latest Table SQL";
                    public string ForecastUsageLatestTempTableSQL = "Forecast Usage Latest Temp Table SQL";
                    public string ForecastUsageLatestDeleteStoredProcedureSQL = "Forecast Usage Latest Delete Stored Procedure SQL";
                    public string ForecastUsageLatestInsertStoredProcedureSQL = "Forecast Usage Latest Insert Stored Procedure SQL";
                    public string ForecastUsageLatestGetLatestStoredProcedureSQL = "Forecast Usage Latest Get Latest Stored Procedure SQL";

                    public string DateMappingTableSQL = "Date Mapping Table SQL";
                }
            }
        }
    }
}