USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CreateManageCustomersWebpageApp')
    BEGIN
        CREATE USER [CreateManageCustomersWebpageApp] FOR LOGIN [CreateManageCustomersWebpageApp]
    END
GO

ALTER USER [CreateManageCustomersWebpageApp] WITH DEFAULT_SCHEMA=[System]
GO
