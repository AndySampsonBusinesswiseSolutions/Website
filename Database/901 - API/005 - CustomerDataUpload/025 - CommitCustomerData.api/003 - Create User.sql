USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitCustomerData.api')
    BEGIN
        CREATE USER [CommitCustomerData.api] FOR LOGIN [CommitCustomerData.api]
    END
GO

ALTER USER [CommitCustomerData.api] WITH DEFAULT_SCHEMA=[System]
GO
