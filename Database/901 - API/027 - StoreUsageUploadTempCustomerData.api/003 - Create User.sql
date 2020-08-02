USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'StoreUsageUploadTempCustomerData.api')
    BEGIN
        CREATE USER [StoreUsageUploadTempCustomerData.api] FOR LOGIN [StoreUsageUploadTempCustomerData.api]
    END
GO

ALTER USER [StoreUsageUploadTempCustomerData.api] WITH DEFAULT_SCHEMA=[System]
GO
