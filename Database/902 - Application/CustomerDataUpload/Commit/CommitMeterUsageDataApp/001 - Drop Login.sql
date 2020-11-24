USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterUsageDataApp')
    BEGIN
        DROP LOGIN [CommitMeterUsageDataApp]
    END
GO
