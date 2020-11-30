USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidatePassword.api')
    BEGIN
        DROP LOGIN [ValidatePassword.api]
    END
GO