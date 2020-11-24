USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidatePageGUIDApp')
    BEGIN
        DROP LOGIN [ValidatePageGUIDApp]
    END
GO