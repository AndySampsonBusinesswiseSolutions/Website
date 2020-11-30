USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateCustomerData.api')
    BEGIN
        DROP LOGIN [ValidateCustomerData.api]
    END
GO
