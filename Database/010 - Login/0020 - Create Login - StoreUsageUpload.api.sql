USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUpload.api')
    BEGIN
        DROP LOGIN [StoreUsageUpload.api]
    END
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUpload.api')
    BEGIN
        CREATE LOGIN [StoreUsageUpload.api] WITH PASSWORD=N'Mt35GJs9un!Jq7pg', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreUsageUpload.api')
    BEGIN
        CREATE USER [StoreUsageUpload.api] FOR LOGIN [StoreUsageUpload.api]
    END
GO

ALTER USER [StoreUsageUpload.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [StoreUsageUpload.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [StoreUsageUpload.api]

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [StoreUsageUpload.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [StoreUsageUpload.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByAPIGUID] TO [StoreUsageUpload.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [StoreUsageUpload.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIdAndAPIAttributeId] TO [StoreUsageUpload.api];  
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[User_GetByUserGUID] TO [StoreUsageUpload.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceAttribute_GetBySourceAttributeDescription] TO [StoreUsageUpload.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription] TO [StoreUsageUpload.api];
GO

GRANT EXECUTE ON OBJECT::[System].[Error_Insert] TO [StoreUsageUpload.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[Error_GetByErrorGUID] TO [StoreUsageUpload.api];  
GO