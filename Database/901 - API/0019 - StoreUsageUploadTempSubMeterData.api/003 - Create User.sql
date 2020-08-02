USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreUsageUploadTempSubMeterData.api')
    BEGIN
        CREATE USER [StoreUsageUploadTempSubMeterData.api] FOR LOGIN [StoreUsageUploadTempSubMeterData.api]
    END
GO

ALTER USER [StoreUsageUploadTempSubMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
