namespace databaseInteraction
{
    public partial class CommonEnums
    {
        public class StoredProcedure
        {
            public class Administration
            {
                public string Password_GetByPassword = "[Administration.User].[Password_GetByPassword]";
                public string UserDetail_GetByUserDetailDescription = "[Administration.User].[UserDetail_GetByUserDetailDescription]";
                public string UserDetail_GetByUserDetailId = "[Administration.User].[UserDetail_GetByUserDetailId]";
            }

            public class Mapping
            {
                public string APIToProcess_GetAPIIdListByProcessId = "[Mapping].[API_GetAPIIdListByProcessId]"; //TODO: Rename sproc
                public string PasswordToUser_GetByPasswordIdAndUserId = "[Mapping].[PasswordToUser_GetByPasswordIdAndUserId]";
            }

            public class System
            {
                public string API_GetByGUID = "[System].[API_GetByGUID]";
                public string API_GetById = "[System].[API_GetById]";
                public string APIAttribute_GetByAPIAttributeDescription = "[System].[APIAttribute_GetByAPIAttributeDescription]";
                public string APIDetail_GetByAPIIdAndAPIAttributeId = "[System].[APIDetail_GetByAPIIdAndAPIAttributeId]";
                public string Page_GetByGUID = "[System].[Page_GetByGUID]";
                public string Process_GetByGUID = "[System].[Process_GetByGUID]";
                public string ProcessQueue_Insert = "[System].[ProcessQueue_Insert]";
                public string ProcessQueue_Update = "[System].[ProcessQueue_Update]";
                public string ProcessQueue_GetByGUIDAndAPIId = "[System].[ProcessQueue_GetByGUIDAndAPIId]";
                public string ProcessArchive_Insert = "[System].[ProcessArchive_Insert]";
                public string ProcessArchive_Update = "[System].[ProcessArchive_Update]";
                public string ProcessArchive_GetByGUID = "[System].[ProcessArchive_GetByGUID]";
                public string ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription = "[System].[ProcessArchiveAttribute_GetByProcessArchiveAttributeDescription]";
                public string ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId = "[System].[ProcessArchiveDetail_GetByProcessArchiveIDAndProcessArchiveAttributeId]";
                public string ProcessArchiveDetail_Insert = "[System].[ProcessArchiveDetail_Insert]";
            }
        }
    }
}