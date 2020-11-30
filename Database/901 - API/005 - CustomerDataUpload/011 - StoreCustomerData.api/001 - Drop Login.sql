USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreCustomerData.api')
    BEGIN
        DROP LOGIN [StoreCustomerData.api]
    END
GO
