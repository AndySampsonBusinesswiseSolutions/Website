using System.Collections.Generic;

namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Supply
            {
                public List<string> ForecastUsageGranularityLatestStoredProcedureList = new List<string>
                {
                    "[Supply.{0}{1}].[ForecastUsage{2}Latest_Delete]",
                    "[Supply.{0}{1}].[ForecastUsage{2}Latest_Insert]"
                };
            }
        }
    }
}