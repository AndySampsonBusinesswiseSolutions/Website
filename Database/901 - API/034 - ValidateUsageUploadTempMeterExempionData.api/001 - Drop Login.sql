USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempMeterExempionData.api')
    BEGIN
        DROP LOGIN [ValidateUsageUploadTempMeterExempionData.api]
    END
GO
