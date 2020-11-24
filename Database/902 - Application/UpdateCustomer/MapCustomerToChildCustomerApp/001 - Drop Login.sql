USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'MapCustomerToChildCustomerApp')
    BEGIN
        DROP LOGIN [MapCustomerToChildCustomerApp]
    END
GO