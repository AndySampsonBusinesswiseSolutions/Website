using System.Collections.Generic;

namespace enums
{
    public partial class Enums
    {
        public partial class System
        {
            public partial class API
            {
                public class RequireAccessToUsageEntities
                {
                    //TODO: Move to database
                    public List<string> APIList = new List<string>
                    {
                        "CommitEstimatedAnnualUsage.api",
                        "CommitPeriodicUsageData.api",
                        "GetProfile.api",
                        "CommitProfiledUsage.api",
                        "CreateForecastUsage.api",
                        "GetMappedUsageDateId.api",
                        "CreateFiveMinuteForecast.api",
                        "CreateHalfHourForecast.api",
                        "CreateDateForecast.api",
                        "CreateWeekForecast.api",
                        "CreateMonthForecast.api",
                        "CreateQuarterForecast.api",
                        "CreateYearForecast.api",
                        "Website.api",
                        "CreateDataAnalysisWebpage.api",
                        "DataAnalysisWebpageGetForecast.api"
                    };
                }
            }
        }
    }
}
