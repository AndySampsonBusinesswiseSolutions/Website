USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreMeterDataApp')
    BEGIN
        DROP LOGIN [StoreMeterDataApp]
    END
GO
