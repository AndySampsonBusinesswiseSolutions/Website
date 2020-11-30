USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'CommitSubMeterUsageData.api')
    BEGIN
        DROP LOGIN [CommitSubMeterUsageData.api]
    END
GO
