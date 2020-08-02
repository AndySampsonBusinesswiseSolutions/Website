USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateUsageUploadTempSubMeterData.api')
    BEGIN
        CREATE USER [ValidateUsageUploadTempSubMeterData.api] FOR LOGIN [ValidateUsageUploadTempSubMeterData.api]
    END
GO

ALTER USER [ValidateUsageUploadTempSubMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
