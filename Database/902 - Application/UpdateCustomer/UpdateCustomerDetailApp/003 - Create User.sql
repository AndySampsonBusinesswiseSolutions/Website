USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'UpdateCustomerDetailApp')
    BEGIN
        CREATE USER [UpdateCustomerDetailApp] FOR LOGIN [UpdateCustomerDetailApp]
    END
GO

ALTER USER [UpdateCustomerDetailApp] WITH DEFAULT_SCHEMA=[System]
GO