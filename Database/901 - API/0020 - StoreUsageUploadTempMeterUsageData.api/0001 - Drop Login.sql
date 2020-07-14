USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempMeterUsageData.api')
    BEGIN
        DROP LOGIN [StoreUsageUploadTempMeterUsageData.api]
    END
GO
