USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateCustomerFolders.api')
    BEGIN
        DROP LOGIN [CreateCustomerFolders.api]
    END
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateCustomerFolders.api')
    BEGIN
        CREATE LOGIN [CreateCustomerFolders.api] WITH PASSWORD=N'UE9ggtwMq6G4fpYv', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateCustomerFolders.api')
    BEGIN
        CREATE USER [CreateCustomerFolders.api] FOR LOGIN [CreateCustomerFolders.api]
    END
GO

ALTER USER [CreateCustomerFolders.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [CreateCustomerFolders.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [CreateCustomerFolders.api]
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [CreateCustomerFolders.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [CreateCustomerFolders.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByAPIGUID] TO [CreateCustomerFolders.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [CreateCustomerFolders.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIdAndAPIAttributeId] TO [CreateCustomerFolders.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[User_GetByUserGUID] TO [CreateCustomerFolders.api];  
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceAttribute_GetBySourceAttributeDescription] TO [CreateCustomerFolders.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription] TO [CreateCustomerFolders.api];
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [CreateCustomerFolders.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [CreateCustomerFolders.api];  
GO

GRANT EXECUTE ON OBJECT::[Information].[RootFolderType_GetByRootFolderTypeDescription] TO [CreateCustomerFolders.api];
GO

GRANT EXECUTE ON OBJECT::[Mapping].[FolderToRootFolderType_GetByRootFolderTypeId] TO [CreateCustomerFolders.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[FolderAttribute_GetByFolderAttributeDescription] TO [CreateCustomerFolders.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[FolderDetail_GetByFolderIdAndFolderAttributeId] TO [CreateCustomerFolders.api];
GO

GRANT EXECUTE ON OBJECT::[Mapping].[FolderToFolderExtension_GetByFolderId] TO [CreateCustomerFolders.api];
GO