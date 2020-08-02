USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempSubMeterData.api')
    BEGIN
        DROP LOGIN [ValidateUsageUploadTempSubMeterData.api]
    END
GO
