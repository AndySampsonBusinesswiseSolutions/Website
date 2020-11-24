USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'AddNewCustomerApp')
    BEGIN
        CREATE USER [AddNewCustomerApp] FOR LOGIN [AddNewCustomerApp]
    END
GO

ALTER USER [AddNewCustomerApp] WITH DEFAULT_SCHEMA=[System]
GO