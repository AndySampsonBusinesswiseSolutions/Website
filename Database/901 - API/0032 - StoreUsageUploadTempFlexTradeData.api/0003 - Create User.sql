USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreUsageUploadTempFlexTradeData.api')
    BEGIN
        CREATE USER [StoreUsageUploadTempFlexTradeData.api] FOR LOGIN [StoreUsageUploadTempFlexTradeData.api]
    END
GO

ALTER USER [StoreUsageUploadTempFlexTradeData.api] WITH DEFAULT_SCHEMA=[System]
GO
