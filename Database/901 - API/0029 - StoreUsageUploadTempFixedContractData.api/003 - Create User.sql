USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreUsageUploadTempFixedContractData.api')
    BEGIN
        CREATE USER [StoreUsageUploadTempFixedContractData.api] FOR LOGIN [StoreUsageUploadTempFixedContractData.api]
    END
GO

ALTER USER [StoreUsageUploadTempFixedContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
