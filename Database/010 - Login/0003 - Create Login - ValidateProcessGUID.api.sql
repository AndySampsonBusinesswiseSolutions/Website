USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateProcessGUID.api')
    BEGIN
        DROP LOGIN [ValidateProcessGUID.api]
    END
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateProcessGUID.api')
    BEGIN
        CREATE LOGIN [ValidateProcessGUID.api] WITH PASSWORD=N'Y4c?.KT(>HXj@f8D', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateProcessGUID.api')
    BEGIN
        CREATE USER [ValidateProcessGUID.api] FOR LOGIN [ValidateProcessGUID.api]
    END
GO

ALTER USER [ValidateProcessGUID.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [ValidateProcessGUID.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [ValidateProcessGUID.api]
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByAPIGUID] TO [ValidateProcessGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [ValidateProcessGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIdAndAPIAttributeId] TO [ValidateProcessGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [ValidateProcessGUID.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [ValidateProcessGUID.api];
GO

GRANT EXECUTE ON OBJECT::[System].[Process_GetByProcessGUID] TO [ValidateProcessGUID.api];
GO

GRANT EXECUTE ON OBJECT::[Administration.User].[User_GetByUserGUID] TO [ValidateProcessGUID.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceAttribute_GetBySourceAttributeDescription] TO [ValidateProcessGUID.api];
GO

GRANT EXECUTE ON OBJECT::[Information].[SourceDetail_GetBySourceAttributeIdAndSourceDetailDescription] TO [ValidateProcessGUID.api];
GO