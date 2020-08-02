USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateUsageUploadTempMeterData.api')
    BEGIN
        CREATE USER [ValidateUsageUploadTempMeterData.api] FOR LOGIN [ValidateUsageUploadTempMeterData.api]
    END
GO

ALTER USER [ValidateUsageUploadTempMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
