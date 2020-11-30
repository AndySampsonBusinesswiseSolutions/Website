USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'LockUserApp')
    BEGIN
        CREATE USER [LockUserApp] FOR LOGIN [LockUserApp]
    END
GO

ALTER USER [LockUserApp] WITH DEFAULT_SCHEMA=[System]
GO