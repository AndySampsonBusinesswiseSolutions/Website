USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitEstimatedAnnualUsage.api')
    BEGIN
        CREATE USER [CommitEstimatedAnnualUsage.api] FOR LOGIN [CommitEstimatedAnnualUsage.api]
    END
GO

ALTER USER [CommitEstimatedAnnualUsage.api] WITH DEFAULT_SCHEMA=[System]
GO
