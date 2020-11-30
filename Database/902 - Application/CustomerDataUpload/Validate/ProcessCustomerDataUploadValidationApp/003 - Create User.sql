USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ProcessCustomerDataUploadValidationApp')
    BEGIN
        CREATE USER [ProcessCustomerDataUploadValidationApp] FOR LOGIN [ProcessCustomerDataUploadValidationApp]
    END
GO

ALTER USER [ProcessCustomerDataUploadValidationApp] WITH DEFAULT_SCHEMA=[System]
GO
