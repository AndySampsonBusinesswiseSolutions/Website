using System.Collections.Generic;

namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class Supply
            {
                public string DateMapping_CreateTable = "[Supply].[DateMapping_CreateTable]";
                public string DateMapping_CreateDeleteStoredProcedure = "[Supply].[DateMapping_CreateDeleteStoredProcedure]";
                public string DateMapping_CreateInsertStoredProcedure = "[Supply].[DateMapping_CreateInsertStoredProcedure]";
                public string DateMapping_CreateGetLatestStoredProcedure = "[Supply].[DateMapping_CreateGetLatestStoredProcedure]";
                public string DateMapping_Delete = "[Supply.{0}{1}].[DateMapping_Delete]";
                public string DateMapping_Insert = "[Supply.{0}{1}].[DateMapping_Insert]";
                public string DateMapping_GetLatest = "[Supply.{0}{1}].[DateMapping_GetLatest]";

                public IEnumerable<string> DateMappingStoredProcedureList => AddDateMappingStoredProcedures();

                private IEnumerable<string> AddDateMappingStoredProcedures()
                {
                    return new List<string>
                    {
                        DateMapping_Delete,
                        DateMapping_Insert,
                        DateMapping_GetLatest,
                    };
                }
            }
        }
    }
}