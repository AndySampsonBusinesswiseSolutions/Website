USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitGridSupplyPointToMeterData.api')
    BEGIN
        CREATE USER [CommitGridSupplyPointToMeterData.api] FOR LOGIN [CommitGridSupplyPointToMeterData.api]
    END
GO

ALTER USER [CommitGridSupplyPointToMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
