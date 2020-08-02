USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'UpdateCustomerDetail.api')
    BEGIN
        CREATE USER [UpdateCustomerDetail.api] FOR LOGIN [UpdateCustomerDetail.api]
    END
GO

ALTER USER [UpdateCustomerDetail.api] WITH DEFAULT_SCHEMA=[System]
GO