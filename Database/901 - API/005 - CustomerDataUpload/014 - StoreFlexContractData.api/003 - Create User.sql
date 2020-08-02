USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreFlexContractData.api')
    BEGIN
        CREATE USER [StoreFlexContractData.api] FOR LOGIN [StoreFlexContractData.api]
    END
GO

ALTER USER [StoreFlexContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
