USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'GetMappedUsageDateIdApp')
    BEGIN
        DROP LOGIN [GetMappedUsageDateIdApp]
    END
GO
