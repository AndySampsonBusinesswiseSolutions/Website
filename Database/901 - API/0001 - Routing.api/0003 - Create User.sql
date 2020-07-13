USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'Routing.api')
    BEGIN
        CREATE USER [Routing.api] FOR LOGIN [Routing.api]
    END
GO

ALTER USER [Routing.api] WITH DEFAULT_SCHEMA=[System]
GO