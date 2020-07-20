USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateUsageUploadTempFlexTradeData.api')
    BEGIN
        CREATE USER [ValidateUsageUploadTempFlexTradeData.api] FOR LOGIN [ValidateUsageUploadTempFlexTradeData.api]
    END
GO

ALTER USER [ValidateUsageUploadTempFlexTradeData.api] WITH DEFAULT_SCHEMA=[System]
GO
