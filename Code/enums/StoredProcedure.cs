namespace enums
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
                public string UserDetail_Insert = "[Administration.User].[UserDetail_Insert]";
                public string Login_Insert = "[Administration.User].[Login_Insert]";
                public string Login_GetByProcessArchiveGUID = "[Administration.User].[Login_GetByProcessArchiveGUID]";
                public string Login_GetByLoginId = "[Administration.User].[Login_GetByLoginId]";
            }

            public class Information
            {
                public string SourceType_GetBySourceTypeDescription = "[Information].[SourceType_GetBySourceTypeDescription]";
                public string Source_GetBySourceTypeIdAndSourceTypeEntityId = "[Information].[Source_GetBySourceTypeIdAndSourceTypeEntityId]";
            }

            public class Mapping
            {
                public string APIToProcess_GetAPIIdListByProcessId = "[Mapping].[API_GetAPIIdListByProcessId]"; //TODO: Rename sproc
                public string PasswordToUser_GetByPasswordIdAndUserId = "[Mapping].[PasswordToUser_GetByPasswordIdAndUserId]";
                public string LoginToUser_Insert = "[Mapping].[LoginToUser_Insert]";
                public string LoginToUser_GetByUserId = "[Mapping].[LoginToUser_GetByUserId]";
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