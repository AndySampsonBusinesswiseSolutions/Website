USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateMeterUsageData.api')
    BEGIN
        DROP LOGIN [ValidateMeterUsageData.api]
    END
GO
