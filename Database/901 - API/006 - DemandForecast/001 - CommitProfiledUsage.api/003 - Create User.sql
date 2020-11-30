USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitProfiledUsage.api')
    BEGIN
        CREATE USER [CommitProfiledUsage.api] FOR LOGIN [CommitProfiledUsage.api]
    END
GO

ALTER USER [CommitProfiledUsage.api] WITH DEFAULT_SCHEMA=[System]
GO
