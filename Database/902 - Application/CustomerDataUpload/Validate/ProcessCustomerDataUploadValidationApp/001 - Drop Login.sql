USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ProcessCustomerDataUploadValidationApp')
    BEGIN
        DROP LOGIN [ProcessCustomerDataUploadValidationApp]
    END
GO
