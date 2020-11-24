USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetProfileIdApp')
    BEGIN
        DROP LOGIN [GetProfileIdApp]
    END
GO
