USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempMeterExemptionData.api')
    BEGIN
        DROP LOGIN [StoreUsageUploadTempMeterExemptionData.api]
    END
GO
