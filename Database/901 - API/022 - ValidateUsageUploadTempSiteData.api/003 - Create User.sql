USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateUsageUploadTempSiteData.api')
    BEGIN
        CREATE USER [ValidateUsageUploadTempSiteData.api] FOR LOGIN [ValidateUsageUploadTempSiteData.api]
    END
GO

ALTER USER [ValidateUsageUploadTempSiteData.api] WITH DEFAULT_SCHEMA=[System]
GO
