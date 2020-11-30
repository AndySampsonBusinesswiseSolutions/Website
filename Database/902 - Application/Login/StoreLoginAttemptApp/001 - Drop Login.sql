USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreLoginAttemptApp')
    BEGIN
        DROP LOGIN [StoreLoginAttemptApp]
    END
GO