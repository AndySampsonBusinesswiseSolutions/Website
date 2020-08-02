USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'UpdateCustomerDetail.api')
    BEGIN
        DROP LOGIN [UpdateCustomerDetail.api]
    END
GO