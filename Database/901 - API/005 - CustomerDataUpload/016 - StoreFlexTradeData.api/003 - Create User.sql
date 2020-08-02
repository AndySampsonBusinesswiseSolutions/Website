USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreFlexTradeData.api')
    BEGIN
        CREATE USER [StoreFlexTradeData.api] FOR LOGIN [StoreFlexTradeData.api]
    END
GO

ALTER USER [StoreFlexTradeData.api] WITH DEFAULT_SCHEMA=[System]
GO
