USE [master]
GO

IF NOT EXISTS(SELECT TOP 1 1 FROM syslogins WHERE loginname = 'ValidateUsageUploadTempSubMeterUsageData.api')
    BEGIN
       CREATE LOGIN [ValidateUsageUploadTempSubMeterUsageData.api] WITH PASSWORD=N'kY4f4KaZCgJcHUnH', DEFAULT_DATABASE=[EMaaS], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
    END
GO
