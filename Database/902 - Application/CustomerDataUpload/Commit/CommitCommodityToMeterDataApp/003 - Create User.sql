USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitCommodityToMeterDataApp')
    BEGIN
        CREATE USER [CommitCommodityToMeterDataApp] FOR LOGIN [CommitCommodityToMeterDataApp]
    END
GO

ALTER USER [CommitCommodityToMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
