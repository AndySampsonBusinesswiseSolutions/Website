USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'AddNewCustomer.api')
    BEGIN
        DROP LOGIN [AddNewCustomer.api]
    END
GO