USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreFlexTradeDataApp')
    BEGIN
        CREATE USER [StoreFlexTradeDataApp] FOR LOGIN [StoreFlexTradeDataApp]
    END
GO

ALTER USER [StoreFlexTradeDataApp] WITH DEFAULT_SCHEMA=[System]
GO
