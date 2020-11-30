namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class System
            {
                public string ProcessArchive_Insert = "[System].[ProcessArchive_Insert]";
                public string ProcessArchive_Update = "[System].[ProcessArchive_Update]";
                public string ProcessArchive_GetByProcessArchiveGUID = "[System].[ProcessArchive_GetByProcessArchiveGUID]";
                public string ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription = "[System].[ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription]";
                public string ProcessArchiveDetail_GetByProcessArchiveIdAndProcessArchiveAttributeId = "[System].[ProcessArchiveDetail_GetByProcessArchiveIdAndProcessArchiveAttributeId]";
                public string ProcessArchiveDetail_GetByEffectiveFromDateAndEffectiveToDateTimeAndProcessArchiveDetailDescription = "[System].[ProcessArchiveDetail_GetByEffectiveFromDateAndEffectiveToDateTimeAndProcessArchiveDetailDescription]";
                public string ProcessArchiveDetail_Insert = "[System].[ProcessArchiveDetail_Insert]";
                public string ProcessArchiveDetail_InsertAll = "[System].[ProcessArchiveDetail_InsertAll]";
                public string ProcessArchiveDetail_GetByProcessArchiveDetailId = "[System].[ProcessArchiveDetail_GetByProcessArchiveDetailId]";
            }
        }
    }
}