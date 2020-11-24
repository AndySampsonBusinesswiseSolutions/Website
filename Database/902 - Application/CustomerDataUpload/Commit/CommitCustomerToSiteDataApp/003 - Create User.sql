USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitCustomerToSiteDataApp')
    BEGIN
        CREATE USER [CommitCustomerToSiteDataApp] FOR LOGIN [CommitCustomerToSiteDataApp]
    END
GO

ALTER USER [CommitCustomerToSiteDataApp] WITH DEFAULT_SCHEMA=[System]
GO
