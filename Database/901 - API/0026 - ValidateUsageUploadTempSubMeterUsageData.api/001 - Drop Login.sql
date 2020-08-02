USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempSubMeterUsageData.api')
    BEGIN
        DROP LOGIN [ValidateUsageUploadTempSubMeterUsageData.api]
    END
GO
