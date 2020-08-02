USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateUsageUploadTempFixedContractData.api')
    BEGIN
        CREATE USER [ValidateUsageUploadTempFixedContractData.api] FOR LOGIN [ValidateUsageUploadTempFixedContractData.api]
    END
GO

ALTER USER [ValidateUsageUploadTempFixedContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
