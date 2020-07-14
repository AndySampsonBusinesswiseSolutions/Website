USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempSiteData.api')
    BEGIN
        DROP LOGIN [ValidateUsageUploadTempSiteData.api]
    END
GO
