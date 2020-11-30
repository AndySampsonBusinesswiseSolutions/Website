USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreFixedContractDataApp')
    BEGIN
        CREATE USER [StoreFixedContractDataApp] FOR LOGIN [StoreFixedContractDataApp]
    END
GO

ALTER USER [StoreFixedContractDataApp] WITH DEFAULT_SCHEMA=[System]
GO
