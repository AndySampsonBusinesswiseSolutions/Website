USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'AddNewCustomerApp')
    BEGIN
        DROP LOGIN [AddNewCustomerApp]
    END
GO