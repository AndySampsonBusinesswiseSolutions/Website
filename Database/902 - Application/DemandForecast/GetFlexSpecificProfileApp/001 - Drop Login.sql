USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetFlexSpecificProfileApp')
    BEGIN
        DROP LOGIN [GetFlexSpecificProfileApp]
    END
GO
