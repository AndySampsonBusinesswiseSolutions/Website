USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'StoreUsageUploadTempSiteData.api')
    BEGIN
       CREATE LOGIN [StoreUsageUploadTempSiteData.api] WITH PASSWORD=N'46PP5VdL6Djet8tA', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
