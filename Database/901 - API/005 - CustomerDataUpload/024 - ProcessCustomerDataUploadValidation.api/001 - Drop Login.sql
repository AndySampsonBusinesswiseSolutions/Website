USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ProcessCustomerDataUploadValidation.api')
    BEGIN
        DROP LOGIN [ProcessCustomerDataUploadValidation.api]
    END
GO
