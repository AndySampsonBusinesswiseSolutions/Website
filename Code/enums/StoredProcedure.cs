namespace enums
{
    public partial class Enums
    {
        public class StoredProcedure
        {
            public class Administration
            {
                public string Password_GetByPassword = "[Administration.User].[Password_GetByPassword]";
                public string UserDetail_GetByUserDetailDescription = "[Administration.User].[UserDetail_GetByUserDetailDescription]";
                public string UserDetail_GetByUserDetailId = "[Administration.User].[UserDetail_GetByUserDetailId]";
                public string UserDetail_GetByUserIdAndUserAttributeId = "[Administration.User].[UserDetail_GetByUserIdAndUserAttributeId]";
                public string UserDetail_Insert = "[Administration.User].[UserDetail_Insert]";
                public string User_GetByUserGUID = "[Administration.User].[User_GetByUserGUID]";
                public string UserAttribute_GetByUserAttributeDescription = "[Administration.User].[UserAttribute_GetByUserAttributeDescription]";
                public string Login_Insert = "[Administration.User].[Login_Insert]";
                public string Login_GetByProcessArchiveGUID = "[Administration.User].[Login_GetByProcessArchiveGUID]";
                public string Login_GetByLoginId = "[Administration.User].[Login_GetByLoginId]";
            }

            public class Customer
            {
                public string CustomerAttribute_GetByCustomerAttributeDescription = "[Customer].[CustomerAttribute_GetByCustomerAttributeDescription]";
                public string CustomerDetail_GetByCustomerAttributeIdAndCustomerDetailDescription = "[Customer].[CustomerDetail_GetByCustomerAttributeIdAndCustomerDetailDescription]";
                public string CustomerDetail_DeleteByCustomerDetailId = "[Customer].[CustomerDetail_DeleteByCustomerDetailId]";
                public string CustomerDetail_GetByCustomerIdAndCustomerAttributeId = "[Customer].[CustomerDetail_GetByCustomerIdAndCustomerAttributeId]";
                public string Customer_Insert = "[Customer].[Customer_Insert]";
                public string CustomerDetail_Insert = "[Customer].[CustomerDetail_Insert]";
                public string Customer_GetByCustomerGUID = "[Customer].[Customer_GetByCustomerGUID]";
                public string Customer_GetList = "[Customer].[Customer_GetList]";
            }

            public class Information
            {
                public string SourceAttribute_GetBySourceAttributeDescription = "[Information].[SourceAttribute_GetBySourceAttributeDescription]";
                public string SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription = "[Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription]";
                public string File_Insert = "[Information].[File_Insert]";
                public string File_GetByFileGUID = "[Information].[File_GetByFileGUID]";
                public string FileAttribute_GetByFileAttributeDescription = "[Information].[FileAttribute_GetByFileAttributeDescription]";
                public string FileDetail_Insert = "[Information].[FileDetail_Insert]";
                public string FileContent_Insert = "[Information].[FileContent_Insert]";
                public string FileType_GetByFileTypeDescription = "[Information].[FileType_GetByFileTypeDescription]";
            }

            public class Mapping
            {
                public string APIToProcess_GetByProcessId = "[Mapping].[APIToProcess_GetByProcessId]";
                public string APIToProcessArchiveDetail_Insert = "[Mapping].[APIToProcessArchiveDetail_Insert]";
                public string PasswordToUser_GetByPasswordIdAndUserId = "[Mapping].[PasswordToUser_GetByPasswordIdAndUserId]";
                public string LoginToUser_Insert = "[Mapping].[LoginToUser_Insert]";
                public string LoginToUser_GetByUserId = "[Mapping].[LoginToUser_GetByUserId]";
                public string ProcessToProcessArchive_Insert = "[Mapping].[ProcessToProcessArchive_Insert]";
                public string CustomerToChildCustomer_Insert = "[Mapping].[CustomerToChildCustomer_Insert]";
                public string CustomerToChildCustomer_GetByChildCustomerId = "[Mapping].[CustomerToChildCustomer_GetByChildCustomerId]";
                public string CustomerToChildCustomer_GetByCustomerId = "[Mapping].[CustomerToChildCustomer_GetByCustomerId]";
                public string CustomertoChildCustomer_DeleteByCustomerIdAndChildCustomerId = "[Mapping].[CustomertoChildCustomer_DeleteByCustomerIdAndChildCustomerId]";
                public string CustomerToChildCustomer_GetList = "[Mapping].[CustomerToChildCustomer_GetList]";
                public string APIToProcessArchiveDetail_GetByAPIId = "[Mapping].[APIToProcessArchiveDetail_GetByAPIId]";
                public string FolderToRootFolderType_GetByRootFolderTypeId = "[Mapping].[FolderToRootFolderType_GetByRootFolderTypeId]";
                public string FolderToFolderExtension_GetByFolderId = "[Mapping].[FolderToFolderExtension_GetByFolderId]";
                public string FolderToFolderExtensionType_GetByFolderExtensionTypeId = "[Mapping].[FolderToFolderExtensionType_GetByFolderExtensionTypeId]";
                public string CustomerToFile_Insert = "[Mapping].[CustomerToFile_Insert]";
                public string FileToFileType_Insert = "[Mapping].[FileToFileType_Insert]";
            }

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