USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitCustomerData.api')
    BEGIN
        DROP LOGIN [CommitCustomerData.api]
    END
GO
