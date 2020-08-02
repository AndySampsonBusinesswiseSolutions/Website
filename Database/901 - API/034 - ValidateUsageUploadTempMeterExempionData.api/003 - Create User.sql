USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateUsageUploadTempMeterExempionData.api')
    BEGIN
        CREATE USER [ValidateUsageUploadTempMeterExempionData.api] FOR LOGIN [ValidateUsageUploadTempMeterExempionData.api]
    END
GO

ALTER USER [ValidateUsageUploadTempMeterExempionData.api] WITH DEFAULT_SCHEMA=[System]
GO
