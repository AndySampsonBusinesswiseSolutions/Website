USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitProfiledUsageApp')
    BEGIN
        CREATE USER [CommitProfiledUsageApp] FOR LOGIN [CommitProfiledUsageApp]
    END
GO

ALTER USER [CommitProfiledUsageApp] WITH DEFAULT_SCHEMA=[System]
GO
