USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetProfileApp')
    BEGIN
        DROP LOGIN [GetProfileApp]
    END
GO
