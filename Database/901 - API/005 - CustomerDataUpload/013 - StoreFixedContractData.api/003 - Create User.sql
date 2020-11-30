USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreFixedContractData.api')
    BEGIN
        CREATE USER [StoreFixedContractData.api] FOR LOGIN [StoreFixedContractData.api]
    END
GO

ALTER USER [StoreFixedContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
