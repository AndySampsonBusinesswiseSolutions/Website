namespace enums
{
    public partial class Enums
    {
        public partial class StoredProcedure
        {
            public partial class System
            {
                public string ProcessQueue_Delete = "[System].[ProcessQueue_Delete]";
                public string ProcessQueue_Insert = "[System].[ProcessQueue_Insert]";
                public string ProcessQueue_Update = "[System].[ProcessQueue_Update]";
                public string ProcessQueue_GetByProcessQueueGUID = "[System].[ProcessQueue_GetByProcessQueueGUID]";
                public string ProcessQueue_GetByProcessQueueGUIDAndAPIId = "[System].[ProcessQueue_GetByProcessQueueGUIDAndAPIId]";
                public string ProcessQueue_GetHasErrorByProcessQueueGUID = "[System].[ProcessQueue_GetHasErrorByProcessQueueGUID]";
                public string ProcessQueue_GetHasSystemErrorByProcessQueueGUID = "[System].[ProcessQueue_GetHasSystemErrorByProcessQueueGUID]";
            }
        }
    }
}