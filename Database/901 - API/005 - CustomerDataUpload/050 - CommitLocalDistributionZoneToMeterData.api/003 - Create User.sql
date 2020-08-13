USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitLocalDistributionZoneToMeterData.api')
    BEGIN
        CREATE USER [CommitLocalDistributionZoneToMeterData.api] FOR LOGIN [CommitLocalDistributionZoneToMeterData.api]
    END
GO

ALTER USER [CommitLocalDistributionZoneToMeterData.api] WITH DEFAULT_SCHEMA=[System]
GO
