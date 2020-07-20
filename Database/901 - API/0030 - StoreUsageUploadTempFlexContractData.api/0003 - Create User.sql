USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreUsageUploadTempFlexContractData.api')
    BEGIN
        CREATE USER [StoreUsageUploadTempFlexContractData.api] FOR LOGIN [StoreUsageUploadTempFlexContractData.api]
    END
GO

ALTER USER [StoreUsageUploadTempFlexContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
