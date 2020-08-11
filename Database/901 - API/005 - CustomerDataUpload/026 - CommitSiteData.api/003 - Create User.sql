USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitSiteData.api')
    BEGIN
        CREATE USER [CommitSiteData.api] FOR LOGIN [CommitSiteData.api]
    END
GO

ALTER USER [CommitSiteData.api] WITH DEFAULT_SCHEMA=[System]
GO
