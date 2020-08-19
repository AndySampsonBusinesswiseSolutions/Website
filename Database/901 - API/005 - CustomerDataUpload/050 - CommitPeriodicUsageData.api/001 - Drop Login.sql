USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitPeriodicUsageData.api')
    BEGIN
        DROP LOGIN [CommitPeriodicUsageData.api]
    END
GO
