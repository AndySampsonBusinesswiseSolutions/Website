USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitPeriodicUsageData.api')
    BEGIN
        CREATE USER [CommitPeriodicUsageData.api] FOR LOGIN [CommitPeriodicUsageData.api]
    END
GO

ALTER USER [CommitPeriodicUsageData.api] WITH DEFAULT_SCHEMA=[System]
GO
