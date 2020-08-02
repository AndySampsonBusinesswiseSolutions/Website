USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempSubMeterUsageData.api')
    BEGIN
        DROP LOGIN [StoreUsageUploadTempSubMeterUsageData.api]
    END
GO
