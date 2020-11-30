USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateProcessGUID.api')
    BEGIN
        DROP LOGIN [ValidateProcessGUID.api]
    END
GO