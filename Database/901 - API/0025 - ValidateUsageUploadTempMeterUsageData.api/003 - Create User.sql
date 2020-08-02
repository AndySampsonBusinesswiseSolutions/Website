USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateUsageUploadTempMeterUsageData.api')
    BEGIN
        CREATE USER [ValidateUsageUploadTempMeterUsageData.api] FOR LOGIN [ValidateUsageUploadTempMeterUsageData.api]
    END
GO

ALTER USER [ValidateUsageUploadTempMeterUsageData.api] WITH DEFAULT_SCHEMA=[System]
GO
