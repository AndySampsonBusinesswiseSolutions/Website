USE [EMaaS]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM sys.database_principals WHERE type = 'S' AND name = 'ProcessCustomerDataUploadValidation.api')
    BEGIN
        CREATE USER [ProcessCustomerDataUploadValidation.api] FOR LOGIN [ProcessCustomerDataUploadValidation.api]
    END
GO

ALTER USER [ProcessCustomerDataUploadValidation.api] WITH DEFAULT_SCHEMA=[System]
GO
