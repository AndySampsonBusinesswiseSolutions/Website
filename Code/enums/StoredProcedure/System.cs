namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public class System
            {
                public string API_GetByAPIGUID = "[System].[API_GetByAPIGUID]";
                public string API_GetByAPIId = "[System].[API_GetByAPIId]";
                public string APIAttribute_GetByAPIAttributeDescription = "[System].[APIAttribute_GetByAPIAttributeDescription]";
                public string APIDetail_GetByAPIIdAndAPIAttributeId = "[System].[APIDetail_GetByAPIIdAndAPIAttributeId]";
                public string Error_Insert = "[System].[Error_Insert]";
                public string Error_GetByErrorGUID = "[System].[Error_GetByErrorGUID]";
                public string Page_GetByPageGUID = "[System].[Page_GetByPageGUID]";
                public string Process_GetByProcessGUID = "[System].[Process_GetByProcessGUID]";
                public string ProcessQueue_Delete = "[System].[ProcessQueue_Delete]";
                public string ProcessQueue_Insert = "[System].[ProcessQueue_Insert]";
                public string ProcessQueue_Update = "[System].[ProcessQueue_Update]";
                public string ProcessQueue_GetByProcessQueueGUID = "[System].[ProcessQueue_GetByProcessQueueGUID]";
                public string ProcessQueue_GetByProcessQueueGUIDAndAPIId = "[System].[ProcessQueue_GetByProcessQueueGUIDAndAPIId]";
                public string ProcessQueue_GetHasErrorByProcessQueueGUID = "[System].[ProcessQueue_GetHasErrorByProcessQueueGUID]";
                public string ProcessQueue_GetHasSystemErrorByProcessQueueGUID = "[System].[ProcessQueue_GetHasSystemErrorByProcessQueueGUID]";
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