USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'MapCustomerToChildCustomerApp')
    BEGIN
        CREATE USER [MapCustomerToChildCustomerApp] FOR LOGIN [MapCustomerToChildCustomerApp]
    END
GO

ALTER USER [MapCustomerToChildCustomerApp] WITH DEFAULT_SCHEMA=[System]
GO