USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CleanUpCustomerDataUploadTempData.api')
    BEGIN
        CREATE USER [CleanUpCustomerDataUploadTempData.api] FOR LOGIN [CleanUpCustomerDataUploadTempData.api]
    END
GO

ALTER USER [CleanUpCustomerDataUploadTempData.api] WITH DEFAULT_SCHEMA=[System]
GO
