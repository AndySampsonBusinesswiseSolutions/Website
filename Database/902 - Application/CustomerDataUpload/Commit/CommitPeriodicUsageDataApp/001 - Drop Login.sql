USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitPeriodicUsageDataApp')
    BEGIN
        DROP LOGIN [CommitPeriodicUsageDataApp]
    END
GO
