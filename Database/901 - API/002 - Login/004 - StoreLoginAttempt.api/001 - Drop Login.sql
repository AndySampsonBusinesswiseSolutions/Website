USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreLoginAttempt.api')
    BEGIN
        DROP LOGIN [StoreLoginAttempt.api]
    END
GO