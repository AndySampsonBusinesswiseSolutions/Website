USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapCustomerToChildCustomer.api')
    BEGIN
        DROP LOGIN [MapCustomerToChildCustomer.api]
    END
GO