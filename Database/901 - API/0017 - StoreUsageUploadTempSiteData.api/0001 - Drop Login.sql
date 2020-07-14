USE [master]
GO

IF EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempSiteData.api')
    BEGIN
        DROP LOGIN [StoreUsageUploadTempSiteData.api]
    END
GO
