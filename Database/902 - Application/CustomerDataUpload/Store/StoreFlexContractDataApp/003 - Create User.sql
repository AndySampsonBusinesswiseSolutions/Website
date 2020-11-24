USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreFlexContractDataApp')
    BEGIN
        CREATE USER [StoreFlexContractDataApp] FOR LOGIN [StoreFlexContractDataApp]
    END
GO

ALTER USER [StoreFlexContractDataApp] WITH DEFAULT_SCHEMA=[System]
GO
