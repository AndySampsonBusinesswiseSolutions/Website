USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreSubMeterUsageData.api')
    BEGIN
        DROP LOGIN [StoreSubMeterUsageData.api]
    END
GO
