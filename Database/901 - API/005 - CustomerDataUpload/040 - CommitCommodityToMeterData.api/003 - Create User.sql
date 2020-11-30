USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitCommodityToMeterData.api')
    BEGIN
        CREATE USER [CommitCommodityToMeterData.api] FOR LOGIN [CommitCommodityToMeterData.api]
    END
GO

ALTER USER [CommitCommodityToMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
