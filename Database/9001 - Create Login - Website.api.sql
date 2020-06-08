USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'Website.api')
    BEGIN
        CREATE LOGIN [Website.api] WITH PASSWORD=N'\wU.D[ArWjPG!F4$', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'Website.api')
    BEGIN
        CREATE USER [Website.api] FOR LOGIN [Website.api]
    END
GO

ALTER USER [Website.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [Website.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [Website.api]
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByGUID] TO [Website.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIDAndAPIAttributeId] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [Website.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [Website.api];
GO