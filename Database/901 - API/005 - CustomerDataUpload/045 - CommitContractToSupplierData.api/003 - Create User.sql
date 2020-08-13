USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitContractToSupplierData.api')
    BEGIN
        CREATE USER [CommitContractToSupplierData.api] FOR LOGIN [CommitContractToSupplierData.api]
    END
GO

ALTER USER [CommitContractToSupplierData.api] WITH DEFAULT_SCHEMA=[System]
GO
