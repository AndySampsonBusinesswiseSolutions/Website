USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateSubMeterUsageData.api')
    BEGIN
        CREATE USER [ValidateSubMeterUsageData.api] FOR LOGIN [ValidateSubMeterUsageData.api]
    END
GO

ALTER USER [ValidateSubMeterUsageData.api] WITH DEFAULT_SCHEMA=[System]
GO
