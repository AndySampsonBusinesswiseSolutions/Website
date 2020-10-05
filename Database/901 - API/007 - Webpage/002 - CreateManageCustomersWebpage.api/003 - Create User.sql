USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateManageCustomersWebpage.api')
    BEGIN
        CREATE USER [CreateManageCustomersWebpage.api] FOR LOGIN [CreateManageCustomersWebpage.api]
    END
GO

ALTER USER [CreateManageCustomersWebpage.api] WITH DEFAULT_SCHEMA=[System]
GO
