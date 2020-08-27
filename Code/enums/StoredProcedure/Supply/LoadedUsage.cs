using System.Collections.Generic;

namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Supply
            {
                public string LoadedUsage_CreateTable = "[Supply].[LoadedUsage_CreateTable]";
                public string LoadedUsage_CreateTempTable = "[Supply].[LoadedUsage_CreateTempTable]";
                public string LoadedUsage_CreateDeleteStoredProcedure = "[Supply].[LoadedUsage_CreateDeleteStoredProcedure]";
                public string LoadedUsage_CreateInsertStoredProcedure = "[Supply].[LoadedUsage_CreateInsertStoredProcedure]";
                public string LoadedUsage_Delete = "[Supply.{0}{1}].[LoadedUsage_Delete]";
                public string LoadedUsage_Insert = "[Supply.{0}{1}].[LoadedUsage_Insert]";

                public List<string> LoadedUsageStoredProcedureList = new List<string>
                {
                    "[Supply.{0}{1}].[LoadedUsage_Delete]",
                    "[Supply.{0}{1}].[LoadedUsage_Insert]"
                };
            }
        }
    }
}