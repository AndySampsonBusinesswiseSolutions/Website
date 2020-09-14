using System.Collections.Generic;

namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Supply
            {
                public string EstimatedAnnualUsage_CreateTable = "[Supply].[EstimatedAnnualUsage_CreateTable]";
                public string EstimatedAnnualUsage_CreateDeleteStoredProcedure = "[Supply].[EstimatedAnnualUsage_CreateDeleteStoredProcedure]";
                public string EstimatedAnnualUsage_CreateInsertStoredProcedure = "[Supply].[EstimatedAnnualUsage_CreateInsertStoredProcedure]";
                public string EstimatedAnnualUsage_Delete = "[Supply.{0}{1}].[EstimatedAnnualUsage_Delete]";
                public string EstimatedAnnualUsage_Insert = "[Supply.{0}{1}].[EstimatedAnnualUsage_Insert]";
                public string EstimatedAnnualUsage_GetLatest = "[Supply.{0}{1}].[EstimatedAnnualUsage_GetLatest]";

                public IEnumerable<string> EstimatedAnnualUsageStoredProcedureList => AddEstimatedAnnualUsageStoredProcedures();

                private IEnumerable<string> AddEstimatedAnnualUsageStoredProcedures()
                {
                    return new List<string>
                    {
                        EstimatedAnnualUsage_Delete,
                        EstimatedAnnualUsage_Insert,
                        EstimatedAnnualUsage_GetLatest,
                    };
                }
            }
        }
    }
}