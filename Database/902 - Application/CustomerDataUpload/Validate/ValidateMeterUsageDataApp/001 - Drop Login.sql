USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateMeterUsageDataApp')
    BEGIN
        DROP LOGIN [ValidateMeterUsageDataApp]
    END
GO
