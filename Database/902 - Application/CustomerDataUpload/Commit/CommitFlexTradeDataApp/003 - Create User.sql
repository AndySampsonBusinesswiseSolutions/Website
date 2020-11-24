USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitFlexTradeDataApp')
    BEGIN
        CREATE USER [CommitFlexTradeDataApp] FOR LOGIN [CommitFlexTradeDataApp]
    END
GO

ALTER USER [CommitFlexTradeDataApp] WITH DEFAULT_SCHEMA=[System]
GO
