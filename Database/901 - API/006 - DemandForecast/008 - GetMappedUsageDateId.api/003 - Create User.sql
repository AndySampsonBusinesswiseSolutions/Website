USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'GetMappedUsageDateId.api')
    BEGIN
        CREATE USER [GetMappedUsageDateId.api] FOR LOGIN [GetMappedUsageDateId.api]
    END
GO

ALTER USER [GetMappedUsageDateId.api] WITH DEFAULT_SCHEMA=[System]
GO
