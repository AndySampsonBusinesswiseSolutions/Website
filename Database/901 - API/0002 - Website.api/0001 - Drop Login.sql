USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'Website.api')
    BEGIN
        DROP LOGIN [Website.api]
    END
GO