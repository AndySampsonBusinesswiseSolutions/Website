USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateFlexTradeDataApp')
    BEGIN
        CREATE USER [ValidateFlexTradeDataApp] FOR LOGIN [ValidateFlexTradeDataApp]
    END
GO

ALTER USER [ValidateFlexTradeDataApp] WITH DEFAULT_SCHEMA=[System]
GO
