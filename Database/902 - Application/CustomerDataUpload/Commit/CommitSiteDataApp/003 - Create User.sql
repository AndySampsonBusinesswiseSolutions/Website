USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitSiteDataApp')
    BEGIN
        CREATE USER [CommitSiteDataApp] FOR LOGIN [CommitSiteDataApp]
    END
GO

ALTER USER [CommitSiteDataApp] WITH DEFAULT_SCHEMA=[System]
GO
