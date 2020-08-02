USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'AddNewCustomer.api')
    BEGIN
        CREATE USER [AddNewCustomer.api] FOR LOGIN [AddNewCustomer.api]
    END
GO

ALTER USER [AddNewCustomer.api] WITH DEFAULT_SCHEMA=[System]
GO