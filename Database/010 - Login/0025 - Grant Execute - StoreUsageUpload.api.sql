USE [EMaaS]
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[UserDetail_GetByUserIdAndUserAttributeId] TO [StoreUsageUpload.api];  
GO

GRANT EXECUTE ON OBJECT::[Information].[RootFolderType_GetByRootFolderTypeDescription] TO [StoreUsageUpload.api];  
GO

GRANT EXECUTE ON OBJECT::[Mapping].[FolderToRootFolderType_GetByRootFolderTypeId] TO [StoreUsageUpload.api];  
GO

GRANT EXECUTE ON OBJECT::[Information].[FolderAttribute_GetByFolderAttributeDescription] TO [StoreUsageUpload.api];  
GO

GRANT EXECUTE ON OBJECT::[Information].[FolderExtensionType_GetByFolderExtensionTypeDescription] TO [StoreUsageUpload.api];  
GO

GRANT EXECUTE ON OBJECT::[Mapping].[FolderToFolderExtensionType_GetByFolderExtensionTypeId] TO [StoreUsageUpload.api];  
GO

GRANT EXECUTE ON OBJECT::[Information].[FolderDetail_GetByFolderIdAndFolderAttributeId] TO [StoreUsageUpload.api];  
GO

GRANT EXECUTE ON OBJECT::[Mapping].[FolderToFolderExtension_GetByFolderId] TO [StoreUsageUpload.api];  
GO