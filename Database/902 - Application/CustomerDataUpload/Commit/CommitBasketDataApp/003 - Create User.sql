USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitBasketDataApp')
    BEGIN
        CREATE USER [CommitBasketDataApp] FOR LOGIN [CommitBasketDataApp]
    END
GO

ALTER USER [CommitBasketDataApp] WITH DEFAULT_SCHEMA=[System]
GO
