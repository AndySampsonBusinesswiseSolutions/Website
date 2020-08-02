USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreUsageUploadTempMeterData.api')
    BEGIN
        CREATE USER [StoreUsageUploadTempMeterData.api] FOR LOGIN [StoreUsageUploadTempMeterData.api]
    END
GO

ALTER USER [StoreUsageUploadTempMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
