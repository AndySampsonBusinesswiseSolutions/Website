using System.Collections.Generic;

namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Supply
            {
                public List<string> ForecastUsageGranularityHistoryStoredProcedureList = new List<string>
                {
                    "[Supply.{0}{1}].[ForecastUsage{2}History_Delete]",
                    "[Supply.{0}{1}].[ForecastUsage{2}History_Insert]"
                };
            }
        }
    }
}