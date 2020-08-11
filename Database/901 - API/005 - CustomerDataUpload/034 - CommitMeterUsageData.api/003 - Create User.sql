USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterUsageData.api')
    BEGIN
        CREATE USER [CommitMeterUsageData.api] FOR LOGIN [CommitMeterUsageData.api]
    END
GO

ALTER USER [CommitMeterUsageData.api] WITH DEFAULT_SCHEMA=[System]
GO
