USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidatePageGUID.api')
    BEGIN
        CREATE LOGIN [ValidatePageGUID.api] WITH PASSWORD=N'n:Q>V&6P9KtG`(5k', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidatePageGUID.api')
    BEGIN
        CREATE USER [ValidatePageGUID.api] FOR LOGIN [ValidatePageGUID.api]
    END
GO

ALTER USER [ValidatePageGUID.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [ValidatePageGUID.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [ValidatePageGUID.api]
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [ValidatePageGUID.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [ValidatePageGUID.api];
GO

GRANT EXECUTE ON OBJECT::[System].[Page_GetByPageGUID] TO [ValidatePageGUID.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByAPIGUID] TO [ValidatePageGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [ValidatePageGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIdAndAPIAttributeId] TO [ValidatePageGUID.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_GetByProcessQueueGUIDAndAPIId] TO [ValidatePageGUID.api];  
GO