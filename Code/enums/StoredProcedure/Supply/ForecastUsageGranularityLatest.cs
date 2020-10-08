using System.Collections.Generic;

namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Supply
            {
                public string ForecastUsageGranularityLatest_Delete = "[Supply.{0}{1}].[ForecastUsage{2}Latest_Delete]";
                public string ForecastUsageGranularityLatest_Insert = "[Supply.{0}{1}].[ForecastUsage{2}Latest_Insert]";
                public string ForecastUsageGranularityLatest_GetLatest = "[Supply.{0}{1}].[ForecastUsage{2}Latest_GetLatest]";

                public List<string> ForecastUsageGranularityLatestStoredProcedureList => AddForecastUsageGranularityLatestStoredProcedures();

                private List<string> AddForecastUsageGranularityLatestStoredProcedures()
                {
                    return new List<string>
                    {
                        // ForecastUsageGranularityLatest_Delete,
                        // ForecastUsageGranularityLatest_Insert,
                        ForecastUsageGranularityLatest_GetLatest,
                    };
                }
            }
        }
    }
}