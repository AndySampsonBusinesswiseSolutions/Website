USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempSubMeterData.api')
    BEGIN
        DROP LOGIN [StoreUsageUploadTempSubMeterData.api]
    END
GO
