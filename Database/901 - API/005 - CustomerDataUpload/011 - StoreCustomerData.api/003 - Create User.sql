USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreCustomerData.api')
    BEGIN
        CREATE USER [StoreCustomerData.api] FOR LOGIN [StoreCustomerData.api]
    END
GO

ALTER USER [StoreCustomerData.api] WITH DEFAULT_SCHEMA=[System]
GO
