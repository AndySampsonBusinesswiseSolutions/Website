USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempMeterData.api')
    BEGIN
        DROP LOGIN [StoreUsageUploadTempMeterData.api]
    END
GO
