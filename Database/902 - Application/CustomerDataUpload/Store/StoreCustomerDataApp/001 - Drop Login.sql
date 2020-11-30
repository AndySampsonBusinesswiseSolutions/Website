USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreCustomerDataApp')
    BEGIN
        DROP LOGIN [StoreCustomerDataApp]
    END
GO
