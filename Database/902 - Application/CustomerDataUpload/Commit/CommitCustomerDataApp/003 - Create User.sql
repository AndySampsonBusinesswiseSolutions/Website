USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitCustomerDataApp')
    BEGIN
        CREATE USER [CommitCustomerDataApp] FOR LOGIN [CommitCustomerDataApp]
    END
GO

ALTER USER [CommitCustomerDataApp] WITH DEFAULT_SCHEMA=[System]
GO
