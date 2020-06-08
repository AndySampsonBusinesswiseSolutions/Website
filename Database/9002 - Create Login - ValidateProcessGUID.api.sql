USE [master]
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

GRANT EXECUTE ON OBJECT::[System].[API_GetByGUID] TO [ValidateProcessGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [ValidateProcessGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIDAndAPIAttributeId] TO [ValidateProcessGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [ValidateProcessGUID.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [ValidateProcessGUID.api];
GO

GRANT EXECUTE ON OBJECT::[System].[Process_GetByGUID] TO [ValidateProcessGUID.api];
GO