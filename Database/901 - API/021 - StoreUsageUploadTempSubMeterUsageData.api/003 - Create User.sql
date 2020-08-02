USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreUsageUploadTempSubMeterUsageData.api')
    BEGIN
        CREATE USER [StoreUsageUploadTempSubMeterUsageData.api] FOR LOGIN [StoreUsageUploadTempSubMeterUsageData.api]
    END
GO

ALTER USER [StoreUsageUploadTempSubMeterUsageData.api] WITH DEFAULT_SCHEMA=[System]
GO
