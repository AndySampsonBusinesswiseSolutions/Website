USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitMeterUsageData.api')
    BEGIN
        DROP LOGIN [CommitMeterUsageData.api]
    END
GO
