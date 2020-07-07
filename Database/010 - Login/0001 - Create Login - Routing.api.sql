USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'Routing.api')
    BEGIN
        DROP LOGIN [Routing.api]
    END
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'Routing.api')
    BEGIN
        CREATE LOGIN [Routing.api] WITH PASSWORD=N'E{*Jj5&nLfC}@Q$:', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO

USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'Routing.api')
    BEGIN
        CREATE USER [Routing.api] FOR LOGIN [Routing.api]
    END
GO

ALTER USER [Routing.api] WITH DEFAULT_SCHEMA=[System]
GO

ALTER ROLE [db_denydatareader] ADD MEMBER [Routing.api]
GO

ALTER ROLE [db_denydatawriter] ADD MEMBER [Routing.api]
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Insert] TO [Routing.api];
GO

GRANT EXECUTE ON OBJECT::[System].[ProcessQueue_Update] TO [Routing.api];
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByAPIGUID] TO [Routing.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIAttribute_GetByAPIAttributeDescription] TO [Routing.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[APIDetail_GetByAPIIdAndAPIAttributeId] TO [Routing.api];  
GO

GRANT EXECUTE ON OBJECT::[Mapping].[APIToProcess_GetByProcessId] TO [Routing.api];  
GO

GRANT EXECUTE ON OBJECT::[System].[API_GetByAPIId] TO [Routing.api];  
GO