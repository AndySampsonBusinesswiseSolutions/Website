using System.Collections.Generic;

namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Supply
            {
                public string ForecastUsageGranularityLatest_CreateTable = "[Supply].[ForecastUsageGranularityLatest_CreateTable]";
                public string ForecastUsageGranularityLatest_CreateDeleteStoredProcedure = "[Supply].[ForecastUsageGranularityLatest_CreateDeleteStoredProcedure]";
                public string ForecastUsageGranularityLatest_CreateInsertStoredProcedure = "[Supply].[ForecastUsageGranularityLatest_CreateInsertStoredProcedure]";
                
                public List<string> ForecastUsageGranularityLatestStoredProcedureList = new List<string>
                {
                    "[Supply.{0}{1}].[ForecastUsage{2}Latest_Delete]",
                    "[Supply.{0}{1}].[ForecastUsage{2}Latest_Insert]"
                };
            }
        }
    }
}