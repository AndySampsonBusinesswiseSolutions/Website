USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateCustomerData.api')
    BEGIN
        CREATE USER [ValidateCustomerData.api] FOR LOGIN [ValidateCustomerData.api]
    END
GO

ALTER USER [ValidateCustomerData.api] WITH DEFAULT_SCHEMA=[System]
GO
