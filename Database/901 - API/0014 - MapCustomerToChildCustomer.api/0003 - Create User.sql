USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'MapCustomerToChildCustomer.api')
    BEGIN
        CREATE USER [MapCustomerToChildCustomer.api] FOR LOGIN [MapCustomerToChildCustomer.api]
    END
GO

ALTER USER [MapCustomerToChildCustomer.api] WITH DEFAULT_SCHEMA=[System]
GO