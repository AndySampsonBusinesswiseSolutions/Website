USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreUsageUploadTempMeterUsageData.api')
    BEGIN
        CREATE USER [StoreUsageUploadTempMeterUsageData.api] FOR LOGIN [StoreUsageUploadTempMeterUsageData.api]
    END
GO

ALTER USER [StoreUsageUploadTempMeterUsageData.api] WITH DEFAULT_SCHEMA=[System]
GO
