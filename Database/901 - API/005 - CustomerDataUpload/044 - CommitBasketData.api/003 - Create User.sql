USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitBasketData.api')
    BEGIN
        CREATE USER [CommitBasketData.api] FOR LOGIN [CommitBasketData.api]
    END
GO

ALTER USER [CommitBasketData.api] WITH DEFAULT_SCHEMA=[System]
GO
