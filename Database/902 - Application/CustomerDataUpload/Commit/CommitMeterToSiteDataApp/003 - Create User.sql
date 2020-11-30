USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterToSiteDataApp')
    BEGIN
        CREATE USER [CommitMeterToSiteDataApp] FOR LOGIN [CommitMeterToSiteDataApp]
    END
GO

ALTER USER [CommitMeterToSiteDataApp] WITH DEFAULT_SCHEMA=[System]
GO
