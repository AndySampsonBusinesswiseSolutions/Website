using System.Collections.Generic;

namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Supply
            {
                public string ForecastUsageGranularityHistory_Delete = "[Supply.{0}{1}].[ForecastUsage{2}History_Delete]";
                public string ForecastUsageGranularityHistory_Insert = "[Supply.{0}{1}].[ForecastUsage{2}History_Insert]";
                public string ForecastUsageGranularityHistory_GetLatest = "[Supply.{0}{1}].[ForecastUsage{2}History_GetLatest]";

                public List<string> ForecastUsageGranularityHistoryStoredProcedureList => AddForecastUsageGranularityHistoryStoredProcedures();

                private List<string> AddForecastUsageGranularityHistoryStoredProcedures()
                {
                    return new List<string>
                    {
                        // ForecastUsageGranularityHistory_Delete,
                        // ForecastUsageGranularityHistory_Insert,
                        ForecastUsageGranularityHistory_GetLatest,
                    };
                }
            }
        }
    }
}