USE [master]
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

USE [EMaaS]
GO

ALTER USER [Routing.api] WITH DEFAULT_SCHEMA=[System]
GO

USE [EMaaS]
GO

ALTER ROLE [db_datareader] ADD MEMBER [Routing.api]
GO

GRANT EXECUTE ON [System].[GetAPIListFromPageAndProcess] TO [Routing.api]