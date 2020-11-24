USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateProcessGUIDApp')
    BEGIN
        DROP LOGIN [ValidateProcessGUIDApp]
    END
GO