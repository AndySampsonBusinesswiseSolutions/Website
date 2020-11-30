USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitLocalDistributionZoneToMeterDataApp')
    BEGIN
        CREATE USER [CommitLocalDistributionZoneToMeterDataApp] FOR LOGIN [CommitLocalDistributionZoneToMeterDataApp]
    END
GO

ALTER USER [CommitLocalDistributionZoneToMeterDataApp] WITH DEFAULT_SCHEMA=[System]
GO
