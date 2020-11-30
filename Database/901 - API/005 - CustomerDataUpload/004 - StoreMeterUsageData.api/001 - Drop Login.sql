USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreMeterUsageData.api')
    BEGIN
        DROP LOGIN [StoreMeterUsageData.api]
    END
GO
