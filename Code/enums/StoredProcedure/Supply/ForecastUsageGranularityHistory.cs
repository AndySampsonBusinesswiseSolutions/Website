using System.Collections.Generic;

namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Supply
            {
                public string ForecastUsageGranularityHistory_CreateTable = "[Supply].[ForecastUsageGranularityHistory_CreateTable]";
                public string ForecastUsageGranularityHistory_CreateDeleteStoredProcedure = "[Supply].[ForecastUsageGranularityHistory_CreateDeleteStoredProcedure]";
                public string ForecastUsageGranularityHistory_CreateInsertStoredProcedure = "[Supply].[ForecastUsageGranularityHistory_CreateInsertStoredProcedure]";

                public List<string> ForecastUsageGranularityHistoryStoredProcedureList = new List<string>
                {
                    "[Supply.{0}{1}].[ForecastUsage{2}History_Delete]",
                    "[Supply.{0}{1}].[ForecastUsage{2}History_Insert]"
                };
            }
        }
    }
}