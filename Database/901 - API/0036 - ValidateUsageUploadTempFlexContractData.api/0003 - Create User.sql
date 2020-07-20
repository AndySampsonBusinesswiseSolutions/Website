USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ValidateUsageUploadTempFlexContractData.api')
    BEGIN
        CREATE USER [ValidateUsageUploadTempFlexContractData.api] FOR LOGIN [ValidateUsageUploadTempFlexContractData.api]
    END
GO

ALTER USER [ValidateUsageUploadTempFlexContractData.api] WITH DEFAULT_SCHEMA=[System]
GO
