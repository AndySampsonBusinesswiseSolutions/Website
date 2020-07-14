USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempMeterData.api')
    BEGIN
        DROP LOGIN [ValidateUsageUploadTempMeterData.api]
    END
GO
