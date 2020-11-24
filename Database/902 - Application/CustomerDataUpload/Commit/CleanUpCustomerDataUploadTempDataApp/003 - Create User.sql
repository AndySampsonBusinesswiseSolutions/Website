USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'CleanUpCustomerDataUploadTempDataApp')
    BEGIN
        CREATE USER [CleanUpCustomerDataUploadTempDataApp] FOR LOGIN [CleanUpCustomerDataUploadTempDataApp]
    END
GO

ALTER USER [CleanUpCustomerDataUploadTempDataApp] WITH DEFAULT_SCHEMA=[System]
GO
