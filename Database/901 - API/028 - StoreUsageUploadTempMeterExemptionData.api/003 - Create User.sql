USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreUsageUploadTempMeterExemptionData.api')
    BEGIN
        CREATE USER [StoreUsageUploadTempMeterExemptionData.api] FOR LOGIN [StoreUsageUploadTempMeterExemptionData.api]
    END
GO

ALTER USER [StoreUsageUploadTempMeterExemptionData.api] WITH DEFAULT_SCHEMA=[System]
GO
