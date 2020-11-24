USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitGridSupplyPointToMeterDataApp')
    BEGIN
        CREATE USER [CommitGridSupplyPointToMeterDataApp] FOR LOGIN [CommitGridSupplyPointToMeterDataApp]
    END
GO

ALTER USER [CommitGridSupplyPointToMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
