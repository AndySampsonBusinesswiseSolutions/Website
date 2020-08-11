USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitSubMeterUsageData.api')
    BEGIN
        CREATE USER [CommitSubMeterUsageData.api] FOR LOGIN [CommitSubMeterUsageData.api]
    END
GO

ALTER USER [CommitSubMeterUsageData.api] WITH DEFAULT_SCHEMA=[System]
GO
