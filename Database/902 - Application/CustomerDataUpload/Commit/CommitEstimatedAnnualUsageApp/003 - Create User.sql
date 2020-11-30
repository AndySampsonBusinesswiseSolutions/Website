USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitEstimatedAnnualUsageApp')
    BEGIN
        CREATE USER [CommitEstimatedAnnualUsageApp] FOR LOGIN [CommitEstimatedAnnualUsageApp]
    END
GO

ALTER USER [CommitEstimatedAnnualUsageApp] WITH DEFAULT_SCHEMA=[System]
GO
