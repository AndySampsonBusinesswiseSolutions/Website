USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'Website.api')
    BEGIN
        CREATE USER [Website.api] FOR LOGIN [Website.api]
    END
GO

ALTER USER [Website.api] WITH DEFAULT_SCHEMA=[System]
GO