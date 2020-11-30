USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'UpdateCustomerDetailApp')
    BEGIN
        DROP LOGIN [UpdateCustomerDetailApp]
    END
GO