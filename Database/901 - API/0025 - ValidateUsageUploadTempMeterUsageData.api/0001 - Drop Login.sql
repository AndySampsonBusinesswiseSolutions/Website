USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempMeterUsageData.api')
    BEGIN
        DROP LOGIN [ValidateUsageUploadTempMeterUsageData.api]
    END
GO
