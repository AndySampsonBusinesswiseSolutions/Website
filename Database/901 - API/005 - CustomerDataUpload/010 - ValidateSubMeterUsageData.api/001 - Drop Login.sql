USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateSubMeterUsageData.api')
    BEGIN
        DROP LOGIN [ValidateSubMeterUsageData.api]
    END
GO
