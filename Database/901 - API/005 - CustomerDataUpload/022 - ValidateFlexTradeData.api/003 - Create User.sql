USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateFlexTradeData.api')
    BEGIN
        CREATE USER [ValidateFlexTradeData.api] FOR LOGIN [ValidateFlexTradeData.api]
    END
GO

ALTER USER [ValidateFlexTradeData.api] WITH DEFAULT_SCHEMA=[System]
GO
