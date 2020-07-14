USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateUsageUploadTempSubMeterUsageData.api')
    BEGIN
        CREATE USER [ValidateUsageUploadTempSubMeterUsageData.api] FOR LOGIN [ValidateUsageUploadTempSubMeterUsageData.api]
    END
GO

ALTER USER [ValidateUsageUploadTempSubMeterUsageData.api] WITH DEFAULT_SCHEMA=[System]
GO
