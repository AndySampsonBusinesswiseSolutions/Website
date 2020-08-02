USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateEmailAddress.api')
    BEGIN
        DROP LOGIN [ValidateEmailAddress.api]
    END
GO