USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitTradeToBasketData.api')
    BEGIN
        CREATE USER [CommitTradeToBasketData.api] FOR LOGIN [CommitTradeToBasketData.api]
    END
GO

ALTER USER [CommitTradeToBasketData.api] WITH DEFAULT_SCHEMA=[System]
GO
