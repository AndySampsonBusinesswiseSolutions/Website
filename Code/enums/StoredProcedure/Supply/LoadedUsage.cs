using System.Collections.Generic;

namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Supply
            {
                public string LoadedUsageHistory_CreateTable = "[Supply].[LoadedUsageHistory_CreateTable]";
                public string LoadedUsageLatest_CreateTable = "[Supply].[LoadedUsageLatest_CreateTable]";
                public string LoadedUsageLatest_CreateGetListStoredProcedure = "[Supply].[LoadedUsageLatest_CreateGetListStoredProcedure]";
                public string LoadedUsageLatest_GetList = "[Supply.{0}{1}].[LoadedUsageLatest_GetList]";

                public List<string> LoadedUsageStoredProcedureList => AddLoadedUsageStoredProcedures();

                private List<string> AddLoadedUsageStoredProcedures()
                {
                    return new List<string>
                    {
                        LoadedUsageLatest_GetList,
                    };
                }
            }
        }
    }
}