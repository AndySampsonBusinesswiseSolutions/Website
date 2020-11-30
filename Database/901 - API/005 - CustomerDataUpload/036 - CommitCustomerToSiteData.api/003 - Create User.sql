USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitCustomerToSiteData.api')
    BEGIN
        CREATE USER [CommitCustomerToSiteData.api] FOR LOGIN [CommitCustomerToSiteData.api]
    END
GO

ALTER USER [CommitCustomerToSiteData.api] WITH DEFAULT_SCHEMA=[System]
GO
