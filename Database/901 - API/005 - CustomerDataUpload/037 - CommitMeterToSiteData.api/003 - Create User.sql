USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitMeterToSiteData.api')
    BEGIN
        CREATE USER [CommitMeterToSiteData.api] FOR LOGIN [CommitMeterToSiteData.api]
    END
GO

ALTER USER [CommitMeterToSiteData.api] WITH DEFAULT_SCHEMA=[System]
GO
