USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreMeterUsageDataApp')
    BEGIN
        DROP LOGIN [StoreMeterUsageDataApp]
    END
GO
