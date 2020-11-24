USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetGenericProfileApp')
    BEGIN
        DROP LOGIN [GetGenericProfileApp]
    END
GO
