USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CreateManageCustomersWebpage.api')
    BEGIN
        DROP LOGIN [CreateManageCustomersWebpage.api]
    END
GO
