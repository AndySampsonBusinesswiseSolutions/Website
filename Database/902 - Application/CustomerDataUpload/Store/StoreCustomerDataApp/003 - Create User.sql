USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreCustomerDataApp')
    BEGIN
        CREATE USER [StoreCustomerDataApp] FOR LOGIN [StoreCustomerDataApp]
    END
GO

ALTER USER [StoreCustomerDataApp] WITH DEFAULT_SCHEMA=[System]
GO
