USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidatePasswordApp')
    BEGIN
        DROP LOGIN [ValidatePasswordApp]
    END
GO