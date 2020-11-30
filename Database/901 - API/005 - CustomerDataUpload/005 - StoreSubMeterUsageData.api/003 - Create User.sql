USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreSubMeterUsageData.api')
    BEGIN
        CREATE USER [StoreSubMeterUsageData.api] FOR LOGIN [StoreSubMeterUsageData.api]
    END
GO

ALTER USER [StoreSubMeterUsageData.api] WITH DEFAULT_SCHEMA=[System]
GO
