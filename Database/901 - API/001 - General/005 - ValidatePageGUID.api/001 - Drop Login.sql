USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidatePageGUID.api')
    BEGIN
        DROP LOGIN [ValidatePageGUID.api]
    END
GO