USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CommitContractMeterToProductData.api')
    BEGIN
        CREATE USER [CommitContractMeterToProductData.api] FOR LOGIN [CommitContractMeterToProductData.api]
    END
GO

ALTER USER [CommitContractMeterToProductData.api] WITH DEFAULT_SCHEMA=[System]
GO
